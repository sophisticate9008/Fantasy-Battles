using System;
using System.IO;


using UnityEngine;


public class ConfigFactory
{

    public static IConfig CreateInjectedConfig(string configName)
    {
        if (!configName.Contains("Config"))
        {
            configName += "Config";
        }
        string fileStr = Path.Combine(Constant.ConfigsPath, $"{configName}.json");

        Type type = CommonUtil.GetTypeByName(configName);

        if (File.Exists(fileStr))
        {
            Debug.Log(configName + "配置文件存在，注入数据");
            string json = File.ReadAllText(fileStr);

            // 反序列化 JSON 到具体类型
            IConfig config = JsonUtility.FromJson(json, type) as IConfig;
            return config;
        }
        else
        {
            Debug.Log(configName + "配置文件不存在，创建新的");
            // 使用反射实例化具体类型
            IConfig config = Activator.CreateInstance(type) as IConfig;
            if (config != null)
            {
                Debug.Log(config.GetType() + "创建成功");
            }
            return config;
        }
    }
}

