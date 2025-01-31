using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YooAsset;

public class CommonUtil
{
    public static Type GetTypeByName(string typeName)
    {
        Debug.Log("获取类型：" + typeName);
        var assembly = Assembly.GetExecutingAssembly();
        var type = assembly.GetTypes()
            .FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));

        return type ?? throw new NotImplementedException($"未找到类型: {typeName}");
    }
    public static List<T> AsList<T>(params T[] items)
    {
        return new List<T>(items);
    }
    /// <summary>
    /// 获得资源
    /// </summary>
    public static T GetAssetByName<T>(string resName) where T : UnityEngine.Object
    {
        try
        {
            // 特殊处理 GameObject 类型资源
            if (typeof(T) == typeof(GameObject))
            {
                GameObject prefabs = GameObject.Find("Prefabs");
                Transform foundTransform = prefabs?.transform.RecursiveFind(resName);

                if (foundTransform != null)
                {
                    return foundTransform.gameObject as T;
                }
            }

            // 使用 YooAssets 加载资源
            return YooAssets.LoadAssetSync<T>(resName).AssetObject as T;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Error loading resource: {resName}. Exception: {ex.Message}");
            // 尝试再次加载资源作为最后的解决方案
            return YooAssets.LoadAssetSync<T>(resName).AssetObject as T;
        }

    }
    /// <summary>
    /// 判断某个类型是否实现了指定的接口
    /// </summary>
    /// <typeparam name="TInterface">接口类型</typeparam>
    /// <param name="type">要检查的类型</param>
    /// <returns>是否实现了指定接口</returns>
    public static bool IsImplementsInterface<TInterface>(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        if (!typeof(TInterface).IsInterface)
            throw new ArgumentException($"{typeof(TInterface).Name} 必须是接口类型。");

        return typeof(TInterface).IsAssignableFrom(type);
    }

    public static string ChangeTextColor(string text, string color)
    {
        return "<color=" + color + ">" + text + "</color>";
    }

    /// <summary>
    /// 将s个总量分到0到n中的x种
    /// </summary>
    public static List<(int id, int n)> DistributeRandomly(int n, int s, int x)
    {
        if (x == 0 || s == 0) return new List<(int, int)>(); // 如果没有分配数量，返回空

        System.Random rand = new();
        HashSet<int> selectedIndexes = new();

        // 随机选取 x 个不重复的序列号
        while (selectedIndexes.Count < x)
        {
            selectedIndexes.Add(rand.Next(0, n + 1));
        }

        List<int> portions = new List<int>(new int[x]);
        int remaining = s;

        // 随机分配 s 到 x 个序列
        for (int i = 0; i < x - 1; i++)
        {
            // 保证剩余的量足够分配
            int maxValue = remaining - (x - i - 1);
            if (maxValue < 1)
            {
                // 如果剩余的量无法满足分配，确保至少为1
                maxValue = 1;
            }
            int value = rand.Next(1, maxValue + 1); // 保证remaining和x-i-1的条件
            portions[i] = value;
            remaining -= value;
        }
        portions[x - 1] = remaining; // 剩余的数值给最后一个序列

        // 组装结果
        List<(int, int)> result = new List<(int, int)>();
        int index = 0;
        foreach (int seq in selectedIndexes)
        {
            result.Add((seq, portions[index++]));
        }

        return result;
    }

}