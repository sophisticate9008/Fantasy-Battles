using System;

public class CriticalHitNode : DamageNodeBase
{
    public override bool Process(DamageContext context)
    {
        var config = context.AttackerConfig;
        float critRate = GlobalConfig.CritRate + config.CritRate;
        
        if (UnityEngine.Random.value < critRate)
        {
            context.IsCritical = true;
            context.FinalDamage *= 2 + GlobalConfig.CritDamage;
        }

        // 普通百分比
        var damageArgs = GlobalConfig.DamageWithPercentageAndMax;
        context.FinalDamage += Math.Max(
            context.DefenderComponent.MaxLife * damageArgs[0],
            GlobalConfig.AttackValue * damageArgs[1]
        );
        return true;
    }
}