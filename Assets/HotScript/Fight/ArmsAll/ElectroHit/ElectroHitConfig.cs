

public class ElectroHitConfig : ArmConfigBase, IBoomable
{
    public ArmConfigBase BoomChildConfig =>
        ConfigManager.Instance.GetConfigByClassName("ElectroHitBoom") as ElectroHitBoomConfig;
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

