

public class Monster6Config : EnemyConfigBase
{

    public Monster6Config() : base()
    {
        Life = 200;
        Speed = 0.3f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 20;
        DerateFire = 0.5f;
        DerateElec = -0.5f;
        BuffImmunityList.Add("点燃");
        BuffImmunityList.Add("冰冻");
    }
}