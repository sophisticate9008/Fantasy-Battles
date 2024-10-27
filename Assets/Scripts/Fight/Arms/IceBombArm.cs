using FightBases;
using UnityEngine;
namespace Arms
{
    public class IceBombArm : ArmBase
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