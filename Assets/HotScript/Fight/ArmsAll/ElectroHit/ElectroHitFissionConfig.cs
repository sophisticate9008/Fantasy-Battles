public class ElectroHitFissionConfig : ArmConfigBase, IMultipleable, IPenetrable
{
    public ElectroHitConfig ParentConfig => ArmUtil.electroHitConfig;
    public int MultipleLevel { get; set; } = 6;
    public float AngleDifference { get ; set ; } = 60f;
    public int PenetrationLevel { get ; set; } = 2;
    public override float Tlc => ParentConfig.Tlc * 0.3f;
    public override float addition => ParentConfig.addition;
    public override void Init()
    {
        base.Init();
        DamageType = "elec";
        OnType = "enter";
        Speed = 3f;
    }

}