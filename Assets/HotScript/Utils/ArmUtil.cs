using System.Collections.Generic;
using System.Linq;

public static class ArmUtil
{
    public static List<string> AllArmTypes => SkillUtil.armTypeDict.Values.ToList();
    public static string ArmTypeToDamagePos(string ArmType)
    {
        return ArmType switch
        {
            "GroundStab" => "land",
            "FlameOrb" => "land",
            "IceBloom" => "land",
            _ => "all",
        };
    }
    #region  伤害类型
    public static string ArmTypeToDamageType(string ArmType)
    {
        return ArmType switch
        {
            "BoomFireBall" => "fire",//0
            "IceBall" => "ice",//1
            "ElectroHit" => "elec",//2
            "GroundStab" => "ad",//3
            "EnergyRay" => "energy",//4
            "Laser" => "energy",//5
            "IceBloom" => "ice",//6
            "JumpElectro" => "elec",//7
            "Tornado" => "wind",//8
            "DragonLaunch" => "ad",//9
            "PressureCutter" => "wind",//10
            "FlameOrb" => "fire",//11
            "WhirlingBlade" => "ad",//12
            "MagicBullet" => "ad",//13
            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion
    #region 倍率等差数列
    public static (float a1, float d) ArmTypeToTlcAP(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => (3, 0.5f),
            1 => (2, 0.4f),
            2 => (4, 0.6f),
            3 => (0.3f, 0.1f),
            4 => (2, 0.2f),
            5 => (0.6f, 0.1f),
            6 => (0.4f, 0.2f),
            7 => (1f, 0.1f),
            8 => (0.7f, 0.1f),
            9 => (5, 0.7f),
            10 => (3f, 0.3f),
            11 => (6, 0.2f),
            12 => (1.5f, 0.2f),
            13 => (1, 0.1f),

            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion
    #region  攻击范围
    public static float ArmTypeToRangeFire(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => 6f,
            1 => 6f,
            2 => 6f,
            3 => 6f,
            4 => 6f,
            5 => 6f,
            6 => 6f,
            7 => 6f,
            8 => 6f,
            9 => 6f,
            10 => 6f,
            11 => 6f,
            12 => 6f,
            13 => 6f,
            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion
    #region  cd等差数列

    public static (float a1, float d) ArmTypeToCdAP(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => (5, 0.05f),
            1 => (5, 0.05f),
            2 => (4, 0.05f),
            3 => (10, 0.05f),
            4 => (10, 0.1f),
            5 => (8f, 0.1f),
            6 => (6f, 0.1f),
            7 => (6f, 0.05f),
            8 => (10f, 0.2f),
            9 => (6f, 0.05f),
            10 => (4, 0.05f),
            11 => (4, 0.01f),
            12 => (10f, 0.05f),
            13 => (2, 0.02f),
            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion
    #region 持续时间等差数列
    #endregion
    public static string ArmTypeToLevelFieldName(string armType)
    {
        return "levelArm" + ArmTypeToId(armType);
    }
    public static string ArmTypeToChipFieldName(string armType)
    {
        return "armChip" + ArmTypeToId(armType);
    }
    public static int ArmTypeToId(string armType)
    {
        foreach (var item in SkillUtil.armTypeDict)
        {
            if (item.Value == armType)
            {
                return item.Key;
            }
        }
        return -1;
    }

    #region 武器名字
    public static string ArmTypeToArmName(string armType)
    {
        int id = ArmTypeToId(armType);
        if (id < 13)
        {
            return SkillUtil.IdToName(id);
        }
        return "魔法弹";
    }
    #endregion
    public static string ArmTypeToChipResName(string armType)
    {
        return "chip_" + armType;
    }
    public static float ArmTypeToDuration(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            4 => 3,
            5 => 4,
            6 => 3,
            8 => 15,
            11 => 3,
            12 => 10,
            _ => 20,
        };
    }
    #region  武器穿透
    public static int ArmTypeToPenetration(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => 1,
            1 => 3,
            10 => 5,
            13 => 1,
            _ => 999,
        };
    }
    #endregion
    #region  武器介绍
    public static string ArmTypeToDes(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => "每穿透一个敌人时爆炸并造成击退",
            1 => "造成冰冻伤害并轻微击退",
            2 => "小范围伤害,并造成麻痹伤害,同时也是解锁电系魔法弹的前置技能",
            3 => "从底端到顶端,造成减速和小击退",
            4 => "穿透所有敌人",
            5 => "锁定一个目标持续到结束或对方死亡,并且对路径造成伤害",
            6 => "持续对敌人造成冰冻",
            7 => "在敌人之间进行跳跃,造成单体伤害,同时造成麻痹",
            8 => "持续牵引敌人",
            9 => "选定一个位置投下石块,造成范围物理伤害和击退",
            10 => "轻微击退",
            11 => "选定一个位置生成滞留的火焰区域,造成较高伤害",
            12 => "生成旋转的利刃持续造成物理伤害",
            13 => "魔法弹,最基础的技能",
            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion 
    #region 攻击冷却
    public static float ArmTypeToAttackCd(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            4 => 0.2f,
            5 => 0.2f,
            6 => 0.5f,
            8 => 0.2f,
            11 => 0.5f,
            12 => 0.2f,
            13 => 1f,
            _ => 0.5f,
        };
    }
    #endregion


}