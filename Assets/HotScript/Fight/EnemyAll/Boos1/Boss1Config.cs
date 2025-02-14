

public class Boss1Config : EnemyConfigBase
{

    public Boss1Config() : base()
    {
        Life = 15000;
        Speed = 0.15f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "boss";
        Damage = 200;
        Mass = 3;
        DerateFire = -0.5f;
    }
}