public class DamageReductionNode : DamageNodeBase
{
    public override bool Process(DamageContext context)
    {
        var defenderConfig = context.DefenderConfig;
        var reductions = defenderConfig.GetDamageReduction();
        float final_reduction = 0;
        if (reductions.TryGetValue(context.DamageType, out var reduction))
        {
            final_reduction += reduction;

        }
        if(context.Attacker != null) {
            if(CommonUtil.IsImplementsInterface<IPenetrable>(context.AttackerConfig.GetType())) {
                final_reduction += reductions["penetrate"];
            }
        }
        context.FinalDamage *= 1 - final_reduction;
        return true;
    }
}