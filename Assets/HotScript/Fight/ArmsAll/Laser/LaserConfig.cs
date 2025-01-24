

using MyEnums;
using UnityEngine;


public class LaserConfig : ArmConfigBase
{
    [SerializeField] private bool isFlame;
    public virtual bool IsFlame { get => isFlame; set => isFlame = value; }
    public string FindType { get; set; } = "scope";
    public virtual bool IsMainDamageUp { get; set; } = false;
    public override void Init()
    {
        base.Init();
        AttackCd = 0.2f;
        OnType = "stay";
        CritRate = 0.5f;
        ScopeRadius = 12f;
        IsLineCast = true;
        CdType = CdTypes.WaitLast;
        ControlBy = ControlBy.Arm;
        MaxForce = 10;
    }
}
