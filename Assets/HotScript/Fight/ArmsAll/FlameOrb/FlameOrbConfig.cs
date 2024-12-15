
public class FlameOrbConfig : ArmConfigBase, IHoldable
{
    public ArmConfigBase HoldChildConfig => ConfigManager.Instance.GetConfigByClassName("FlameOrbHold") as FlameOrbHoldConfig;

    public override void Init()
    {
        base.Init();
        Duration = 2.1f;
        AttackCount = 1;
        AttackCd = 0.5f;
        Cd = 3;
        OnType = "enter";
        RangeFire = 10;
        DamagePos = "land";
        ComponentStrs.Add("Hold");
    }
}
