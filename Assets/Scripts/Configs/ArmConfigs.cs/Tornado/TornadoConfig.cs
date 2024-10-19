using MyBase;

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
            Cd = 30f;
            Duration = 20f;
            AttackCount = 1;
            AttackCd = 0.2f;
            OnType = "stay";
            DamageType = "wind";
            ScopeRadius = 15f;
            Speed = 2f;
            SelfScale = 10f;
        }
    }
}
