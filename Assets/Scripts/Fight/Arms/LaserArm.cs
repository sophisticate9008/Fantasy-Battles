using FightBases;
using Unity.Mathematics;
using UnityEngine;
using YooAsset;

public class LaserArm : ArmBase
{

    public override void FisrtFindTarget()
    {

        FindTargetNearestOrElite();
    }
    public override void OtherFindTarget()
    {
        FindRandomTarget();

    }
    public override void Attack()
    {

        
        ArmChildBase obj = GetOneFromPool();
        obj.transform.position = transform.position;
        obj.SetDirectionToTarget();
        obj.TargetEnemyByArm = TargetEnemy;
        obj.Init();
    }
}