

public class Monster3Config : EnemyConfigBase
{

    public Monster3Config() : base()
    {
        Life = 1500;
        Speed = 0.2f;
        Blocks = 1;
        RangeFire = 2;
        CharacterType = "normal";
        Damage = 20;
        SelfScale = 0.8f;
    }
}