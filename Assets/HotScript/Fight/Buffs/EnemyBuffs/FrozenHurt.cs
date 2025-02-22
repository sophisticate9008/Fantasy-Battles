using UnityEngine;

public class FrozenHurt : BuffBase
{
    static IEffectController controller;
    public FrozenHurt(string buffName, float duration, GameObject selfObj, GameObject enemyObj) : base(buffName, duration, selfObj, enemyObj)
    {
    }

    public override void Effect()
    {
        if(EnemyBase.FrozenHurtCount == 0) {
            controller = EffectManager.Instance.Play(EnemyObj, "FrozenHurtEffect");
        }
        EnemyBase.FrozenHurtCount += 1;
        EnemyBase.FrozenHurtCount = Mathf.Max(EnemyBase.FrozenHurtCount, ArmUtil.globalConfig.frozenHurtMaxCount);
        
    }

    public override void Remove()
    {
        EnemyBase.FrozenHurtCount -= 1;
        if(EnemyBase.FrozenHurtCount == 0 ) {
            controller.Stop();
        }
        EnemyBase.FrozenHurtCount = Mathf.Min(EnemyBase.FrozenHurtCount, 0);
    }
}