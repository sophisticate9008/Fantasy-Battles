using System;
using System.Collections.Generic;
using UnityEngine;

public static class SkillUtil
{
    #region 技能冲突
    public static readonly List<List<int>> conflictLists = new() {
        CommonUtil.AsList(15,17,18,19),
        CommonUtil.AsList(113, 114),
        CommonUtil.AsList(166,167)
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
        {"icon_IceBall", CommonUtil.AsList(1, 48, 50, 51, 52, 53, 54, 55, 56, 57, 58)},
        {"icon_ElectroHit", CommonUtil.AsList(2, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76)},
        {"icon_GroundStab", CommonUtil.AsList(3, 85, 86, 87, 88, 89, 90, 91)},
        {"icon_EnergyRay", CommonUtil.AsList(4, 95, 96, 97, 98, 99, 100, 101)},
        {"icon_Laser", CommonUtil.AsList(5, 111, 112, 113, 114, 115, 116, 117)},
        {"icon_IceBloom", CommonUtil.AsList(6, 127, 128, 129, 130, 131, 132)},
        {"icon_JumpElectro", CommonUtil.AsList(7, 142, 143, 144, 145, 146, 147, 148, 149, 150)},
        {"icon_Tornado", CommonUtil.AsList(8, 160, 161, 162, 163, 164, 165, 166, 167)},
        {"icon_DragonLaunch", CommonUtil.AsList(9, 177, 178, 179, 180, 181)},
        {"icon_PressureCutter", CommonUtil.AsList(10, 191, 192,  194, 195, 196, 197, 198, 199)},
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
            50 => "寒冰弹伤害-20%, 穿透+2",
            51 => "寒冰弹伤害+60%,穿透+1",
            52 => "寒冰弹伤害+100%",
            53 => "寒冰弹叠加冻伤,上限加5层，持续5s",
            54 => "寒冰弹穿透+1,击退+60%",
            55 => "寒冰弹命中冻结敌人2s",
            56 => "寒冰弹进化,每次命中分裂1个原始冰弹,继承加成",
            57 => "数量+1",
            58 => "数量+2",
            //预留十个
            66 => "电磁穿刺次数+1",
            67 => "次数+2",
            68 => "伤害+80%",
            69 => "杀死怪物后再释放一次",
            70 => "命中后释放6个粒子",
            71 => "命中后爆炸",
            72 => "爆炸范围+80%",
            73 => "爆炸范围内生成电磁场,持续两秒,减速50%,造成攻击力100%的伤害",
            74 => "电磁场持续时间+4s",
            75 => "电流直击进化,相关所有伤害+200%",
            76 => "麻痹时间+1.5s",

            //预留十个
            85 => "次数+1",
            86 => " 次数+2",
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
            101 => "能量射线进化,变高能光波,伤害增加100%,附带击退",

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
            130 => "多释放+1",
            132 => "可以叠加冻伤,冻伤层数上限+5",
            131 => "5%概率深度冻结,无视冰冻抗性",
            133 => "结束后释放6个冰魔法弹冰片",

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

            194 => "齐射+1",
            195 => "穿透+6",
            196 => "体积增大50%,伤害增加50%",
            197 => "伤害+100%",
            198 => "冷却-20%,伤害+30%",
            199 => "击退+50%",

            //滞留火焰
            209 => "次数+1",
            210 => "伤害-20%, 次数+2",
            211 => "灼烧伤害+80%",
            212 => "点燃伤害倍率+0.5",
            213 => "范围增加100%",
            214 => "范围内减速50%",
            215 => "点燃的怪物死亡后留下一片无强化燃烧区域",
            216 => "滞留火焰进化, 灼烧伤害+100%, 点燃倍率+0.8",

            //旋转利刃
            226 => "速度+40%,持续时间+2s",
            227 => "附加点燃",
            228 => "附加两秒眩晕",
            229 => "伤害+60%",
            230 => "持续时间+50%",
            231 => "体积增加50%",
            232 => "每次命中30次敌人时,释放一个无强化的小型利刃",
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
            131 => "深度冻结",
            132 => "冻伤解放",
            133 => "释放冰片",

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
            130 => 2,
            143 => 2,
            146 => 3,
            177 => 2,
            179 => 2,
            181 => 3,
            191 => 2,

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
        if (id >= 47 && id <= 65)
        {
            return CommonUtil.AsList(1);
        }
        if (id >= 66 && id <= 84)
        {
            return CommonUtil.AsList(2);
        }
        if (id >= 85 && id <= 94)
        {
            return CommonUtil.AsList(3);
        }
        if (id >= 95 && id <= 110)
        {
            return CommonUtil.AsList(4);
        }
        if (id >= 111 && id <= 126)
        {
            return CommonUtil.AsList(5);
        }
        if (id >= 127 && id <= 141)
        {
            return CommonUtil.AsList(6);
        }
        if (id >= 142 && id <= 159)
        {
            return CommonUtil.AsList(7);
        }
        if (id >= 160 && id <= 176)
        {
            return CommonUtil.AsList(8);
        }
        if (id >= 177 && id <= 190)
        {
            return CommonUtil.AsList(9);
        }
        if (id >= 191 && id <= 208)
        {
            return CommonUtil.AsList(10);
        }
        if (id >= 209 && id <= 225)
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
        List<int> lockList = CommonUtil.AsList(75, 17, 22, 26, 27, 37, 48, 49, 56, 194, 89, 117, 131, 216, 232);
        // if (id <= 16)
        // {
        //     return true;
        // }
        if (lockList.Contains(id))
        {
            return false;
        }
        return true;
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
            13 => () => { ArmUtil.magicBulletConfig.MultipleLevel += 1; }
            ,//"魔法弹弹道数量+1",
            14 => () => { ArmUtil.magicBulletConfig.RepeatLevel += 1; }
            , //"魔法弹连发数+1",
            15 => () =>
            {
                ArmUtil.magicBulletConfig.MagicBulletFissionConfig.MultipleLevel += 2;
                ArmUtil.magicBulletConfig.ComponentStrs.Add("分裂");
            }
            ,//"魔法弹分裂+2",
            16 => () => { ArmUtil.magicBulletConfig.PenetrationLevel += 1; ArmUtil.magicBulletConfig.addition += 0.3f; }
            , //"魔法弹穿透+1,伤害+30%",
            17 => () => { ArmUtil.magicBulletConfig.DamageType = "fire"; ArmUtil.magicBulletConfig.ComponentStrs.Add("点燃"); }
            ,//"魔法弹变为火系魔法弹",
            18 => () => { ArmUtil.magicBulletConfig.DamageType = "elec"; ArmUtil.magicBulletConfig.ComponentStrs.Add("麻痹"); }
            ,//"魔法弹变为电系魔法弹",
            19 => () => { ArmUtil.magicBulletConfig.DamageType = "ice"; ArmUtil.magicBulletConfig.ComponentStrs.Add("冰冻"); }
            ,//"魔法弹变为冰系魔法弹",
            20 => () => { ArmUtil.magicBulletConfig.ComponentStrs.Add("爆炸"); }
            ,//"魔法弹爆炸",
            21 => () => { (ArmUtil.magicBulletConfig.BoomChildConfig as MagicBulletBoomConfig).SelfScale *= 1.6f; }
            ,//"魔法弹爆炸范围增加60%",
            22 => () => { ArmUtil.magicBulletConfig.addition += 0.6f; }
            ,//"魔法弹伤害+60%",
            23 => () => { ArmUtil.magicBulletConfig.addition += 1; ArmUtil.magicBulletConfig.MagicBulletFissionConfig.addition += 1; }
            ,//"魔法弹与次级魔法弹伤害+100%",
            24 => () => { ArmUtil.magicBulletConfig.MagicBulletFissionConfig.addition += 1; }
            ,//"次级魔法弹伤害+100%",
            25 => () =>
            {
                ArmUtil.magicBulletConfig.MagicBulletFissionConfig.MultipleLevel = 4;
                ArmUtil.magicBulletConfig.MagicBulletFissionConfig.DamageType = "ice";
                ArmUtil.magicBulletConfig.ComponentStrs.Add("分裂");
            }
            ,//"魔法弹命中后散发四个小冰片",
            26 => () => { ArmUtil.magicBulletConfig.AttackCd *= 0.9f; }
            ,//"射速增加10%",
            27 => () => { ArmUtil.magicBulletConfig.ReboundCount += 1; }
            ,//"魔法弹碰到墙壁反弹次数+1",


            28 => () => { ArmUtil.boomFireBallConfig.addition += 0.75f; ArmUtil.boomFireBallConfig.BoomChildConfig.MaxForce *= 1.6f; }
            ,//"高爆火球冲撞伤害+75%,爆炸击退+60%",
            29 => () => { ArmUtil.boomFireBallConfig.addition += 0.8f; }
            ,//"高爆火球爆炸伤害+80%",
            30 => () => { ArmUtil.boomFireBallConfig.BoomChildConfig.SelfScale += 0.8f; }
            ,//"高爆火球爆炸范围+80%",
            31 => () => { ArmUtil.boomFireBallConfig.AttackCount += 1; }
            ,//"高爆火球数量+1",
            32 => () => { ArmUtil.boomFireBallConfig.AttackCount += 2; }
            , //"高爆火球数量+2",
            33 => () => { ArmUtil.boomFireBallConfig.BoomChildConfig.FireTime += 6; ArmUtil.boomFireBallConfig.BoomChildConfig.ComponentStrs.Add("点燃"); }
            ,//"高爆火球点燃敌人6s",
            34 => () => { ArmUtil.boomFireBallConfig.BoomChildConfig.FirePercentage += 0.03f; }
            ,//"高爆火球点燃附加最大生命值3%",
            35 => () => { ArmUtil.boomFireBallConfig.ComponentStrs.Add("分裂"); }
            , //"高爆火球击中释放3个火焰碎片",
            36 => () =>
            {
                ArmUtil.boomFireBallConfig.typeActions["enter"].Add((selfObj, enemyObj) =>
                {
                    var eb = enemyObj.GetComponent<EnemyBase>();
                    eb.allTypeActions["die"].Add(() =>
                    {
                        var InitConfig = ArmUtil.boomFireBallConfig.CreateInitConfig<BoomFireBallBoomConfig>(false);
                        FighteManager.Instance.AttackWithCustomConfig(enemyObj, InitConfig, selfObj, 1);
                    });
                });
            }
            ,//"高爆火球点燃的怪物死亡后爆炸",
            37 => () =>
            {
                ArmUtil.boomFireBallConfig.PenetrationLevel += 2;
                ArmUtil.boomFireBallConfig.SelfScale *= 1.5f;
                ArmUtil.boomFireBallConfig.BoomChildConfig.SelfScale += 0.8f;
                ArmUtil.boomFireBallConfig.BoomChildConfig.MaxForce *= 1.6f;
            }
            , //"高爆火球进化,穿透+2, 爆炸范围+80%, 爆炸击退+60%",
            //预留10个
            48 => () => { ArmUtil.iceBallConfig.MultipleLevel += 1; }
            , //"寒冰弹齐射+1",
            50 => () => { ArmUtil.iceBallConfig.addition -= 0.2f; ArmUtil.iceBallConfig.PenetrationLevel += 2; }
            ,// "寒冰弹伤害-20%, 穿透+2",
            51 => () => { ArmUtil.iceBallConfig.addition += 0.6f; ArmUtil.iceBallConfig.PenetrationLevel += 1; }
            ,//"寒冰弹伤害+60%,穿透+1",
            52 => () => { ArmUtil.iceBallConfig.addition += 1; }
            ,//"寒冰弹伤害+100%",
            53 => () => { ArmUtil.iceBallConfig.ComponentStrs.Add("冻伤"); ArmUtil.globalConfig.freezenHurtMaxLevel += 5; }
            ,//"寒冰弹叠加冻伤,上限加5层，持续5s",
            54 => () => { ArmUtil.iceBallConfig.PenetrationLevel += 1; ArmUtil.iceBallConfig.MaxForce *= 0.6f; }
            ,//"寒冰弹穿透+1,击退+60%",
            55 => () => { ArmUtil.iceBallConfig.ComponentStrs.Add("冰冻"); ArmUtil.iceBallConfig.FreezeTime += 2; }
            ,//"寒冰弹命中冻结敌人2s",
            56 => () => { ArmUtil.iceBallConfig.IsUpgrade = true; }
            ,//"寒冰弹进化,每次命中分裂1个原始冰弹,继承加成",
            57 => () => { ArmUtil.iceBallConfig.AttackCount += 1; }
            ,//"数量+1",
            58 => () => { ArmUtil.iceBallConfig.AttackCount += 2; }
            ,//"数量+2",
            //预留十个
            66 => () => { ArmUtil.electroHitConfig.AttackCount += 1; }
            ,//"电磁穿刺次数+1",
            67 => () => { ArmUtil.electroHitConfig.AttackCount += 2; }
            ,//"次数+2",
            68 => () => { ArmUtil.electroHitConfig.addition += 0.8f; }
            ,//"伤害+80%",
            69 => () =>
            {
                ArmUtil.electroHitConfig.TheArm.killActions.Add(() =>
                {
                    ArmUtil.electroHitConfig.TheArm.Attack();
                });
            }
            , //"杀死怪物后再释放一次",
            70 => () => { ArmUtil.electroHitConfig.ComponentStrs.Add("分裂"); }
            ,// "命中后释放6个粒子",
            71 => () => { ArmUtil.electroHitConfig.ComponentStrs.Add("爆炸"); }
            ,//"命中后爆炸",
            72 => () => { ArmUtil.electroHitConfig.BoomChildConfig.SelfScale += 0.8f; }
            , //"爆炸范围+80%",
            73 => () => { ArmUtil.electroHitConfig.ComponentStrs.Add("Hold"); }
            , //"爆炸范围内生成电磁场,持续两秒,减速50%,造成攻击力50%的伤害",
            74 => () => { }
            ,//"电磁场持续时间+4s",
            75 => () => { ArmUtil.electroHitConfig.addition += 2; }
            ,//"电流直击进化,相关所有伤害+200%",
            76 => () => { ArmUtil.electroHitConfig.PalsyTime += 1.5f; }
            ,//"麻痹时间+1.5s",

            //预留十个
            85 => () => { ArmUtil.groundStabConfig.AttackCount += 1; }
            ,//"次数+1",
            86 => () => { ArmUtil.groundStabConfig.AttackCount += 2; }
            , // 次数+2",
            87 => () =>
            {
                ArmUtil.groundStabConfig.MaxForce *= 2;
                ArmUtil.groundStabConfig.ComponentStrs.Add("击退");
                ArmUtil.groundStabConfig.DizzyProb = 0.1f;
            }
            , //击退+100%,10%几率眩晕1s",
            88 => () => { ArmUtil.groundStabConfig.Speed += 0.2f; ArmUtil.groundStabConfig.Cd *= 0.75f; }
            , //速度加快20%,冷却-25%",
            89 => () => { ArmUtil.groundStabConfig.ComponentStrs.Add("点燃"); ArmUtil.groundStabConfig.FireTime = 3; }
            , //点燃怪物3s",
            90 => () => { ArmUtil.groundStabConfig.SelfScale += 0.2f; }
            , //体积增大20%",
            91 => () => { ArmUtil.groundStabConfig.addition += 0.8f; }
            , //伤害+80%",

            //能量射线
            95 => () => { ArmUtil.energyRayConfig.Duration += 1; }
            , //伤害次数+5",
            96 => () => { ArmUtil.energyRayConfig.Duration *= 2; ArmUtil.energyRayConfig.Cd *= 1.5f; }
            , //次数翻倍,冷却+50%",
            97 => () => { ArmUtil.energyRayConfig.SelfScale *= 1.5f; }
            , //范围增加150%",
            98 => () =>
            {
                ArmUtil.energyRayConfig.ComponentStrs.Add("易伤");
                ArmUtil.energyRayConfig.EasyHurtDegree += 0.25f;
            }
            , //伤害的怪物5s内受到的伤害+25%，不叠加",
            99 => () => { ArmUtil.energyRayConfig.addition += 0.8f; }
            , //伤害+80%",
            100 => () => { ArmUtil.energyRayConfig.SlowDegree += 0.8f; ArmUtil.energyRayConfig.ComponentStrs.Add("减速"); }
            , //减速80%",
            101 => () =>
            {
                ArmUtil.energyRayConfig.IsUpgrade = true;
                ArmUtil.energyRayConfig.addition += 1;
                ArmUtil.energyRayConfig.MaxForce += 10;
            }
            , //能量射线进化,变高能光波,伤害增加100%,附带击退",

            //激光
            111 => () => { ArmUtil.laserConfig.addition += 0.6f; }
            , //伤害+60%",
            112 => () => { ArmUtil.laserConfig.IsMainDamageUp = true; }
            , //主目标伤害+200%",
            113 => () => { ArmUtil.laserConfig.Duration += 3f; }
            , //伤害次数+15",
            114 => () => { ArmUtil.laserConfig.Duration += 5f; }
            , //伤害次数+25",
            115 => () => { ArmUtil.laserConfig.laserFissionConfig.FissionLevel += 2; }
            , //激光分裂数+2",
            116 => () => { ArmUtil.laserConfig.addition += 0.6f; ArmUtil.electroHitConfig.addition += 0.6f; }
            , //激光和电流直击伤害+60%",
            117 => () =>
            {
                ArmUtil.laserConfig.typeActions["stay"].Add((selfObj, enemyObj) =>
                {
                    ElectroHitConfig initConfig = ArmUtil.laserConfig.CreateInitConfig<ElectroHitConfig>();
                    FighteManager.Instance.AttackWithCustomConfig(enemyObj, initConfig, selfObj, 1);
                });
            }
            , //主目标持续释放电流直击",

            //寒冰绽放
            127 => () => { ArmUtil.iceBloomConfig.addition += 0.6f; }
            , //伤害+60%",
            128 => () => { ArmUtil.iceBloomConfig.SelfScale += 1; }
            , //范围+100%",
            129 => () => { ArmUtil.iceBloomConfig.Duration += 2; }
            , //持续时间+2s",
            130 => () => { ArmUtil.iceBloomConfig.AttackCount += 1; }
            , //多释放+1",
            132 => () => { ArmUtil.iceBloomConfig.ComponentStrs.Add("冻伤"); ArmUtil.globalConfig.freezenHurtMaxLevel += 5; }
            , //可以叠加冻伤",
            131 => () =>
            {
                ArmUtil.iceBloomConfig.typeActions["stay"].Add((selfObj, enemyObj) =>
                {
                    if (UnityEngine.Random.value <= 0.05f)
                    {
                        enemyObj.GetComponent<EnemyBase>().AddBuff("深度冻结", selfObj, 0.5f);
                    }
                });
            }
            , //5%概率深度冻结,无视冰冻抗性",
            133 => () =>
            {
                ArmUtil.iceBloomConfig.typeActions["return"].Add((selfObj, enemyObj) =>
                {
                    for (int i = 0; i < ArmUtil.iceBloomConfig.IceChipNum; i++)
                    {

                    }
                });
            }
            , //结束后释放6个冰魔法弹冰片",

            //跳跃电子
            142 => () => { ArmUtil.jumpElectroConfig.addition += 0.6f; }
            , //伤害+60%",
            143 => () => { ArmUtil.jumpElectroConfig.AttackCount += 1; }
            , //次数+1",
            144 => () => { ArmUtil.jumpElectroConfig.addition -= 0.2f; ArmUtil.jumpElectroConfig.AttackCount += 2; }
            , //伤害-20%,次数+2",
            145 => () => { ArmUtil.jumpElectroConfig.JumpCount += 2; }
            , //弹射次数+2",
            146 => () => { ArmUtil.jumpElectroConfig.PalsyTime += 2f; }
            , //麻痹时间+2s",
            147 => () =>
            {
                ArmUtil.jumpElectroConfig.typeActions["enter"].Add((selfObj, enemyObj) =>
                {
                    ArmConfigBase initConfig = ArmUtil.jumpElectroConfig.CreateInitConfig<ElectroHitConfig>();
                    FighteManager.Instance.AttackWithCustomConfig(enemyObj, initConfig, selfObj, 1);
                });
            }
            , //弹射目标处释放1次无强化电流直击",
            148 => () => { ArmUtil.jumpElectroConfig.ComponentStrs.Add("易伤"); ArmUtil.jumpElectroConfig.EasyHurtDegree += 0.25f; }
            , //弹射目标受到的伤害增加25%",
            149 => () => { ArmUtil.jumpElectroConfig.ComponentStrs.Add("爆炸"); }
            , //弹射目标处发生爆炸",
            150 => () => { ArmUtil.jumpElectroConfig.BoomChildConfig.SelfScale += 0.8f; }
            , //爆炸范围+80%",


            //龙卷风
            160 => () => { ArmUtil.tornadoConfig.addition += 0.6f; }
            , //伤害+60%",
            161 => () => { ArmUtil.tornadoConfig.Speed *= 0.4f; ArmUtil.tornadoConfig.Duration += 2f; }
            , //速度+40%,持续时间+40%",
            162 => () => { ArmUtil.tornadoConfig.Duration *= 2f; }
            , //持续时间+100%",
            163 => () => { ArmUtil.tornadoConfig.MaxForce *= 1.4f; }
            , //牵引力+40%",
            164 => () => { ArmUtil.tornadoConfig.AttackCount += 1; ArmUtil.tornadoConfig.Duration *= 0.7f; }
            , //双重龙卷,持续时间-30%",
            165 => () => { ArmUtil.tornadoConfig.SelfScale += 0.5f; }
            , //范围+50%",
            166 => () => { ArmUtil.tornadoConfig.IsIceTornado = false; }
            , //冰龙卷",
            167 => () => { ArmUtil.tornadoConfig.IsElecTornado = true; }
            , //雷龙卷",

            //飞龙投射
            177 => () => { ArmUtil.dragonLaunchConfig.AttackCount += 1; }
            , //次数+1",
            178 => () => { ArmUtil.dragonLaunchConfig.AttackCount += 2; ArmUtil.dragonLaunchConfig.addition -= 0.2f; }
            , //伤害-20%,次数+2",
            179 => () => { ArmUtil.dragonLaunchConfig.SelfScale += 0.5f; }
            , //范围+50%",
            180 => () => { ArmUtil.dragonLaunchConfig.MaxForce *= 1.5f; }
            , //击退+50%",
            181 => () => { ArmUtil.dragonLaunchConfig.addition += 0.8f; }
            , //伤害+80%",

            //压缩气刃
            191 => () => { ArmUtil.pressureCutterConfig.AttackCount += 1; }
            , //次数+1",
            192 => () => { ArmUtil.pressureCutterConfig.AttackCount += 2; ArmUtil.pressureCutterConfig.addition -= 0.2f; }
            , //伤害-20%,次数+2",
            194 => () => { ArmUtil.pressureCutterConfig.MultipleLevel += 1; }
            , //齐射+1",
            195 => () => { ArmUtil.pressureCutterConfig.PenetrationLevel += 6; }
            , //穿透+6",
            196 => () => { ArmUtil.pressureCutterConfig.SelfScale += 0.5f; ArmUtil.pressureCutterConfig.addition += 0.5f; }
            , //体积增大50%",
            197 => () => { ArmUtil.pressureCutterConfig.addition += 1f; }
            , //伤害+100%",
            198 => () => { ArmUtil.pressureCutterConfig.Cd *= 0.8f; ArmUtil.pressureCutterConfig.addition += 0.3f; }
            , //冷却-20%,伤害+30%",
            199 => () => { ArmUtil.pressureCutterConfig.MaxForce *= 1.5f; }
            , //击退+50%",

            //滞留火焰
            209 => () => { ArmUtil.flameOrbConfig.AttackCount += 1; }
            , //次数+1",
            210 => () => { ArmUtil.flameOrbConfig.addition -= 0.2f; ArmUtil.flameOrbConfig.AttackCount += 2; }
            , //伤害-20%, 次数+2",
            211 => () => { ArmUtil.flameOrbConfig.addition += 0.8f; }
            , //灼烧伤害+80%",
            212 => () => { ArmUtil.flameOrbConfig.fireTlc += 0.5f; }
            , //点燃伤害+50%",
            213 => () => { ArmUtil.flameOrbConfig.SelfScale += 1; }
            , //范围增加100%",
            214 => () => { ArmUtil.flameOrbConfig.SlowDegree += 0.5f; ArmUtil.flameOrbConfig.ComponentStrs.Add("减速"); }
            , //范围内减速50%",
            215 => () =>
            {
                ArmUtil.flameOrbConfig.typeActions["enter"].Add((selfObj, enemyObj) =>
                {
                    EnemyBase eb = enemyObj.GetComponent<EnemyBase>();
                    eb.allTypeActions["die"].Add(() =>
                    {
                        if (eb.buffEffects.Contains("滞留火焰点燃"))
                        {
                            FlameOrbConfig initConfig = ArmUtil.flameOrbConfig.CreateInitConfig<FlameOrbConfig>();
                            initConfig.ComponentStrs.Clear();
                            initConfig.Owner = nameof(FlameOrb);
                            FighteManager.Instance.AttackWithCustomConfig(enemyObj, initConfig, selfObj, 1);
                        }
                    });
                });
            }
            , //点燃的怪物死亡后留下一片无强化燃烧区域",
            216 => () => { ArmUtil.flameOrbConfig.IsUpgrade = true; }
            , //滞留火焰进化, 灼烧伤害+100%, 点燃伤害+100%",

            //旋转利刃
            226 => () => { ArmUtil.whirlingBladeConfig.Speed *= 0.4f; ArmUtil.whirlingBladeConfig.Duration += 0.4f; }
            , //速度+40%,持续时间+40%",
            227 => () => { ArmUtil.whirlingBladeConfig.IsFire = true; ArmUtil.whirlingBladeConfig.ComponentStrs.Add("点燃"); }
            , //附加点燃",
            228 => () => { ArmUtil.whirlingBladeConfig.DizzyTime = 2; ArmUtil.whirlingBladeConfig.ComponentStrs.Add("眩晕"); }
            , //附加两秒眩晕",
            229 => () => { ArmUtil.whirlingBladeConfig.addition += 0.6f; }
            , //伤害+60%",
            230 => () => { ArmUtil.whirlingBladeConfig.Duration *= 1.5f; }
            , //持续时间+50%",
            231 => () => { ArmUtil.whirlingBladeConfig.SelfScale += 0.5f; }
            , //体积增加50%",
            232 => () =>
            {
                FighteManager.Instance.AddAccumulateListener(nameof(WhirlingBlade), 30, (selfObj) =>
                {
                    WhirlingBladeConfig initConfig = ArmUtil.whirlingBladeConfig.CreateInitConfig<WhirlingBladeConfig>();
                    initConfig.SelfScale = 0.5f;
                    List<GameObject> objs = ArmUtil.whirlingBladeConfig.TheArm.FindRandomTarget();
                    GameObject targetEnemy;
                    if (objs.Count > 0)
                    {
                        targetEnemy = objs[0];
                    }
                    else
                    {
                        targetEnemy = null;
                    }
                    FighteManager.Instance.AttackWithCustomConfig(targetEnemy, initConfig, selfObj);

                });
            }
            , //每次命中30次敌人时,释放一个无强化的小型利刃",
        };

    }


    #endregion
}