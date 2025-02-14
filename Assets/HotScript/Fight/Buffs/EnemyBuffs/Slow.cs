
using UnityEngine;


public class Slow : BuffBase
{
    private float slowRate = 0.3f;
    private float originSpeed;
    private float MaxSlowRate => EnemyBase.Config.MaxSlowRate;
    public Slow(string buffName, float duration, GameObject selfObj, GameObject enemyObj, float slowRate) : base(buffName, duration, selfObj, enemyObj)
    {
        this.slowRate = slowRate;
        EnemyBase.Config.SlowRates.Add(slowRate);
    }
    public override void Effect()
    {
        originSpeed = EnemyBase.Config.Speed;
        EnemyBase.Config.Speed = originSpeed * (1 - MaxSlowRate);
        AnimatorManager.Instance.PlayAnim(EnemyBase.animator, EnemyBase.animator.speed * (1 - MaxSlowRate));
    }

    public override void Remove()
    {
        EnemyBase.Config.SlowRates.Remove(slowRate);
        EnemyBase.Config.Speed = originSpeed * (1 - MaxSlowRate);
        AnimatorManager.Instance.PlayAnim(EnemyBase.animator, EnemyBase.animator.speed / (1 - MaxSlowRate));
    }
}
