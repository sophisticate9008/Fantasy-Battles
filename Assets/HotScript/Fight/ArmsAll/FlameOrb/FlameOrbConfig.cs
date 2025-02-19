
public class FlameOrbConfig : ArmConfigBase
{
    public bool IsUpgrade{get; set ;} = false;
    public override void Init()
    {
        base.Init();
        OnType = "stay";
        Speed = 0;
        fireTlc = 0.5f;
        ComponentStrs.Add("点燃");
    }
}
