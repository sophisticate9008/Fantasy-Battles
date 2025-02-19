public class JumpElectroBoomConfig: ArmConfigBase {
    public JumpElectroConfig parentConfig => ArmUtil.jumpElectroConfig;
    public override float PalsyTime => parentConfig.PalsyTime;
    public override void Init()
    {
        base.Init();
        ComponentStrs.Add("麻痹");
    }
}