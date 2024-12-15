
using UnityEngine;


public class Dizzy : BuffBase
{
    IEffectController controller;
    public Dizzy(string buffName, float duration, GameObject selfObj, GameObject enemyObj) : base(buffName, duration, selfObj, enemyObj)
    {

    }
    public override void Effect()
    {
        EffectControl();
        controller = EffectManager.Instance.Play(EnemyObj, "DizzyEffect");
    }

    public override void Remove()
    {
        RemoveControl();
        EffectManager.Instance.Stop(controller);
    }
}


