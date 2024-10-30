
using FightBases;
using UnityEngine;

public class UavArm : ArmBase
{
    public override void Attack()
    {
        ArmChildBase obj = GetOneFromPool();
        obj.transform.position = TargetEnemy.transform.position;
        var _ = CommonUtil.AsList(TargetEnemy);
        var enemys = obj.FindTargetInScope(exceptObjs: _, centerObj: TargetEnemy);
        //有敌人 两点之间形成方向

        if(enemys.Count > 0) {
            Vector3 direction = (obj.TargetEnemy.transform.position - TargetEnemy.transform.position).normalized;
            obj.Direction = direction;

        }else {
            obj.TargetEnemyByArm = TargetEnemy;
            Vector3 randomDirection = Random.onUnitSphere;
            obj.Direction = randomDirection;
        }
        
        obj.Init();
    }
}