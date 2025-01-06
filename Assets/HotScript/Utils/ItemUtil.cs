
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            1 => "#BABABA",
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
            7 => "multicolor",
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
            1 => "baoshi_1",
            2 => "baoshi_2",
            3 => "baoshi_3",
            4 => "baoshi_4",
            5 => "baoshi_5",
            6 => "baoshi_6",
            7 => "baoshi_7",
            _ => throw new System.NotImplementedException(),
        };
    }
    public static string PlaceIdToPlaceName(int placeId)
    {
        return placeId switch
        {
            1 => "法球",
            2 => "腰带",
            3 => "戒指",
            4 => "头冠",
            5 => "匕首",
            6 => "吊坠",
            _ => throw new System.NotImplementedException(),
        };
    }
    public static string VarNameToSipleName(string varName)
    {
        return varName switch
        {
            "keyPurple" => "紫钥匙",
            "keyBlue" => "蓝钥匙",
            "washWater" => "洗练试剂",
            _ => throw new System.NotImplementedException(),
        };
    }
    public static string ProbDictToString(Dictionary<int, float> probabilityDict)
    {
        string result = "";
        foreach (var probability in probabilityDict)
        {
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
        // 使用Unity内置的TryParseHtmlString来解析16进制字符串
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
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
        Dictionary<int, float> adjustedDict = new();
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
        float randomValue = UnityEngine.Random.value;
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
    public static void ChangeTextColor(Transform transform, int level)
    {
        // 获取 TextMeshPro 组件
        if (!transform.TryGetComponent<TextMeshProUGUI>(out var tmp))
        {
            Debug.LogError("TextMeshProUGUI component not found on the given transform.");
            return;
        }
        // 创建材质实例，避免修改共享材质
        Material newMaterial = new(tmp.fontMaterial);

        // 根据等级设置颜色
        Color baseColor = ItemUtil.LevelToColor(level); // 假设 LevelToColor 返回的是一个 Color

        // 转换为 HDR 颜色并增加亮度
        float hdrIntensity = level < 7 ? 1.13954f : 2.456513f; // 亮度强度因子，可以理解为“加 1 的效果”
        Color hdrColor = baseColor * hdrIntensity;
        // 设置 HDR Face Color
        newMaterial.SetColor("_FaceColor", hdrColor);

        // 应用新的材质
        tmp.fontMaterial = newMaterial;
    }
    public static void SetSprite(Transform transform, string resName)
    {
        Image pic = transform.GetComponent<Image>();
        pic.sprite = CommonUtil.GetAssetByName<Sprite>(resName);
    }
}