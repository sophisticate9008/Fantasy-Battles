using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
}