

public class Monster1Config: EnemyConfigBase{
    
    public Monster1Config():base() {
        Life = 3000;
        Speed = 0.3f;
        RangeFire = 0;
        CharacterType = "normal";
        Damage = 20;
        Mass = 5;
        Blocks = 2;
        SelfScale = 1f;
    }
}