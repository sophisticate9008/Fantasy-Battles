

public class TornadoArm : ArmBase
{

    public override void Attack()
    {

        ArmChildBase obj = GetOneFromPool();
        obj.transform.position = TargetEnemy.transform.position;
        obj.Init();
    }
    
}