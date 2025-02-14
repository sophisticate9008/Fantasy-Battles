

public class Boss6Config : EnemyConfigBase
{

    public Boss6Config() : base()
    {
        Life = 1500;
        Speed = 0.15f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 200;
        DerateAd = -0.5f;
        DerateElec = 1;
        DerateWind = 1;
        DerateFire = 1;
        DerateIce = 1;
        BuffImmunityList.Add("点燃");
        BuffImmunityList.Add("麻痹");
        BuffImmunityList.Add("冰冻");
    }
}