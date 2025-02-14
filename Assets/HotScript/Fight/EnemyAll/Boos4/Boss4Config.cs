

public class Boss4Config : EnemyConfigBase
{

    public Boss4Config() : base()
    {
        Life = 15000;
        Speed = 0.15f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 200;
        DerateFire = 1;
        DerateIce = 1;
        BuffImmunityList.Add("冰冻");
        BuffImmunityList.Add("点燃");
        
    }
}