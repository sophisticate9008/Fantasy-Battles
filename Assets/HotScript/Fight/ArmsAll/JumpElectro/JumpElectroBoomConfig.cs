public class JumpElectroBoomConfig: ArmConfigBase {
    public JumpElectroConfig parentConfig => ArmUtil.jumpElectroConfig;
    public override float PalsyTime => parentConfig.PalsyTime;
    public override float Tlc => parentConfig.Tlc * 0.25f;
    public override float addition => parentConfig.addition;
    public override void Init()
    {
        base.Init();
        Speed = 0;
        DamageType = "elec";
        OnType = "enter";
        ComponentStrs.Add("麻痹");
    }
}