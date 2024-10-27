using FightBases;
using UnityEngine;
namespace Arms
{
    public class PressureCutterArm : ArmBase
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
            AttackMultipleOnce();
        }
    }
}