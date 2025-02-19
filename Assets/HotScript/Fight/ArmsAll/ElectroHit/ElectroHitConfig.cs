

public class ElectroHitConfig : ArmConfigBase, IBoomable,IFissionable
{
    public ArmConfigBase BoomChildConfig =>
        ConfigManager.Instance.GetConfigByClassName("ElectroHitBoom") as ElectroHitBoomConfig;

    public ArmConfigBase FissionableChildConfig => throw new System.NotImplementedException();

    public string FindType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    
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

