

public class Monster10Config : EnemyConfigBase
{

    public Monster10Config() : base()
    {
        Life = 3500;
        Speed = 0.2f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        BuffImmunityList.Add("减速");
        Damage = 100;
        DerateFire = -0.5f;
        

    }
}