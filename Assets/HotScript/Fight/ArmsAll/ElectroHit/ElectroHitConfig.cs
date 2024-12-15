

public class ElectroHiteConfig : ArmConfigBase, IBoomable
{
    public ArmConfigBase BoomChildConfig =>
        ConfigManager.Instance.GetConfigByClassName("ElectroHiteBoom") as ElectroHiteBoomConfig;
    public override void Init()
    {
        base.Init();
        Name = "电磁穿透";
        Description = "电磁穿透";
        ComponentStrs.Add("爆炸");
        ComponentStrs.Add("麻痹");
        ScopeRadius = 2;
        OnType = "enter";
        DamageType = "elec";
        Tlc = 2;
        CritRate = 0.2f;
        AttackCd = 0.5f;
        Cd = 5f;
        AttackCount = 5;
        Duration = 0.3f;
        RangeFire = 9;
    }
}

