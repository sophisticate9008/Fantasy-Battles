
public class FlameOrbConfig : ArmConfigBase
{
    public override void Init()
    {
        base.Init();
        AttackCount = 1;
        AttackCd = 0.5f;
        OnType = "stay";
        DamageType = "fire";
        RangeFire = 10;
        Duration = 3;
        Tlc = 2;
        DamagePos = "land";
    }
}
