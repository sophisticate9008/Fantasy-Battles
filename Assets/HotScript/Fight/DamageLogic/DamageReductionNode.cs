public class DamageReductionNode : DamageNodeBase
{
    public override bool Process(DamageContext context)
    {
        var defenderConfig = context.DefenderConfig;
        var reductions = defenderConfig.GetDamageReduction();

        if (reductions.TryGetValue(context.DamageType, out var reduction))
        {
            context.FinalDamage *= 1 + reduction;
        }

        return true;
    }
}