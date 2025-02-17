
using MyEnums;


public class WhirlingBladeConfig : ArmConfigBase, IReboundable
{
    public int ReboundCount { get; set; } = 999;

    public override void Init()
    {
        base.Init();
        OnType = "stay";
        AttackCd = 0.2f;
        CdType = CdTypes.WaitLast;
        ControlBy = ControlBy.Arm;
        ComponentStrs.Add("反弹");
        ComponentStrs.Add("阻尼");
        Speed = 3;
        ScopeRadius = 6;
    }
}
