namespace ArmConfigs
{
    public class AirDropBombConfig : ArmConfigBase
    {
        public override void Init()
        {
            base.Init();
            Duration = 0.5f;
            OnType = "enter";
            Tlc = 2f;
            DamageType = "ad";
            ComponentStrs.Add("眩晕");
            AttackCd = 0.5f;
            AttackCount = 1;
            Cd = 5;
            RangeFire = 10;
            MaxForce = 100;
        }
    }
}