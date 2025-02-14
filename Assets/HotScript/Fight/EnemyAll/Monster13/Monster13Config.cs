

public class Monster13Config : EnemyConfigBase
{

    public Monster13Config() : base()
    {
        Life = 200;
        Speed = 0.25f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 25;
        AttackCount = 2;
        DerateFire = -0.5f;
        DerateElec = 0.5f;
    }
}