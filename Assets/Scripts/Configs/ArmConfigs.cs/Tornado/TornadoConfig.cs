using MyBase;

namespace ArmConfigs
{
    public class TornadoConfig : ArmConfigBase
    {
        public override void Init()
        {
            Tlc = 0.1f;
            Name = "龙卷";
            Description = "龙卷攻击";
            RangeFire = 8;
            Cd = 10f;
            Duration = 6f;
            AttackCount = 1;
            AttackCd = 0.2f;
            OnType = "stay";
            DamageType = "wind";
            ScopeRadius = 15f;

        }
    }
}
