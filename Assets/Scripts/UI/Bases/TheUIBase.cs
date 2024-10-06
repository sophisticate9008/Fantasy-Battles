using UnityEngine;
using YooAsset;

public class TheUIBase : MonoBehaviour, TheUI
{
    public GameObject Prefab
    {
        get { return gameObject;}
        set { }
    }

    public PlayerDataConfig PlayerDataConfig { get => ConfigManager.Instance.GetConfigByClassName("PlayerDataConfig") as PlayerDataConfig;  set{} }

    public virtual void Init()
    {
        
    }
}