namespace ArmConfigs {
    public class CarConfig: ArmConfigBase {
        public override void Init()
        {
            base.Init();
            OnType = "stay";
            DamageType = "fire";
            Tlc = 0.1f;
            AttackCount = 1;
            AttackCd = 0.1f;
            RangeFire = 10;
            Speed = 1.5f;
            Cd = 10;
            MaxForce = 50;
            ComponentStrs.Add("减速");
            SlowDegree = 0.8f;
            SlowTime = 2;
            ComponentStrs.Add("阻尼");
        }
    }
}