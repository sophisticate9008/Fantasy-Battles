




public class MagicBulletConfig : ArmConfigBase, IMultipleable, IFissionable, IPenetrable, IReboundable, IBoomable
{
    // 新增属性
    public int PenetrationLevel { get; set; } = 1;
    public int ReboundCount { get; set; } = 1;
    public MagicBulletFissionConfig MagicBulletFissionConfig => ConfigManager.Instance.GetConfigByClassName("MagicBulletFission") as MagicBulletFissionConfig;
    public int MagicBulletFissionCount { get; set; } = 2;
    public int MultipleLevel { get; set; } = 1;
    public int RepeatLevel { get; set; } = 1;
    public float AngleDifference { get; set; } = 5f;
    public float RepeatCd { get; set; } = 0.1f;
    public ArmConfigBase FissionableChildConfig => MagicBulletFissionConfig;


    public string FindType { get; set; } = "random";

    public ArmConfigBase BoomChildConfig => ConfigManager.Instance.GetConfigByClassName("MagicBulletBoom") as MagicBulletBoomConfig;

    // 构造函数
    public MagicBulletConfig() : base()
    {
        // 延迟初始化 MagicBulletFissionConfig，并传递当前 MagicBulletConfig 实例

    }

    // 重写父类的 Init 方法
    public override void Init()
    {
        base.Init();
        // 初始化 MagicBulletConfig 的属性
        Speed = 10f;
        ComponentStrs.Add("穿透");
        // ComponentStrs.Add("点燃");
        // ComponentStrs.Add("冰冻");
        // ComponentStrs.Add("麻痹");
        // ComponentStrs.Add("眩晕");
        AttackCd = 1f;
        AttackCount = 30;
        OnType = "enter";
        CdType = MyEnums.CdTypes.Exhaust;
    }
}
