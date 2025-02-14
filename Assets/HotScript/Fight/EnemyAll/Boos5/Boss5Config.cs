

public class Boss5Config : EnemyConfigBase
{

    public Boss5Config() : base()
    {
        Life = 15000;
        Speed = 0.15f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 200;
        DerateWind = -0.5f;
    }
}