using System;
using System.Collections.Generic;
using UnityEngine;

public static class SkillUtil
{
    #region 技能冲突
    public static readonly List<List<int>> conflictLists = new() {
        CommonUtil.AsList(15,17,18,19),

    };
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
    #endregion
    #region  技能图标
    public static readonly Dictionary<string, List<int>> iconList = new()
    {
        {"icon_BoomFireBall", CommonUtil.AsList(0, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37)},
        {"icon_IceBall", CommonUtil.AsList(1, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58)},
        {"icon_ElectroHit", CommonUtil.AsList(2, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76)},
        {"icon_GroundStab", CommonUtil.AsList(3, 85, 86, 87, 88, 89, 90, 91)},
        {"icon_EnergyRay", CommonUtil.AsList(4, 95, 96, 97, 98, 99, 100, 101)},
        {"icon_Laser", CommonUtil.AsList(5, 111, 112, 113, 114, 115, 116, 117)},
        {"icon_IceBloom", CommonUtil.AsList(6, 127, 128, 129, 130, 131, 132)},
        {"icon_JumpElectro", CommonUtil.AsList(7, 142, 143, 144, 145, 146, 147, 148, 149, 150)},
        {"icon_Tornado", CommonUtil.AsList(8, 160, 161, 162, 163, 164, 165, 166, 167)},
        {"icon_DragonLaunch", CommonUtil.AsList(9, 177, 178, 179, 180, 181)},
        {"icon_PressureCutter", CommonUtil.AsList(10, 191, 192, 193, 194, 195, 196, 197, 198, 199)},
        {"icon_FlameOrb", CommonUtil.AsList(11, 209, 210, 211, 212, 213, 214, 215, 216)},
        {"icon_WhirlingBlade", CommonUtil.AsList(12, 226, 227, 228, 229, 230, 231)},
        {"icon_MagicBullet", CommonUtil.AsList(13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27)}
    };

    public static string ArmTypeToResName(string armType)
    {
        foreach (var armTypeItem in armTypeDict)
        {
            if (armTypeItem.Value.Equals(armType))
            {
                return IdToResName(armTypeItem.Key);
            }
        }
        return null;
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
    #endregion

    #region 技能类型
    public static readonly Dictionary<int, string> armTypeDict = new() {
        {0, "BoomFireBall"},
        {1, "IceBall"},
        {2, "ElectroHit"},
        {3, "GroundStab"},
        {4, "EnergyRay"},
        {5, "Laser"},
        {6, "IceBloom"},
        {7, "JumpElectro"},
        {8, "Tornado"},
        {9, "DragonLaunch"},
        {10, "PressureCutter"},
        {11, "FlameOrb"},
        {12, "WhirlingBlade"},
        {13,"MagicBullet"}
    };
    public static string IdToArmType(int id)
    {
        if (armTypeDict.TryGetValue(id, out var armType))
        {
            return armType;
        }
        else
        {
            return "MagicBullet";
        }
    }
    #endregion


    #region 技能描述
    public static string IdToDesc(int id)
    {
        return id switch
        {
            0 => "学习高爆火球",
            1 => "学习寒冰弹",
            2 => "学习电流直击",
            3 => "学习地刺",
            4 => "学习能量射线",
            5 => "学习锁定激光",
            6 => "学习寒冰绽放",
            7 => "学习跳跃电子",
            8 => "学习龙卷风",
            9 => "学习飞龙投射",
            10 => "学习压缩气刃",
            11 => "学习滞留火焰",
            12 => "学习旋转利刃",
            13 => "魔法弹弹道数量+1",
            14 => "魔法弹连发数+1",
            15 => "魔法弹分裂+2",
            16 => "魔法弹穿透+1,伤害+30%",
            17 => "魔法弹变为火系魔法弹",
            18 => "魔法弹变为电系魔法弹",
            19 => "魔法弹变为冰系魔法弹",
            20 => "魔法弹爆炸",
            21 => "魔法弹爆炸范围增加60%",
            22 => "魔法弹伤害+60%",
            23 => "魔法弹与次级魔法弹伤害+100%",
            24 => "次级魔法弹伤害+100%",
            25 => "魔法弹命中后散发四个小冰片",
            26 => "射速增加10%",
            27 => "魔法弹碰到墙壁反弹次数+1",


            28 => "高爆火球冲撞伤害+75%,爆炸击退+60%",
            29 => "高爆火球爆炸伤害+80%",
            30 => "高爆火球爆炸范围+80%",
            31 => "高爆火球数量+1",
            32 => "高爆火球数量+2",
            33 => "高爆火球点燃敌人6s",
            34 => "高爆火球点燃附加最大生命值3%",
            35 => "高爆火球击中释放3个火焰碎片",
            36 => "高爆火球点燃的怪物死亡后爆炸",
            37 => "高爆火球进化,穿透+2, 爆炸范围+80%, 爆炸击退+60%",
            //预留10个
            48 => "寒冰弹齐射+1",
            49 => "寒冰弹连发次数+1",
            50 => "寒冰弹伤害-20%, 穿透+2",
            51 => "寒冰弹伤害+60%,穿透+1",
            52 => "寒冰弹伤害+100%,击退+30%",
            53 => "寒冰弹叠加冻伤,上限加5层，持续5s",
            54 => "寒冰弹穿透+1,击退+60%",
            55 => "寒冰弹命中冻结敌人2s",
            56 => "寒冰弹进化,每次命中分裂1个原始冰弹,继承加成",
            57 => "数量+1",
            58 => "数量+2",
            //预留十个
            66 => "电磁穿刺次数+1",
            67 => "伤害-20%.次数+2",
            68 => "伤害+80%",
            69 => "杀死怪物后对范围内敌人追击一次伤害",
            70 => "命中后释放6个粒子",
            71 => "命中后爆炸",
            72 => "爆炸范围+80%",
            73 => "爆炸范围内生成电磁场,持续两秒,减速50%,造成攻击力100%的伤害",
            74 => "电磁场持续时间+4s",
            75 => "电流直击进化,相关所有伤害+200%",
            76 => "麻痹时间+1.5s",

            //预留十个
            85 => "车次数+1",
            86 => "伤害-20%, 车次数+2",
            87 => "击退+100%,10%几率眩晕1s",
            88 => "速度加快20%,冷却-25%",
            89 => "点燃怪物3s",
            90 => "体积增大20%",
            91 => "伤害+80%",

            //能量射线
            95 => "伤害次数+5",
            96 => "次数翻倍,冷却+50%",
            97 => "范围增加150%",
            98 => "伤害的怪物5s内受到的伤害+25%，不叠加",
            99 => "伤害+80%",
            100 => "减速80%",
            101 => "能量射线进化,变高能光波,伤害翻倍,附带击退",

            //激光
            111 => "伤害+60%",
            112 => "主目标伤害+200%",
            113 => "伤害次数+15",
            114 => "伤害次数+25",
            115 => "激光分裂数+2",
            116 => "激光和电流直击伤害+60%",
            117 => "主目标持续释放电流直击",

            //寒冰绽放
            127 => "伤害+60%",
            128 => "范围+100%",
            129 => "持续时间+2s",
            130 => "次数+1",
            132 => "中心伤害翻倍",


            //跳跃电子
            142 => "伤害+60%",
            143 => "次数+1",
            144 => "伤害-20%,次数+2",
            145 => "弹射次数+2",
            146 => "麻痹时间+2s",
            147 => "弹射目标处释放1次无强化电流直击",
            148 => "弹射目标受到的伤害增加25%",
            149 => "弹射目标处发生爆炸",
            150 => "爆炸范围+80%",


            //龙卷风
            160 => "伤害+60%",
            161 => "速度+40%,持续时间+40%",
            162 => "持续时间+100%",
            163 => "牵引力+40%",
            164 => "双重龙卷,持续时间-30%",
            165 => "范围+50%",
            166 => "冰龙卷",
            167 => "雷龙卷",

            //飞龙投射
            177 => "次数+1",
            178 => "伤害-20%,次数+2",
            179 => "范围+50%",
            180 => "击退+50%",
            181 => "伤害+80%",

            //压缩气刃
            191 => "次数+1",
            192 => "伤害-20%,次数+2",
            193 => "连发+1",
            194 => "齐射+1",
            195 => "穿透+6",
            196 => "体积增大50%",
            197 => "伤害+80%",
            198 => "冷却-20%,伤害+30%",
            199 => "击退+50%",

            //滞留火焰
            209 => "次数+1",
            210 => "伤害-20%, 次数+2",
            211 => "灼烧伤害+80%",
            212 => "点燃伤害+80%",
            213 => "范围增加100%",
            214 => "范围内减速50%",
            215 => "点燃的怪物死亡后留下一片无强化燃烧区域",
            216 => "滞留火焰进化, 灼烧伤害+200%, 点燃伤害+200%",

            //旋转利刃
            226 => "速度+40%,持续时间+40%",
            227 => "附加点燃",
            228 => "附加两秒眩晕",
            229 => "伤害+60%",
            230 => "持续时间+50%",
            231 => "体积增加50%",

            _ => throw new NotImplementedException(),


        };
    }
    #endregion
    #region 技能名字
    public static string IdToName(int id)
    {
        return id switch
        {
            0 => "高爆火球",
            1 => "寒冰弹",
            2 => "电流直击",
            3 => "地刺",
            4 => "能量射线",
            5 => "锁定激光",
            6 => "寒冰绽放",
            7 => "跳跃电子",
            8 => "龙卷风",
            9 => "飞龙投射",
            10 => "压缩气刃",
            11 => "滞留火焰",
            12 => "旋转利刃",
            13 => "弹道增幅",
            14 => "连发增强",
            15 => "魔法弹分裂",
            16 => "穿透强化",
            17 => "火系附魔",
            18 => "电系附魔",
            19 => "冰系附魔",
            20 => "魔法弹爆破",
            21 => "爆炸增幅",
            22 => "伤害增幅",
            23 => "超级增伤",
            24 => "次级增伤",
            25 => "冰片散射",
            26 => "射速提升",
            27 => "墙体反弹",
            28 => "冲撞强化",
            29 => "爆炸强化",
            30 => "爆炸范围扩展",
            31 => "数量提升I",
            32 => "数量提升II",
            33 => "点燃效果",
            34 => "灼烧增益",
            35 => "火焰碎片",
            36 => "爆炸传染",
            37 => "温压进化",
            48 => "齐射强化",
            49 => "连发强化",
            50 => "穿透提升I",
            51 => "穿透提升II",
            52 => "击退强化",
            53 => "冻伤叠加",
            54 => "击退扩展",
            55 => "冻结效果",
            56 => "干冰进化",
            57 => "数量提升I",
            58 => "数量提升II",
            66 => "穿刺增幅",
            67 => "穿刺数量增幅",
            68 => "伤害提升",
            69 => "范围追击",
            70 => "粒子释放",
            71 => "粒子爆炸",
            72 => "爆炸范围增大",
            73 => "电磁场",
            74 => "电磁场持续延长",
            75 => "电磁进化",
            76 => "麻痹延长",
            85 => "车辆增量",
            86 => "车辆增幅",
            87 => "眩晕增幅",
            88 => "冷却缩减",
            89 => "点燃效果",
            90 => "体积增大",
            91 => "伤害强化",
            95 => "高能增幅",
            96 => "冷却增幅",
            97 => "范围增幅",
            98 => "持续伤害增幅",
            99 => "伤害强化",
            100 => "减速增幅",
            101 => "高能进化",
            111 => "激光增伤",
            112 => "主目标增伤",
            113 => "次数增幅I",
            114 => "次数增幅II",
            115 => "激光分裂",
            116 => "穿透增伤",
            117 => "电磁附加",
            127 => "冰暴增伤",
            128 => "范围扩展",
            129 => "持续延长",
            130 => "次数增幅I",
            132 => "冷却大幅缩减",
            142 => "跃迁增伤",
            143 => "次数增幅I",
            144 => "次数增幅II",
            145 => "弹射增幅",
            146 => "麻痹增幅",
            147 => "电磁释放",
            148 => "受伤增幅",
            149 => "爆炸效果",
            150 => "爆炸范围扩展",
            160 => "伤害增幅",
            161 => "速度增幅",
            162 => "持续增幅",
            163 => "牵引力强化",
            164 => "双重龙卷",
            165 => "范围扩展",
            166 => "冰龙卷",
            167 => "雷龙卷",
            177 => "数量提升I",
            178 => "数量提升II",
            179 => "范围增幅",
            180 => "击退增幅",
            181 => "伤害强化",
            191 => "数量增幅I",
            192 => "数量增幅II",
            193 => "连发增幅",
            194 => "齐射增幅",
            195 => "穿透强化",
            196 => "体积扩展",
            197 => "伤害强化",
            198 => "冷却减少",
            199 => "击退强化",
            209 => "数量增幅I",
            210 => "数量增幅II",
            211 => "灼烧增伤",
            212 => "点燃增伤",
            213 => "范围扩展",
            214 => "减速效果",
            215 => "燃烧区域",
            216 => "燃油进化",
            226 => "速度增幅",
            227 => "点燃附加",
            228 => "眩晕附加",
            229 => "旋转利刃增伤",
            230 => "持续时间延长",
            231 => "体积扩展",
            _ => "",
        };
    }

    #endregion

    #region 技能最大选择数
    public static int IdToMaxSelCount(int id)
    {
        return id switch
        {
            13 => 3,
            14 => 2,
            15 => 2,
            16 => 2,
            26 => 2,
            27 => 2,
            28 => 2,
            29 => 2,
            31 => 2,
            47 => 2,
            48 => 2,
            57 => 2,
            66 => 2,
            68 => 2,
            85 => 2,
            90 => 2,
            99 => 2,
            111 => 3,
            115 => 2,
            143 => 2,
            146 => 3,
            177 => 2,
            179 => 2,
            181 => 3,
            191 => 2,
            193 => 2,
            194 => 3,
            199 => 2,
            209 => 2,
            211 => 3,
            212 => 2,
            229 => 2,
            230 => 2,
            231 => 2,
            _ => 1,
        };

    }
    #endregion
    //技能前置列表

    #region 技能前置

    public static Dictionary<int, List<int>> GetPreListDict()
    {
        Dictionary<int, List<int>> dict = new();
        for (int i = 0; i <= Constant.maxSkillId; i++)
        {
            dict.Add(i, IdToPreList(i));
        }
        return dict;
    }
    public static List<int> IdToPreList(int id)
    {
        if (CommonUtil.AsList(28, 29, 30, 31, 32, 33, 35, 36).Contains(id))
        {
            return CommonUtil.AsList(0);
        }
        if (id >= 47 && id <= 55)
        {
            return CommonUtil.AsList(1);
        }
        if (id >= 66 && id <= 71)
        {
            return CommonUtil.AsList(2);
        }
        if (id >= 85 && id <= 91)
        {
            return CommonUtil.AsList(3);
        }
        if (id >= 95 && id <= 10)
        {
            return CommonUtil.AsList(4);
        }
        if (id >= 111 && id <= 116)
        {
            return CommonUtil.AsList(5);
        }
        if (id >= 127 && id <= 132)
        {
            return CommonUtil.AsList(6);
        }
        if (id >= 142 && id <= 149)
        {
            return CommonUtil.AsList(7);
        }
        if (id >= 160 && id <= 165)
        {
            return CommonUtil.AsList(8);
        }
        if (id >= 177 && id <= 181)
        {
            return CommonUtil.AsList(9);
        }
        if (id >= 191 && id <= 199)
        {
            return CommonUtil.AsList(10);
        }
        if (id >= 209 && id <= 215)
        {
            return CommonUtil.AsList(11);
        }
        if (id >= 226 && id <= 231)
        {
            return CommonUtil.AsList(12);
        }
        return id switch
        {
            17 => CommonUtil.AsList(0),
            18 => CommonUtil.AsList(2),
            19 => CommonUtil.AsList(6),
            20 => CommonUtil.AsList(0),
            21 => CommonUtil.AsList(20),
            23 => CommonUtil.AsList(15),
            24 => CommonUtil.AsList(15),
            25 => CommonUtil.AsList(19),
            37 => CommonUtil.AsList(28, 29, 33),
            34 => CommonUtil.AsList(33),
            56 => CommonUtil.AsList(53, 55),
            72 => CommonUtil.AsList(71),
            73 => CommonUtil.AsList(71),
            74 => CommonUtil.AsList(73),
            75 => CommonUtil.AsList(68, 69),
            76 => CommonUtil.AsList(2),
            101 => CommonUtil.AsList(95, 96, 97, 98, 100),
            117 => CommonUtil.AsList(2, 5),
            150 => CommonUtil.AsList(149),
            166 => CommonUtil.AsList(8, 6),
            167 => CommonUtil.AsList(8, 2),
            216 => CommonUtil.AsList(211, 212, 214),

            _ => new List<int>(),
        };
    }
    #endregion

    #region 所有技能
    public static List<SkillNode> AllSkill()
    {
        List<SkillNode> all = new();
        for (int i = 0; i <= Constant.maxSkillId; i++)
        {
            if (IdToName(i) != "")
            {
                all.Add(CreateSkillNode(i));
            }

        }
        return all;
    }
    public static SkillNode CreateSkillNode(int id)
    {
        return new SkillNode(id, null, IdToConflictIds(id), IdToName(id), IdToDesc(id),
                    IdToMaxSelCount(id), IdToResName(id), IdToIsUnlocked(id), IdToIsSatisfied(id), IdToArmType(id));
    }
    #endregion

    #region 技能是否解锁
    public static bool IdToIsUnlocked(int id)
    {
        if (id <= 16)
        {
            return true;
        }
        return id switch
        {
            17 => false,
            22 => false,
            26 => false,
            27 => false,
            48 => false,
            193 => false,
            194 => false,
            _ => false,
        };
    }
    #endregion

    #region 技能是否满足前置,仅供初始
    public static bool IdToIsSatisfied(int id)
    {
        if (id <= 16)
        {
            return true;
        }
        return id switch
        {
            22 => true,
            _ => false,
        };
    }
    #endregion



    #region  技能使用函数
    public static Action IdToUseAction(int id)
    {
        if (id <= 12)
        {
            GameObject ArmsParent = GameObject.Find("Arms");
            string armType = IdToArmType(id);
            Debug.Log("armtype" + armType);
            GameObject arm = ArmsParent.transform.RecursiveFind(armType + "Arm").gameObject;
            return () => { arm.SetActive(true); };
        }
        return id switch
        {
            _ => () => { }
            ,
        };
    }
    #endregion
}