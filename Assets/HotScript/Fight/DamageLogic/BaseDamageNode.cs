public class BaseDamageNode : DamageNodeBase
{
    public override bool Process(DamageContext context)
    {
        if (context.Tlc != default)
        {
            context.FinalDamage = GlobalConfig.AttackValue * context.Tlc;
            return true;
        }
        var config = context.AttackerConfig;
        context.FinalDamage = GlobalConfig.AttackValue * config.Tlc;
        return true;
    }
}