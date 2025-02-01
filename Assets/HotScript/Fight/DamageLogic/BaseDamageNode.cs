public class BaseDamageNode : DamageNodeBase
{
    public override bool Process(DamageContext context)
    {
        var config = context.AttackerConfig;
        if(context.Tlc != default) {
            context.FinalDamage = GlobalConfig.AttackValue * context.Tlc;
            return true;
        }
        if (context.IsBuffDamage)
        {
            context.FinalDamage = GlobalConfig.AttackValue * config.BuffDamageTlc;
        }
        else
        {
            context.FinalDamage = GlobalConfig.AttackValue * config.Tlc;
        }
        
        return true;
    }
}