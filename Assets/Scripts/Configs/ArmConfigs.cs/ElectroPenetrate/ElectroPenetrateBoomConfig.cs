namespace ArmConfigs
{
    public class ElectroPenetrateBoomConfig : ArmConfigBase
    {
        public override void Init()
        {
            base.Init();
            Duration = 0.5f;
            OnType = "enter";
            Tlc = 0.5f;
            DamageType = "elec";
        }
    }
}