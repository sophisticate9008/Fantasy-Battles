

public class Boss3Config : EnemyConfigBase
{

    public Boss3Config() : base()
    {
        Life = 15000;
        Speed = 0.15f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 200;
        DerateAd = 1;
        BuffImmunityList.Add("眩晕");
        DerateElec = -0.5f;
        DerateWind = -0.5f;
        DerateFire = -0.5f;
        DerateIce = -0.5f;

    }
}