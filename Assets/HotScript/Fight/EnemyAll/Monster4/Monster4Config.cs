

using MyEnums;

public class Monster4Config : EnemyConfigBase
{

    public Monster4Config() : base()
    {
        Life = 1500;
        Speed = 0.4f;
        Blocks = 1;
        RangeFire = 1.2f;
        CharacterType = "normal";
        Damage = 30;
        SelfScale = 1.5f;
        attackRange = AttackRange.tel;
        DerateEnergy = -0.5f;
        DerateWind = -0.5f;
    }
}