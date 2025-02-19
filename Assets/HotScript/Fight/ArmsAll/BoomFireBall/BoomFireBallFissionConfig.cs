public class BoomFireBallFissionConfig : ArmConfigBase, IMultipleable, IPenetrable
{
    public BoomFireBallConfig BoomFireBallConfig => ArmUtil.boomFireBallConfig;
    public int MultipleLevel { get; set; } = 3;
    public float AngleDifference { get; set; } = 20f;
    public int PenetrationLevel { get => BoomFireBallConfig.PenetrationLevel; set { } }
    public override float Tlc => BoomFireBallConfig.Tlc * 0.3f;
    public override float Speed => BoomFireBallConfig.Speed;
    public override void Init()
    {
        base.Init();
        ComponentStrs.Add("穿透");
        DamageType = "fire";
        OnType = "enter";
    }
}