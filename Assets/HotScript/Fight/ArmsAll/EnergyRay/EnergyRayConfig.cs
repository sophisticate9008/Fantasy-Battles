using MyEnums;


public class EnergyRayConfig : ArmConfigBase
{
    public override void Init()
    {
        base.Init();
        OnType = "stay";
        AttackCd = 0.2f;
        CdType = CdTypes.WaitLast;
        ControlBy = ControlBy.Arm;
    }
}

