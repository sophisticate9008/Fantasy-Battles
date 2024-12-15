using FightBases;
using UnityEngine;
using YooAsset;

public class LaserArm : ArmBase
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