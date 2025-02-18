using System;

public class DamageAppend : DamageNodeBase
{
    public override bool Process(DamageContext context)
    {
        if (context.IsCritical)
        {
            // 百爆计算
            var args = GlobalConfig.CritWithPercentageAndMax;
            context.FinalDamage += Math.Max(
                args[0] * context.DefenderConfig.PerLife,
                GlobalConfig.AttackValue * args[1]
            );
        }
        var damageArgs = GlobalConfig.DamageWithPercentageAndMax;
        context.FinalDamage += Math.Max(
            context.DefenderComponent.MaxLife * damageArgs[0] / context.DefenderConfig.BloodBarCount,
            GlobalConfig.AttackValue * damageArgs[1]
        );
        context.FinalDamage += context.Percentage * context.DefenderComponent.MaxLife;
        return true;
    }
}