using FightBases;
using UnityEngine;

namespace TheBuffs
{
    public class Slow : BuffBase
    {
        private float slowRate = 0.3f;
        private float originSpeed;

        public Slow(string buffName, float duration,GameObject selfObj, GameObject enemyObj, float slowRate) : base(buffName, duration,selfObj, enemyObj)
        {
            this.slowRate = slowRate;
        }
        public override void Effect()
        {
            originSpeed = EnemyBase.Config.Speed;
            EnemyBase.Config.MaxSlowRate = Mathf.Max(EnemyBase.Config.MaxSlowRate, slowRate);

            EnemyBase.Config.Speed = originSpeed * EnemyBase.Config.MaxSlowRate;
            AnimatorManager.Instance.PlayAnim(EnemyBase.animator, 1 - slowRate);
        }

        public override void Remove()
        {
            EnemyBase.Config.Speed = originSpeed;
            EnemyBase.Config.MaxSlowRate = 0;
            AnimatorManager.Instance.PlayAnim(EnemyBase.animator, 1);
        }
    }
}