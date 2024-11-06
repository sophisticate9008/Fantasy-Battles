using FightBases;

namespace Arms
{
    public class FuelBulletArm : ArmBase
    {
        public override void Attack()
        {
            ArmChildBase obj = GetOneFromPool();
            obj.transform.position = transform.position;
            obj.TargetEnemyByArm = TargetEnemy;
            obj.Init();
        }
    }
}