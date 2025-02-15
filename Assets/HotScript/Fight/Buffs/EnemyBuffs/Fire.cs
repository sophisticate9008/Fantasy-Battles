using System.Collections;

using UnityEngine;


public class Fire : BuffBase
{
    float tlc;
    bool lasting = false;
    IEffectController controller;
    public Fire(string buffName, float duration, GameObject selfObj, GameObject enemyObj, float tlc) : base(buffName, duration, selfObj, enemyObj)
    {
        this.tlc = tlc;
    }

    public override void Effect()
    {
        lasting = true;
        EnemyObj.GetComponent<MonoBehaviour>().StartCoroutine(LastingFlame());
        controller = EffectManager.Instance.Play(EnemyObj, "FireEffect");
        
    }

    public override void Remove()
    {
        lasting = false;
        EffectManager.Instance.Stop(controller);
    }
    IEnumerator LastingFlame()
    {
        while (lasting)
        {

            if (ArmChildBase != null)
            {
                FighteManager.Instance.SelfDamageFilter(EnemyObj, SelfObj, true, ArmChildBase.Config.FirePercentage, damageType: "fire", tlc: tlc);
            }
            else
            {
                FighteManager.Instance.SelfDamageFilter(EnemyObj, SelfObj, true, 0, damageType: "fire", tlc: tlc);
            }
            yield return new WaitForSeconds(0.49f);
        }



    }
}
