
public class LaserFissionConfig : LaserConfig
{
    public LaserConfig LaserConfig => ConfigManager.Instance.GetConfigByClassName("Laser") as LaserConfig;
    public override float Tlc { get => LaserConfig.Tlc * 0.1f; }
    public override bool IsFlame => LaserConfig.IsFlame;
    public override float CritRate => LaserConfig.CritRate;
    public int FissionLevel = 0;
    public override void Init()
    {
        base.Init();
        ScopeRadius = 1f;
        OnType = "stay";
        DamageType = "energy";
        Duration = 0.19f;
        AttackCd = 0.2f;
    }
}
