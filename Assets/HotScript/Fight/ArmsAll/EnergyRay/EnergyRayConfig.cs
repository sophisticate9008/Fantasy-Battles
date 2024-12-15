using MyEnums;


public class EnergyRayConfig : ArmConfigBase
{
    public override void Init()
    {
        base.Init();
        AttackCount = 1;
        Cd = 5;
        Tlc = 1;
        DamageType = "energy";
        OnType = "stay";
        AttackCd = 0.2f;
        AttackCount = 1;
        RangeFire = 10;
        CdType = CdTypes.WaitLast;
        ControlBy = ControlBy.Arm;
        Duration = 3;
    }
}

