
using FightBases;
using UnityEngine;

namespace Arms
{
    public class CarArm : ArmBase
    {
        public override void Attack()
        {

            ArmChildBase obj = GetOneFromPool();
            Vector3 pos = transform.position;
            pos.x = TargetEnemy.transform.position.x;
            obj.transform.position = pos;
            obj.Direction = Vector3.up;
            obj.TargetEnemyByArm = TargetEnemy;
            obj.Init();
        }
    }

}