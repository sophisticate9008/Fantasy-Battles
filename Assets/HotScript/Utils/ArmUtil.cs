using System.Collections.Generic;

public static class ArmUtil
{
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
            6 => (10f, 0.1f),
            7 => (6f, 0.05f),
            8 => (10f, 0.2f),
            9 => (6f, 0.05f),
            10 => (4, 0.05f),
            11 => (4, 0.01f),
            12 => (8f, 0.05f),
            13 => (2, 0.02f),
            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion
    #region 持续时间等差数列
    #endregion
    public static string ArmTypeToFieldName(string armType)
    {
        return "levelArm" + ArmTypeToId(armType);
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




}