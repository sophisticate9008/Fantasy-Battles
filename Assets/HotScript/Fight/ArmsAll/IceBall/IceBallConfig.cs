
public class IceBallConfig : ArmConfigBase, IPenetrable, IMultipleable
{
    public int PenetrationLevel { get; set; } = 1;
    public int MultipleLevel { get; set; } = 1;
    public float AngleDifference { get; set; } = 5;

    public override void Init()
    {
        ComponentStrs.Add("穿透");
        RangeFire = 8;
        Speed = 8;
        OnType = "enter";
        DamageType = "ice";
        Cd = 3;
        ScopeRadius = 10f;
        Tlc = 1;
        MaxForce = 100;
    }
}
