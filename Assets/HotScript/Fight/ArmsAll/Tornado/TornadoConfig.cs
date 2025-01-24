using MyEnums;
public class TornadoConfig : ArmConfigBase
{
    public override void Init()
    {
        base.Init();
        AttackCount = 1;
        AttackCd = 0.2f;
        OnType = "stay";
        ScopeRadius = 15f;
        Speed = 2f;
        CdType = CdTypes.WaitLast;
        ControlBy = ControlBy.Arm;
        MaxForce = -10;
    }
}
