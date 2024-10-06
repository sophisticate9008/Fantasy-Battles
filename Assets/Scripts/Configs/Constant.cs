using System.IO;
using UnityEngine;

public class Constant {
    public static readonly string ConfigsPath =  Path.Combine(Application.persistentDataPath, "Configs");
    public static readonly string DataPath = Path.Combine(Application.persistentDataPath, "Data");
    public static readonly string SelfPrefabResPath = "Prefabs/Self/";
    public static readonly string EnemyPrefabResPath = "Prefabs/Enemy/";
    public static readonly string PrefabResOther = "Prefabs/Other/";
}