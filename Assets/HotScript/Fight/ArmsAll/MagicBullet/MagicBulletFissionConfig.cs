


public class MagicBulletFissionConfig : ArmConfigBase, IMultipleable, IPenetrable
{
    public MagicBulletConfig MagicBulletConfig => ConfigManager.Instance.GetConfigByClassName("MagicBullet") as MagicBulletConfig;

    // 使用 override 重写属性，保持多态性
    public override float Tlc
    {
        get => MagicBulletConfig.Tlc * 0.25f;  // 使用 MagicBulletConfig 的 tlc 属性
    }
    public override float Speed => MagicBulletConfig.Speed;
    public int MultipleLevel { get; set; } = 0;
    public float AngleDifference { get; set; } = 15f;
    public override float CritRate => MagicBulletConfig.CritRate;
    public override string Owner => MagicBulletConfig.Name;

    public int PenetrationLevel { get => MagicBulletConfig.PenetrationLevel; set { } }

    // 构造函数
    public MagicBulletFissionConfig() : base()
    {
        DamageType = "ad";
        OnType = "enter";
        ComponentStrs.Add("穿透");
    }
}
