

public class Boss2Config : EnemyConfigBase
{

    public Boss2Config() : base()
    {
        Life = 1500;
        Speed = 0.15f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 100;
    }
}