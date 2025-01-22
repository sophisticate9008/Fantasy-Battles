using System;
using System.IO;

using UnityEngine;


public static class ConfigFactory
{
    public static ConfigBase Create(string configName)
    {
        if (!configName.Contains("Config"))
        {
            configName += "Config";
        }

        ConfigBase config;
        Type type = CommonUtil.GetTypeByName(configName);
        if (CommonUtil.IsImplementsInterface<IFlagInjectFromFile>(type))
        {
            string fileStr = Path.Combine(Constant.ConfigsPath, $"{configName}.json");
            if (File.Exists(fileStr))
            {
                Debug.Log(configName + "配置文件存在，注入数据");
                string json = File.ReadAllText(fileStr);

                // 反序列化 JSON 到具体类型
                config = JsonUtility.FromJson(json, type) as ConfigBase;
                return config;
            }
        }

        config = Activator.CreateInstance(type) as ConfigBase;
        return config;

    }
}
