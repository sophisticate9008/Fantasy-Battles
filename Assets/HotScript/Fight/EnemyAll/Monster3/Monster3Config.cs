

using MyEnums;

public class Monster3Config : EnemyConfigBase
{

    public Monster3Config() : base()
    {
        Life = 1500;
        Speed = 0.2f;
        Blocks = 1;
        RangeFire = 1.5f;
        CharacterType = "normal";
        Damage = 20;
        SelfScale = 0.8f;
        attackRange = AttackRange.tel;
        DerateFire = -0.5f;
        DerateElec = -0.5f;
    }
}