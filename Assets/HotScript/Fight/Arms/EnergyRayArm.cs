using FightBases;

namespace Arms
{
    public class EnergyRayArm : ArmBase
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
}