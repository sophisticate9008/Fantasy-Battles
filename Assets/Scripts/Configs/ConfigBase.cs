using System.IO;
using UnityEngine;

namespace MyBase
{
    public class ConfigBase : IConfig
    {
        public virtual GameObject Prefab { get;set;}

        public void SaveConfig()
        {
            string json = JsonUtility.ToJson(this, true);
            string path = Constant.ConfigsPath;
            // 确保目录存在
            Directory.CreateDirectory(path);
            string fileName = $"{GetType().Name}.json";
            File.WriteAllText(Path.Combine(path, fileName), json);
        }
    }
}
