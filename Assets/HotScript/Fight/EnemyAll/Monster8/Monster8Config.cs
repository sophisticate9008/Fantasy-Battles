

public class Monster8Config : EnemyConfigBase
{

    public Monster8Config() : base()
    {
        Life = 1500;
        Speed = 0.3f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 40;
        BuffImmunityList.Add("冰冻");
        BuffImmunityList.Add("点燃");
        DerateIce = 0.5f;
        DerateFire = -0.5f;
        SelfScale = 0.8f;
    }
}