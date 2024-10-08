using MyBase;
using UnityEngine;

namespace TheBuffs
{
    public class DebuffSlow : BuffBase
    {
        private float slowRate = 0.3f;
        private float originSpeed;

        public DebuffSlow(string buffName, float duration,GameObject selfObj, GameObject enemyObj) : base(buffName, duration,selfObj, enemyObj)
        {

        }
        public override void Effect()
        {
            originSpeed = EnemyBase.Config.Speed;
            EnemyBase.Config.Speed = originSpeed * slowRate;
        }

        public override void Remove()
        {
            EnemyBase.Config.Speed = originSpeed;
        }
    }
}