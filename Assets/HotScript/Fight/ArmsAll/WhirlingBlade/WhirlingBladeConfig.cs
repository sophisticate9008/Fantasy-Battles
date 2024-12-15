using FightBases;
using MyEnums;

namespace ArmConfigs
{
    public class WhirlingBladeConfig : ArmConfigBase, IReboundable 
    {
        public int ReboundCount { get; set ; } = 999;

        public override void Init()
        {
            base.Init();
            AttackCount = 1;
            Cd = 5;
            Tlc = 1;
            DamageType = "energy";
            OnType = "stay";
            AttackCd = 0.2f;
            AttackCount = 1;
            RangeFire = 10;
            CdType = CdTypes.WaitLast;
            ControlBy = ControlBy.Arm;
            Duration = 10;
            ComponentStrs.Add("反弹");
            ComponentStrs.Add("阻尼");
            Speed = 3;
            ScopeRadius = 6;
        }
    }
}