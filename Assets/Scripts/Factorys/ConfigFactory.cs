using System;
using System.IO;
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
            return configName switch
            {
                "Bullet" => typeof(BulletConfig),
                "BulletFission" => typeof(BulletFissionConfig),
                "NormalZombie" => typeof(NormalZombieConfig),
                "Global" => typeof(GlobalConfig),
                "PlayerDataConfig" => typeof(PlayerDataConfig),
                "Laser" => typeof(LaserConfig),
                "LaserFission" => typeof(LaserFissionConfig),
                "ElectroPenetrate" => typeof(ElectroPenetrateConfig),
                _ => throw new System.NotImplementedException()
            };
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
                if(config != null) {
                    Debug.Log(config.GetType() + "创建成功");
                }
                return config;
            }
        }
    }
}
