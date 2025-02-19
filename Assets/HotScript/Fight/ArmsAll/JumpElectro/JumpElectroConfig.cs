
public class JumpElectroConfig : ArmConfigBase,IBoomable
{
    public float PathDamageTlc { get; set; } = 0.1f;
    public int JumpCount { get; set; } = 5;

    public ArmConfigBase BoomChildConfig => ConfigManager.Instance.GetConfigByClassName("JumpElectroBoom") as JumpElectroBoomConfig;

    public override void Init()
    {
        base.Init();
        AttackCd = 0.2f;
        OnType = "enter";
        ComponentStrs.Add("麻痹");
    }
}

