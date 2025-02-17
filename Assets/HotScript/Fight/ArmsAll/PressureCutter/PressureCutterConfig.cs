



public class PressureCutterConfig : ArmConfigBase, IPenetrable, IMultipleable
{
    public int PenetrationLevel { get; set; } = 1;
    public int MultipleLevel { get; set; } = 1;
    public float AngleDifference { get; set; } = 5;

    public override void Init()
    {
        base.Init();
        Speed = 8;
        OnType = "enter";
        ScopeRadius = 10f;
        ComponentStrs.Add("穿透");
        MaxForce = 100;
    }
}
