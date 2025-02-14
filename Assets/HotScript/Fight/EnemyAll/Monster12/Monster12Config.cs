

public class Monster12Config : EnemyConfigBase
{

    public Monster12Config() : base()
    {
        Life = 1500;
        Speed = 0.25f;
        AttackCount = 2;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 15;
        DerateElec = -0.5f;
        DerateWind = -0.5f;
        DerateFire = -0.5f;
        DerateIce = -0.5f;
    }
}