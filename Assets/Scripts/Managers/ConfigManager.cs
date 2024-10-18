
using System.Collections.Generic;
using Factorys;

using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }
    private readonly List<string> pools = new();
    private Dictionary<string, IConfig> configCache = new();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 如果已经存在，销毁这个实例
        }
    }

    public IConfig GetConfigByClassName(string className)
    {

        if (!configCache.TryGetValue(className, out IConfig config))
        {
            config = ConfigFactory.CreateInjectedConfig(className);
            configCache[className] = config; // 缓存配置
        }
        try {
            CreatePool(className, config);
        }catch {
            
        }
        
        return config;
    }
    private void CreatePool(string configName, IConfig config)
    {
        if(configName.Contains("Global")) {
            return;
        }
        if (pools.IndexOf(configName) == -1)
        {
            pools.Add(configName);
            ObjectPoolManager.Instance.CreatePool(configName + "Pool", config.Prefab, 1, 500);
            
        }
    }

}
