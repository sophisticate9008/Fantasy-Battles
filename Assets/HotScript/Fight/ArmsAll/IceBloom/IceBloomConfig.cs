
public class IceBloomConfig : ArmConfigBase
{
    public override void Init()
    {
        base.Init();
        OnType = "stay";
        ComponentStrs.Add("冰冻");
        Duration = 3f;
        AttackCd = 0.2f;
        ComponentStrs.Add("减速");
    }
}
