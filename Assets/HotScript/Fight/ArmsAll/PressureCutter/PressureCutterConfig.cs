

namespace ArmConfigs
{
    public class PressureCutterConfig : ArmConfigBase, IPenetrable, IMultipleable
    {
        public int PenetrationLevel { get; set; } = 1;
        public int MultipleLevel { get; set; } = 1;
        public float AngleDifference { get; set; } = 5;

        public override void Init()
        {
            RangeFire = 8;
            Speed = 8;
            OnType = "enter";
            DamageType = "wind";
            DamagePos = "all";
            Cd = 3;
            ScopeRadius = 10f;
            SelfScale = 1;
            Tlc = 1;
            ComponentStrs.Add("穿透");
            MaxForce = 100;
        }
    }
}