using FightBases;

namespace ArmConfigs
{
    public class TntHoldConfig : ArmConfigBase
    {
        public override void Init()
        {
            base.Init();
            AttackCount = 1;
            AttackCd = 0.5f;
            OnType = "stay";
            DamageType = "fire";
            RangeFire = 10;
            Duration = 3;
            Tlc = 1;
            DamagePos = "land";
        }
    }
}