using System;
using System.Collections.Generic;


public static class ItemFactory
{
    private static readonly Dictionary<string, (int id, int level, string description)> ItemConfigs = new()
    {
        { "keyBlue", (501, 3, "蓝钥匙，可以进行蓝色祈愿") },
        { "keyPurple", (502, 4, "紫钥匙，可以进行紫色祈愿") },
        { "money", (503, 3, "金币，养成消耗") },
        { "washWater", (504, 4, "洗练水，重置橙色宝石以上属性") },
        {"diamond", (505, 5,"钻石，最高级货币" )},
        {"exp",(507,2, "经验值，升级用") }
    };

    public static ItemBase Create(string resName, int count = 1)
    {
        if (ItemConfigs.TryGetValue(resName, out var config))
        {
            return new ItemBase
            {
                resName = resName,
                count = count,
                id = config.id,
                level = config.level,
                description = config.description
            };
        }
        if (resName.Contains("armChip"))
        {
            return new ItemBase
            {
                resName = ArmUtil.ArmTypeToChipResName(ArmUtil.IdToArmType(int.Parse(resName.Replace("armChip", "")))),
                count = count,
                id = 506,
                level = 3,
                description = "武器碎片,升级用"
            };
        }
        if(resName.Contains("equipmentChip")) {
            return new ItemBase {
                resName = resName,
                count = count,
                id = 508,
                level = 3,
                description = "装备碎片,升级用"
            };
        }

        throw new NotImplementedException($"未实现的资源名称: {resName}");
    }
    public static JewelBase Create(int id, int level, int placeId)
    {
        return new JewelBase(id, level, placeId, IdLevelToJewelDesc(id, level));
    }
    private static string IdLevelToJewelDesc(int id, int level)
    {
        return id switch
        {
            1 => $"攻击力加{level * 10}",
            2 => $"暴击率加{level * 1}%",
            3 => $"暴击伤害加{level * 10}%",
            4 => $"全输出加{level * 3}%",
            5 => $"火系伤害加{level * 4}%",
            6 => $"能量系伤害加{level * 4}%",
            7 => $"冰系伤害加{level * 4}%",
            8 => $"风系伤害加{level * 4}%",
            9 => $"物理伤害加{level * 4}%",
            10 => $"电系伤害加{level * 4}%",
            _ => throw new System.NotImplementedException(),
        };
    }
    public static Action CreateJewelAction(int id, int level)
    {
        GlobalConfig globalConfig = ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
        return id switch
        {
            1 => () => globalConfig.AttackValue += level * 10,
            2 => () => globalConfig.CritRate += level * 0.01f,
            3 => () => globalConfig.CritDamage += level * 0.1f,
            4 => () => globalConfig.AllAddition += level * 0.03f,
            5 => () => globalConfig.FireAddition += level * 0.04f,
            6 => () => globalConfig.EnergyAddition += level * 0.04f,
            7 => () => globalConfig.IceAddition += level * 0.04f,
            8 => () => globalConfig.WindAddition += level * 0.04f,
            9 => () => globalConfig.AdAddition += level * 0.04f,
            10 => () => globalConfig.ElecAddition += level * 0.04f,
            _ => throw new NotImplementedException(),
        };
    }
    public static ItemBase CreateArmChip((int idx, int count) res)
    {
        string resName = "armChip" + res.idx;
        return Create(resName, res.count);
    }
    public static ItemBase CreateEquipmentChip((int idx, int count) res)
    {
        string resName = "equipmentChip" + (res.idx + 1);
        return Create(resName, res.count);
    }
}
