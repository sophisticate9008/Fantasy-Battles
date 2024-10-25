using FightBases;
using MyEnums;

namespace ArmConfigs
{
    public class TornadoConfig : ArmConfigBase
    {
        public float DragDegree{get;set;} = 1f;
        
        public override void Init()
        {
            Tlc = 0.1f;
            Name = "龙卷";
            Description = "龙卷攻击";
            RangeFire = 8;
            Cd = 10f;
            Duration = 15f;
            AttackCount = 2;
            AttackCd = 0.2f;
            OnType = "stay";
            DamageType = "wind";
            ScopeRadius = 15f;
            Speed = 2f;
            SelfScale = 10f;
            CdType = CdTypes.WaitLast;
            ControlBy = ControlBy.Arm;
        }
    }
}
