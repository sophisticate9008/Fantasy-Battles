
using System.Collections.Generic;


using UnityEngine;

public class ConfigManager : ManagerBase<ConfigManager>
{
    private readonly List<string> pools = new();
    private Dictionary<string, ConfigBase> configCache = new();


    public ConfigBase GetConfigByClassName(string className)
    {

        if (!configCache.TryGetValue(className, out ConfigBase config))
        {
            config = ConfigFactory.Create(className);
            configCache[className] = config; // 缓存配置
        }
        try {
            CreatePool(className, config);
        }catch {
            
        }
        
        return config;
    }
    private void CreatePool(string configName, ConfigBase config)
    {
        if(!config.IsCreatePool) {
            return;
        }
        if (pools.IndexOf(configName) == -1)
        {
            pools.Add(configName);
            ObjectPoolManager.Instance.CreatePool(configName + "Pool", config.Prefab, 5, 500);
            
        }
    }

}
