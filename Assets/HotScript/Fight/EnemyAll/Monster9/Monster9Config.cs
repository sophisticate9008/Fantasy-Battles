

public class Monster9Config : EnemyConfigBase
{

    public Monster9Config() : base()
    {
        Life = 5000;
        Speed = 0.12f;
        Blocks = 999;
        RangeFire = 0;
        CharacterType = "normal";
        BuffImmunityList.Add("麻痹");
        BuffImmunityList.Add("冰冻");
        Mass = 5;
        Damage = 50;
        DerateFire = -1;
    }
}