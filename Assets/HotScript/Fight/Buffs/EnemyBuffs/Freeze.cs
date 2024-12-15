
using UnityEngine;


public class Freeze : BuffBase
{
    //特效控制器
    IEffectController controller;
    public Freeze(string buffName, float duration, GameObject selfObj, GameObject enemyObj) : base(buffName, duration, selfObj, enemyObj)
    {

    }
    public override void Effect()
    {
        EffectControl();
        controller = EffectManager.Instance.Play(EnemyObj, "FreezenEffect");
    }

    public override void Remove()
    {
        RemoveControl();
        EffectManager.Instance.Stop(controller);
    }
}
