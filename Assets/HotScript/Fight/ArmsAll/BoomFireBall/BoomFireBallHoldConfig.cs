
public class BoomFireBallHoldConfig : ArmConfigBase
{
    public override void Init()
    {
        base.Init();
        OnType = "stay";
        DamageType = "fire";
        RangeFire = 10;
        Duration = 3;
        Tlc = 1;
        DamagePos = "land";
    }
}
