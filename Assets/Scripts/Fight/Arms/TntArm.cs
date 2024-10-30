using FightBases;
using UnityEngine;
namespace Arms
{
    public class TntArm : ArmBase
    {
        public override void Attack()
        {
            
            ArmChildBase obj = GetOneFromPool();
            obj.transform.position = transform.position;
            obj.TargetEnemyByArm = TargetEnemy;
            obj.SetDirectionToTarget();
            obj.Init();
        }
    }
}