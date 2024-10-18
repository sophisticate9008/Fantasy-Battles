using MyBase;

namespace ArmConfigs
{
    public class ElectroPenetrateConfig : ArmConfigBase{

        public override void Init()
        {
            base.Init();
            Name = "电磁穿透";
            Description = "电磁穿透";
            ScopeRadius = 2;
            OnType = "enter";
            DamageType = "elec";
            Tlc = 2;
            CritRate = 0.2f;
            AttackCd = 0.5f;
            Cd = 5f;
            AttackCount = 5;
            Duration = 0.3f;
            RangeFire = 9;
        }
    }
}
