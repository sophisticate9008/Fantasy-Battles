
public class DragonLaunchConfig : ArmConfigBase
{
    public override void Init()
    {
        base.Init();
        Duration = 0.5f;
        OnType = "enter";
        ComponentStrs.Add("眩晕");
        MaxForce = 100;
    }
}
