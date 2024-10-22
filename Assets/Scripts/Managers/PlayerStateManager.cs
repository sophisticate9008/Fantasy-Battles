using System.Collections.Generic;
using System.IO;
using ArmConfigs;
using Factorys;
using FightBases;
using UnityEngine;

//管理玩家选择，buff等
public class PlayerStateManager : MonoBehaviour
{

    // 静态实例
    public static PlayerStateManager Instance { get; private set; }
    public GlobalConfig globalConfig => ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
    // Unity Awake 方法，确保在所有其他组件之前初始化
    private void Awake()
    {

        // 检查是否已经有实例存在
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 防止创建多个实例
        }
        else
        {
            Instance = this;
        }
    }

    // 其他管理逻辑可以在此添加
    void Start()
    {
    }

}