using UnityEngine;

public class ElementImmunityNode : DamageNodeBase
{
    public override bool Process(DamageContext context)
    {
        if (context.DamageType == "")
        {
            context.DamageType = context.AttackerConfig.DamageType;
        }
        // 确定最终伤害类型


        // 元素免疫检查
        if (context.DefenderConfig.DamageTypeImmunityList.Contains(context.DamageType))
        {
            return false;
        }

        // 消耗免疫次数
        if (context.DefenderConfig.ImmunityCount > 0)
        {
            context.DefenderConfig.ImmunityCount--;
            return false;
        }

        return true;
    }
}