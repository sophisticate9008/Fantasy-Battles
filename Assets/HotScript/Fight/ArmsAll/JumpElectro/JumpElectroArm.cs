



public class JumpElectroArm : ArmBase
{
    public override void Attack()
    {

        ArmChildBase obj = GetOneFromPool();
        obj.transform.position = TargetEnemy.transform.position;
        obj.TargetEnemyByArm = TargetEnemy;
        obj.Init();
    }
}

