using FightBases;

namespace Arms
{
    public class IceGeneratorArm : ArmBase
    {
        public override void Attack()
        {

            ArmChildBase obj = GetOneFromPool();
            obj.transform.position = TargetEnemy.transform.position;
            obj.TargetEnemyByArm = TargetEnemy;
            obj.Init();
        }
    }
}