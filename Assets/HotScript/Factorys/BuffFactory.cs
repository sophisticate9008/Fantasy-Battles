using System;

using UnityEngine;


public class BuffFactory
{
    public static BuffBase Create(string buffName, float duration, GameObject selfObj, GameObject enemyObj, params object[] args)
    {
        if (buffName.Contains("减速"))
        {
            return new Slow(buffName, duration, selfObj, enemyObj, (float)args[0]);
        }
        if (buffName.Contains("点燃"))
        {
            return new Fire(buffName, duration, selfObj, enemyObj);
        }
        return buffName switch
        {
            // 不需要自定义参数的 Buffs
            "眩晕" => new Dizzy(buffName, duration, selfObj, enemyObj),
            "冰冻" => new Freeze(buffName, duration, selfObj, enemyObj),
            "麻痹" => new Palsy(buffName, duration, selfObj, enemyObj),
            // 处理未知的 Buff 类型
            _ => throw new ArgumentException($"Unknown debuff: {buffName}"),
        };
    }
}
