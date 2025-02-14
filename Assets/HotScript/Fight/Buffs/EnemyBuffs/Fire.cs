using System.Collections;

using UnityEngine;


public class Fire : BuffBase
{
    float tlc;
    IEffectController controller;
    public Fire(string buffName, float duration, GameObject selfObj, GameObject enemyObj, float tlc) : base(buffName, duration, selfObj, enemyObj)
    {
        this.tlc = tlc;
    }

    public override void Effect()
    {
        EnemyObj.GetComponent<MonoBehaviour>().StartCoroutine(LastingFlame());
        controller = EffectManager.Instance.Play(EnemyObj, "FireEffect");
    }

    public override void Remove()
    {
        EnemyObj.GetComponent<MonoBehaviour>().StopCoroutine(LastingFlame());
        EffectManager.Instance.Stop(controller);
    }
    IEnumerator LastingFlame()
    {
        yield return new WaitForSeconds(0.49f);
        FighteManager.Instance.SelfDamageFilter(EnemyObj, SelfObj, true, ArmChildBase.Config.FirePercentage, damageType: "fire", tlc: tlc);

    }
}
