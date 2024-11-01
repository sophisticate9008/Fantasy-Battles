namespace ArmConfigs {
    public class IceGeneratorConfig: ArmConfigBase {
        public override void Init()
        {
            base.Init();
            Tlc = 1;
            RangeFire = 8;
            OnType = "stay";
            DamageType = "ice";
            ComponentStrs.Add("冰冻");
            Duration = 3f;
            AttackCd = 0.2f;
            AttackCount = 1;
            ComponentStrs.Add("减速");
            Cd = 5;
        }
    }
}