
using System.Collections;
using UnityEngine;
using YooAsset;

public class GroundStab : ArmChildBase
{
    // 召唤特效cd
    readonly float callEffectCd = 0.3f;
    GroundStabConfig ConcreteConfig => Config as GroundStabConfig;
    GameObject EffectPrefab => CommonUtil.GetAssetByName<GameObject>("GroundStabEffect");
    //创建特效池子
    public override void Init()
    {
        base.Init();
        GameObject effectPrefab = CommonUtil.GetAssetByName<GameObject>("GroundStabEffect");
        //创建特效池子
        ObjectPoolManager.Instance.CreatePool("GroundStabEffect", effectPrefab, 10, 50);
        StartCoroutine(CallEffect());
    }
    IEnumerator CallEffect()
    {
        while (true)
        {
            GameObject TheEffect = GetEffectOneFromPool();
            if (ConcreteConfig.isFire) {
                TheEffect.transform.RecursiveFind("fire").gameObject.SetActive(true);
                ChangeEffectScale(TheEffect.transform,false);
            }
            TheEffect.transform.position = transform.position;
            //防止父对象消失丢失对特效的控制
            ToolManager.Instance.SetTimeout(() => EffectReturnToPool(TheEffect), callEffectCd);
            yield return new WaitForSeconds(callEffectCd);
        }
    }
    void ChangeEffectScale( Transform effect,bool isRecovery) {

        if(isRecovery) {
            effect.localScale /= Config.SelfScale;
        }else {
            effect.localScale *= Config.SelfScale;
        }
    }
    GameObject GetEffectOneFromPool()
    {
        return ObjectPoolManager.Instance.GetFromPool("GroundStabEffect", EffectPrefab);
    }
    void EffectReturnToPool(GameObject obj)
    {
        ChangeEffectScale(obj.transform,true);
        ObjectPoolManager.Instance.ReturnToPool("GroundStabEffect", obj);
    }

}
