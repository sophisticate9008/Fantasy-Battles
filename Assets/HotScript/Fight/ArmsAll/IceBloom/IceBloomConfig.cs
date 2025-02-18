
public class IceBloomConfig : ArmConfigBase
{
    public int IceChipNum = 12;
    public override void Init()
    {
        base.Init();
        OnType = "stay";
        ComponentStrs.Add("冰冻");
        AttackCd = 0.2f;
    }
}
