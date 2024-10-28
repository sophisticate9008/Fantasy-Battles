namespace ArmConfigs
{
    public class EnergyRayConfig :ArmConfigBase {
        public override void Init()
        {
            base.Init();
            AttackCount = 1;
            Cd = 10;
            Tlc = 1;
            DamageType = "energy";
            
        }
    }
}
