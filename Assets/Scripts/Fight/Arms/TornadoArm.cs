using FightBases;

public class TornadoArm : ArmBase
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
        obj.transform.position = TargetEnemy.transform.position;
        obj.Init();
    }
    
}