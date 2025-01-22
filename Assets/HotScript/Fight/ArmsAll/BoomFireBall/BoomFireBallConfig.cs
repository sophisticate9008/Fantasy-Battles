
public class BoomFireBallConfig : ArmConfigBase, IPenetrable, IBoomable, IHoldable
{
    public int PenetrationLevel { get; set; } = 2;
    public ArmConfigBase BoomChildConfig => ConfigManager.Instance.GetConfigByClassName("BoomFireBallBoom") as BoomFireBallBoomConfig;

    public ArmConfigBase HoldChildConfig => ConfigManager.Instance.GetConfigByClassName("BoomFireBallHold") as BoomFireBallHoldConfig;

    public override void Init()
    {

        base.Init();
        Speed = 15;
        OnType = "enter";
        ScopeRadius = 10f;
        ComponentStrs.Add("穿透");
        ComponentStrs.Add("爆炸");
    }
}
