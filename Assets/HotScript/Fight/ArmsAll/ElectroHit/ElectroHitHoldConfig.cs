public class ElectroHitHoldConfig : ArmConfigBase
{
    public ElectroHitConfig parentConfig => ArmUtil.electroHitConfig;
    public ElectroHitBoomConfig electroHitBoomConfig => ConfigManager.Instance.GetConfigByClassName("ElectroHitBoom") as ElectroHitBoomConfig;
    public override float Tlc => parentConfig.Tlc * 0.1f;
    public override float SelfScale => electroHitBoomConfig.SelfScale;
    public override float addition => parentConfig.addition;
    public override void Init()
    {
        base.Init();
        Speed = 0;
        DamageType = "elec";
        OnType = "stay";
        Duration = 2;
        ComponentStrs.Add("减速");
        SlowDegree = 0.5f;
        SlowTime = 0.5f;
        AttackCd = 0.5f;
    }
}