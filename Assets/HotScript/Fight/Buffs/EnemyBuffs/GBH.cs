using UnityEngine;

public class GBH : BuffBase
{
    static IEffectController controller;
    float GBHRate = 0;
    public GBH(string buffName, float duration, GameObject selfObj, GameObject enemyObj, float GBHRate) : base(buffName, duration, selfObj, enemyObj)
    {
        this.GBHRate = GBHRate;
    }

    public override void Effect()
    {
        if (EnemyBase.GBHRate == 0)
        {
            controller = EffectManager.Instance.Play(EnemyObj, "GBHEffect");
        }
        EnemyBase.GBHRate += GBHRate;

    }

    public override void Remove()
    {
        EnemyBase.GBHRate -= GBHRate;
        if (EnemyBase.GBHRate == 0)
        {
            controller.Stop();
        }
    }
}