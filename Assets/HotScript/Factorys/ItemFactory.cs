using System;
using System.Collections.Generic;


public static class ItemFactory
{
    private static readonly Dictionary<string, (int id, int level, string description)> ItemConfigs = ItemUtil.ItemConfigs;

    public static ItemBase Create(string resName, int count = 1)
    {
        if (ItemConfigs.TryGetValue(resName, out var config))
        {
            return new ItemBase
            {
                count = count,
                id = config.id,
                level = config.level,
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
        return new JewelBase(id, level, placeId);
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
