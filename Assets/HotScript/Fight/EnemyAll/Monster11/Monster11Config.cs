

public class Monster11Config : EnemyConfigBase
{

    public Monster11Config() : base()
    {
        Life = 4000;
        Speed = 0.2f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 25;
        Mass = 3;
        BuffImmunityList.Add("眩晕");
        DeratePenetrate = 0.5f;
    }
}