
public class DragonLaunchConfig : ArmConfigBase
{
    public override void Init()
    {
        base.Init();
        Duration = 0.5f;
        OnType = "enter";
        MaxForce = 100;
    }
}
