
public class BoomFireBallBoomConfig : ArmConfigBase,IReRelease
{
    public int PerNum { get; set ; } = 1;
    public float ReleaseProb { get ; set; } = 0;

    public ArmConfigBase ReleaseObjConfig => ArmUtil.boomFireBallConfig;

    public override void Init()
    {
        base.Init();
        Duration = 0.5f;
        OnType = "enter";
        Tlc = 0.5f;
        DamageType = "fire";
        MaxForce = 150;
    }
}

