namespace ArmConfigs
{
    public class TntBoomConfig : ArmConfigBase
    {
        public override void Init()
        {
            base.Init(); 
            Duration = 0.5f;
            OnType = "enter";
            Tlc = 0.5f;
            DamageType = "fire";
            MaxForce = 150;
        }
    }
}
