namespace ArmConfigs {
    public class CarConfig: ArmConfigBase {
        public override void Init()
        {
            base.Init();
            OnType = "stay";
            DamageType = "fire";
            Tlc = 0.5f;
            AttackCount = 1;
            AttackCd = 0.1f;
            RangeFire = 10;
            Speed = 1;
            Cd = 10;
        }
    }
}