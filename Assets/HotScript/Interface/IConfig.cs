using UnityEngine;

public interface IConfig{
    void SaveConfig();
    GameObject Prefab { get;set; }
    bool IsCreatePool { get;set; }
}