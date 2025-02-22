

public class ElectroHitConfig : ArmConfigBase, IBoomable,IFissionable,IHoldable
{
    public ArmConfigBase BoomChildConfig =>
        ConfigManager.Instance.GetConfigByClassName("ElectroHitBoom") as ElectroHitBoomConfig;

    public ArmConfigBase FissionableChildConfig => ConfigManager.Instance.GetConfigByClassName("ElectroHitFission") as ElectroHitFissionConfig;

    public string FindType { get ; set; } = "random";

    public ArmConfigBase HoldChildConfig => ConfigManager.Instance.GetConfigByClassName("ElectroHitHold") as ElectroHitHoldConfig;

    public override void Init()
    {
        base.Init();
        ComponentStrs.Add("麻痹");
        ScopeRadius = 2;
        OnType = "enter";
        CritRate = 0.2f;
        Duration = 0.3f;
    }
}

