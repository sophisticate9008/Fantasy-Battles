

public class Monster7Config : EnemyConfigBase
{

    public Monster7Config() : base()
    {
        Life = 200;
        Speed = 0.2f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 30;
        DerateWind = -0.5f;
        DerateEnergy = -0.5f;
    }
}