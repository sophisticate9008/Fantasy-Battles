public class IceBloomFissionConfig : ArmConfigBase, IMultipleable, IPenetrable
{
    public IceBloomConfig parentConfig => ArmUtil.iceBloomConfig;

    public int MultipleLevel { get { return parentConfig.IceChipNum; } set { } }
    public float AngleDifference { get; set; } = 60f;
    public int PenetrationLevel { get; set; } = 2;
    public override float Tlc => parentConfig.Tlc * 0.1f;

    public override void Init()
    {
        Speed = 15f;
        base.Init();
        ComponentStrs.Add("穿透");
        ComponentStrs.Add("冰冻");
        DamageType = "ice";
        OnType = "enter";
    }
}