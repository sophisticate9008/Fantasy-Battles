public class DamageAdditionNode : DamageNodeBase
{
    public override bool Process(DamageContext context)
    {
        float addition = GlobalConfig.AllAddition;

        // 类型加成
        var damageAddition = GlobalConfig.GetDamageAddition();
        if (damageAddition.TryGetValue(context.DamageType, out var typeAdd))
        {
            addition += typeAdd;
        }

        // 随机浮动
        addition += UnityEngine.Random.Range(
            GlobalConfig.RandomAdditonMin,
            GlobalConfig.RandomAdditonMax
        );

        // 精英/BOSS加成
        addition += GlobalConfig.AdditionToEliteOrBoss;
        context.FinalDamage *= 1 + addition;
        return true;
    }
}