
using UnityEngine;


public class Palsy : BuffBase
{
    IEffectController controller;
    public Palsy(string buffName, float duration, GameObject selfObj, GameObject enemyObj) : base(buffName, duration, selfObj, enemyObj)
    {

    }
    public override void Effect()
    {

        EffectControl();
        controller = EffectManager.Instance.Play(EnemyObj, "PalsyEffect");
    }

    public override void Remove()
    {
        RemoveControl();
        EffectManager.Instance.Stop(controller);
    }
}
