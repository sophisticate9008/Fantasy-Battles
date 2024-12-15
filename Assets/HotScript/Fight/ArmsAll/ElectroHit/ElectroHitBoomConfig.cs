
public class ElectroHiteBoomConfig : ArmConfigBase
{
    public override void Init()
    {
        base.Init();
        Duration = 0.5f;
        OnType = "enter";
        Tlc = 0.5f;
        DamageType = "elec";
        ComponentStrs.Add("麻痹");
    }
}
