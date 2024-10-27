using System;
using FightBases;
using TheBuffs;
using UnityEngine;

namespace Factorys
{
    public class BuffFactory
    {
        public static BuffBase Create(string buffName, float duration, GameObject selfObj, GameObject enemyObj, params object[] args)
        {

            return buffName switch
            {
                // 不需要自定义参数的 Buffs
                "眩晕" => new Dizzy(buffName, duration, selfObj, enemyObj),
                "冰冻" => new Freeze(buffName, duration, selfObj, enemyObj),
                "麻痹" => new Palsy(buffName, duration, selfObj, enemyObj),
                // 需要自定义参数的 Buffs
                "减速" => new Slow(buffName, duration, selfObj, enemyObj, (float) args[0]),

                // 处理未知的 Buff 类型
                _ => throw new ArgumentException($"Unknown debuff: {buffName}"),
            };
        }
    }
}