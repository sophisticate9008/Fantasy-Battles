using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemUtil
{
    public static Dictionary<int, float> probDictBlue = new()
    {
        { 1, 0.6f },  // Level 1, 概率 60%
        { 2, 0.3f },  // Level 2, 概率 30%
        { 3, 0.095f },  // Level 3, 概率 9.5%
        { 4, 0.0035f }, // Level 4, 概率 0.35%
        { 5, 0.0015f }  // Level 5, 概率 0.15%
    };

    public static Dictionary<int, float> probDictPurple = new()
    {
        { 2, 0.5f },      // Level 2, 概率 50%
        { 3, 0.36f },      // Level 3, 概率 36%
        { 4, 0.1f },    // Level 4, 概率 10%
        { 5, 0.024f },   // Level 5, 概率 2.4%
        { 6, 0.01f },   // Level 6, 概率 1%
        { 7, 0.006f }     // Level 7, 概率 0.6%
    };
    public static Color LevelToColor(int level)
    {
        return HexToColor(LevelToColorHex(level));

    }
    public static string LevelToColorHex(int level)
    {
        return level switch
        {
            1 => "#787878",
            2 => "#4E8E00",
            3 => "#0F58D7",
            4 => "#A30ED7",
            5 => "#D77D0E",
            6 => "#D7290F",
            7 => "#E523E6",
            _ => throw new System.NotImplementedException(),
        };
    }
    public static string LevelToColorString(int level)
    {
        return level switch
        {
            1 => "gray",
            2 => "green",
            3 => "blue",
            4 => "purple",
            5 => "orange",
            6 => "red",
            7 => "muticolour",
            _ => throw new System.NotImplementedException(),
        };
    }
    
    public static string LevelToJewelSimpleName(int level)
    {
        return level switch
        {
            1 => "普通宝石",
            2 => "精良宝石",
            3 => "卓越宝石",
            4 => "完美宝石",
            5 => "传说宝石",
            6 => "绝世宝石",
            7 => "至尊宝石",
            _ => throw new System.NotImplementedException(),
        };
    }
    public static string LevelToJewelResName(int level)
    {
        return level switch
        {
            1 => "icon_baoshi_1",
            2 => "icon_baoshi_2",
            3 => "icon_baoshi_3",
            4 => "icon_baoshi_4",
            5 => "icon_baoshi_5",
            6 => "icon_baoshi_6",
            7 => "icon_baoshi_7",
            _ => throw new System.NotImplementedException(),
        };
    }
    public static string PlaceIdToPlaceName(int placeId) {
        return placeId switch
        {
            1 => "头盔",
            2 => "护臂",
            3 => "裤子",
            4 => "盔甲",
            5 => "鞋子",
            6 => "手套",
            _ => throw new System.NotImplementedException(),
        };
    }
    public static string ProbDictToString(Dictionary<int, float> probabilityDict)
    {
        string result = "";
        foreach (var probability in probabilityDict) {
            int level = probability.Key;
            float prob = probability.Value;
            result += $"<color={LevelToColorHex(level)}>{LevelToJewelSimpleName(level)}  {prob * 100}% </color> \n";
        }
        return result;
    }
    /// <summary>
    /// 将16进制颜色代码转换为Unity的Color对象
    /// 支持格式：#RRGGBB 或 #RRGGBBAA
    /// </summary>
    /// <param name="hex">16进制颜色字符串</param>
    /// <returns>解析后的Color对象</returns>
    public static Color HexToColor(string hex)
    {
        Color color;
        // 使用Unity内置的TryParseHtmlString来解析16进制字符串
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogError("Invalid hex color code: " + hex);
            return Color.white; // 如果解析失败，返回默认的白色
        }
    }
    public static int GetRandomLevel(Dictionary<int, float> probabilityDict, int minLevel = 1)
    {
        // 创建合并后的概率字典，将低于保底等级的项合并
        Dictionary<int, float> adjustedDict = new Dictionary<int, float>();
        float accumulatedLowProb = 0f;  // 用于累积低于保底等级的概率

        foreach (var kvp in probabilityDict)
        {
            if (kvp.Key < minLevel)
            {
                accumulatedLowProb += kvp.Value;  // 累积低于保底等级的概率
            }
            else
            {
                adjustedDict[kvp.Key] = kvp.Value;
            }
        }

        // 将累积的低等级概率加到保底等级上
        if (adjustedDict.ContainsKey(minLevel))
        {
            adjustedDict[minLevel] += accumulatedLowProb;
        }
        else
        {
            adjustedDict[minLevel] = accumulatedLowProb;
        }

        // 随机值并累积概率找到对应的等级
        float randomValue = Random.value;
        float cumulativeWeight = 0f;

        foreach (var kvp in adjustedDict)
        {
            cumulativeWeight += kvp.Value;
            if (randomValue <= cumulativeWeight)
            {
                return kvp.Key;  // 返回抽到的等级
            }
        }

        // 默认返回最低保底等级（通常不会执行到这里）
        return minLevel;
    }

}