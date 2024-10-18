using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ArmConfigs;
using MyBase;
using Prefabs.Enemy;
using UnityEngine;

namespace Factorys
{
    public class ConfigFactory
    {
        private static Type GetType(string configName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes()
                .FirstOrDefault(t => t.Name.Equals(configName + "Config", StringComparison.OrdinalIgnoreCase));

            return type ?? throw new NotImplementedException();
        }

        public static IConfig CreateInjectedConfig(string configName)
        {
            string fileStr = Path.Combine(Constant.ConfigsPath, $"{configName}.json");
            Type type = GetType(configName);

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
}
