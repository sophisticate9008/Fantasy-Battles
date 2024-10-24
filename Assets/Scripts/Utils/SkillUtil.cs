using System;
using System.Collections.Generic;
using System.Linq;

public class SkillUtil
{
    public static List<List<int>> conflictLists = new() {
        CommonUtil.AsList(15,17),
    };
    public static Dictionary<string, List<int>> iconList = new()
    {
        {"icon_keji_wenyadan", CommonUtil.AsList(0)},
        {"icon_keji_ganbingzimudan", CommonUtil.AsList(1)},
        {"icon_keji_diancichuanci", CommonUtil.AsList(2)},
        {"icon_keji_gangtiehongliu", CommonUtil.AsList(3)},
        {"icon_keji_gaonengshexian", CommonUtil.AsList(4)},
        {"icon_keji_zhidaojiguang", CommonUtil.AsList(5)},
        {"icon_keji_bingbao", CommonUtil.AsList(6)},
        {"icon_keji_yuedaodianzi", CommonUtil.AsList(7)},
        {"icon_keji_xuanfengjianong", CommonUtil.AsList(8)},
        {"icon_keji_kongtouhongzha", CommonUtil.AsList(9)},
        {"icon_keji_yasuoqiren", CommonUtil.AsList(10)},
        {"icon_keji_ranyoudan", CommonUtil.AsList(11)},
        {"icon_keji_wurenji",CommonUtil.AsList(12)},
        {"icon_keji_qiang", CommonUtil.AsList(13,14,15,16,17)}

    };
    public static Dictionary<int, string> ownerTypeDict = new() {
        {1, "Tnt"},
        {2, "IceBomb"},
        {3, "ElecPenetrate"},
        {4, "EnergyRay"},
        {5, "Laser"},
        {6, "IceGenerator"},
        {7, "JumpElectro"},
        {8, "Tornado"},
        {9, "AirDropBomb"},
        {10, "PressureCutter"},
        {11, "FuelBullet"},
        {12, "UAV"},
    };

    
    public static Action IdToUseAction(int id)
    {
        return null;
    }
    public static List<int> IdToConflictIds(int id)
    {
        foreach (var list in conflictLists)
        {
            if (list.Contains(id))
            {
                return list; // 返回包含该 ID 的冲突组
            }
        }

        return new List<int>(); // 如果没有找到，返回空列表

    }

    public static string IdToDesc(int id)
    {
        return id switch
        {
            0 => "学习温压弹",
            1 => "学习干冰弹",
            2 => "学习电磁穿透",
            3 => "学习装甲车",
            4 => "学习高能射线",
            5 => "学习制导激光",
            6 => "学习冰暴发生器",
            7 => "学习跃迁电子",
            8 => "学习旋风加农",
            9 => "学习空投轰炸",
            10 => "学习压缩气刃",
            11 => "学习燃油弹",
            12 => "学习无人机",
            13 => "子弹弹道数量+1",
            14 => "子弹连发数+1",
            15 => "子弹分裂+2",
            16 => "子弹穿透+1,伤害+30%",
            17 => "子弹变为火系子弹",
            _ => throw new NotImplementedException(),
        };
    }
    public static string IdToName(int id)
    {
        return id switch
        {
            0 => "温压弹",
            1 => "干冰弹",
            2 => "电磁穿透",
            3 => "装甲车",
            4 => "高能射线",
            5 => "制导激光",
            6 => "冰暴发生器",
            7 => "跃迁电子",
            8 => "旋风加农",
            9 => "空投轰炸",
            10 => "压缩气刃",
            11 => "燃油弹",
            12 => "无人机",
            13 => "子弹齐射",
            14 => "子弹连发",
            15 => "次级子弹",
            16 => "子弹穿透",
            17 => "火系子弹",

            _ => throw new NotImplementedException(),
        };
    }
    public static int IdToMaxSelCount(int id)
    {
        return id switch
        {
            13 => 3,
            14 => 2,
            15 => 2,
            16 => 2,

            _ => 1,
        };

    }
    //技能前置列表
    public static List<int> IdToPreList(int id)
    {
        return id switch
        {
            17 => CommonUtil.AsList(0),
            _ => new List<int>(),
        };
    }

    public static List<SkillNode> AllSkill()
    {
        List<SkillNode> all = new();
        for (int i = 0; i <= 17; i++)
        {
            all.Add(CreateSkillNode(i));
        }
        return all;
    }
    public static string IdToResName(int id)
    {
        foreach (var kv in iconList)
        {
            if (kv.Value.Contains(id))
            {
                return kv.Key;
            }
        }
        return null;
    }
    public static bool IdToIsUnlocked(int id)
    {
        if (id <= 16)
        {
            return true;
        }
        return id switch
        {
            17 => true,
            _ => false,
        };
    }
    public static bool IdToIsSatisfied(int id)
    {
        if (id <= 16)
        {
            return true;
        }
        return id switch
        {
            _ => false,
        };
    }
    public static string IdToOwnerType(int id)
    {
        if (ownerTypeDict.TryGetValue(id, out var ownerType))
        {
            return ownerType;
        }
        else
        {
            return "bullet";
        }
    }
    public static SkillNode CreateSkillNode(int id)
    {
        return new SkillNode(id, IdToPreList(id), IdToConflictIds(id), IdToName(id), IdToDesc(id),
                    IdToMaxSelCount(id), IdToResName(id), IdToIsUnlocked(id), IdToIsSatisfied(id), IdToOwnerType(id));
    }
}