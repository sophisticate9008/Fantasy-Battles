

using MyEnums;

public class Monster5Config : EnemyConfigBase
{

    public Monster5Config() : base()
    {
        Life = 1500;
        Speed = 0.2f;
        Blocks = 1;
        RangeFire = 1.2f;
        CharacterType = "normal";
        Damage = 100;
        SelfScale = 1.5f;
        attackRange = AttackRange.tel;
        ActionType = "sky"; 
        DerateAd = -0.5f;
    }
}