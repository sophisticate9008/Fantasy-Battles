
public class BoomFireBallConfig : ArmConfigBase, IPenetrable, IBoomable, IHoldable,IReboundable,IFissionable
{
    public virtual int PenetrationLevel { get; set; } = 1;
    
    public ArmConfigBase BoomChildConfig => ConfigManager.Instance.GetConfigByClassName("BoomFireBallBoom") as BoomFireBallBoomConfig;

    public ArmConfigBase HoldChildConfig => ConfigManager.Instance.GetConfigByClassName("BoomFireBallHold") as BoomFireBallHoldConfig;

    public int ReboundCount { get ; set ; } = 0;

    public ArmConfigBase FissionableChildConfig => ConfigManager.Instance.GetConfigByClassName("BoomFireBallFission") as BoomFireBallFissionConfig;

    public string FindType { get ; set; } ="random";

    public override void Init()
    {

        base.Init();
        Speed = 15;
        OnType = "enter";
        ScopeRadius = 10f;
        ComponentStrs.Add("穿透");
        ComponentStrs.Add("爆炸");
        MaxForce = 150;
        FireTime = 0;
    }
}
