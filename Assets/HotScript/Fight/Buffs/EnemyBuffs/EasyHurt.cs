using UnityEngine;

public class EasyHurt : BuffBase
{
    IEffectController controller;
    float degree;
    public EasyHurt(string buffName, float duration, GameObject selfObj, GameObject enemyObj, float degree) : base(buffName, duration, selfObj, enemyObj)
    {
        this.degree = degree;
    }

    public override void Effect()
    {
        EnemyBase.EasyHurt += degree;
    }

    public override void Remove()
    {
        EnemyBase.EasyHurt -= degree;
    }
}