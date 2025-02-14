

public class Monster2Config : EnemyConfigBase
{

    public Monster2Config() : base()
    {
        Life = 3000;
        Speed = 0.15f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 20;
        SelfScale = 0.9f;
    }
}