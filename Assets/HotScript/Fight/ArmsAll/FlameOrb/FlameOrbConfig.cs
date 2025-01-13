
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
        Duration = 4;
        Tlc = 2;
        DamagePos = "land";
        Cd = 5;
        Speed = 0;
    }
}
