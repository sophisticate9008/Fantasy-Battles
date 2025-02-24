
public class GroundStabConfig : ArmConfigBase
{
    public bool isFire = false;
    public override void Init()
    {
        base.Init();
        OnType = "stay";
        DamagePos = "land";
        AttackCd = 0.1f;
        Speed = 1.5f;
        MaxForce = 50;
        ComponentStrs.Add("减速");
        SlowDegree = 0.8f;
        SlowTime = 2;
        ComponentStrs.Add("阻尼");
    }
}
