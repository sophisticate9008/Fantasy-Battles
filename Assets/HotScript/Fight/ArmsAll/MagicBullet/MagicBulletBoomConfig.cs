namespace ArmConfigs
{
    public class BulletBoomConfig : ArmConfigBase
    {
        public override void Init()
        {
            base.Init();
            Duration = 0.5f;
            OnType = "enter";
            Tlc = 0.5f;
            DamageType = "ad";
            MaxForce = 150;
        }
    }
}