
using System.Collections;
using UnityEngine;
using YooAsset;

public class GroundStab : ArmChildBase
{
    // 召唤特效cd
    readonly float callEffectCd = 0.3f;
    GroundStabConfig ConcreteConfig => Config as GroundStabConfig;
    GameObject EffectPrefab => YooAssets.LoadAssetSync<GameObject>("GroundStabEffect").AssetObject as GameObject;
    //创建特效池子
    public override void Init()
    {
        base.Init();
        GameObject effectPrefab = YooAssets.LoadAssetSync<GameObject>("GroundStabEffect").AssetObject as GameObject;
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
            }
            TheEffect.transform.position = transform.position;
            //防止父对象消失丢失对特效的控制
            ToolManager.Instance.SetTimeout(() => ReturnToPool(TheEffect), callEffectCd);
            yield return new WaitForSeconds(callEffectCd);
        }
    }
    GameObject GetEffectOneFromPool()
    {
        return ObjectPoolManager.Instance.GetFromPool("GroundStabEffect", EffectPrefab);
    }
    void ReturnToPool(GameObject obj)
    {
        ObjectPoolManager.Instance.ReturnToPool("GroundStabEffect", obj);
    }

}
