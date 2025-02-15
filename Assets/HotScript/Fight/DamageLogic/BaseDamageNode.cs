public class BaseDamageNode : DamageNodeBase
{
    public override bool Process(DamageContext context)
    {
        var config = context.AttackerConfig;
        if (context.Tlc != default)
        {
            context.FinalDamage = GlobalConfig.AttackValue * context.Tlc;
            return true;
        }
        context.FinalDamage = GlobalConfig.AttackValue * config.Tlc;
        return true;
    }
}