


public class FlameOrbArm : ArmBase
{
    public override void Attack()
    {
        ArmChildBase obj = GetOneFromPool();
        obj.transform.position = transform.position;
        obj.TargetEnemyByArm = TargetEnemy;
        obj.Init();
    }
}
