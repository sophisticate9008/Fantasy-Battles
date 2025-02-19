
public class IceBallConfig : ArmConfigBase, IPenetrable, IMultipleable,IReboundable
{
    public bool IsUpgrade { get; set; } = false;
    public int PenetrationLevel { get; set; } = 1;
    public int MultipleLevel { get; set; } = 1;
    public float AngleDifference { get; set; } = 5;
    public float FreezenProb { get; set; } = 0f;
    public int ReboundCount { get; set; } = 0;

    public override void Init()
    {
        base.Init();
        ComponentStrs.Add("穿透");
        Speed = 8;
        OnType = "enter";
        ScopeRadius = 10f;
        MaxForce = 100;
    }
}
