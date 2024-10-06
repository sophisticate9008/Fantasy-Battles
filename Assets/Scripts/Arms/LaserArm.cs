using MyBase;
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
        FindTargetNearestOrElite();

    }
    public override void Attack()
    {

        Vector3 baseDirection = (TargetEnemy.transform.position - transform.position).normalized;
        ArmChildBase obj = GetOneFromPool();
        obj.transform.position = transform.position;
        obj.Direction = baseDirection;
        obj.TargetEnemyByArm = TargetEnemy;
        obj.Init();
    }
}