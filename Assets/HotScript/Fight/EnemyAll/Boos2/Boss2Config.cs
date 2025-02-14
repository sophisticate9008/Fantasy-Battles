

public class Boss2Config : EnemyConfigBase
{

    public Boss2Config() : base()
    {
        Life = 15000;
        Speed = 0.15f;
        Blocks = 1;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 200;
        DerateElec = -0.5f;
        DerateEnergy = 0.5f;
        
    }
}