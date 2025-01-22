using System;
using System.IO;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class ConfigBase
{
    public virtual GameObject Prefab { get; set; }
    public virtual bool IsCreatePool { get; set; } = true;

    public void SaveConfig()
    {
        Debug.Log(GetType().Name + "已保存");
        string json = JsonUtility.ToJson(this, true);
        string path = Constant.ConfigsPath;
        // 确保目录存在
        Directory.CreateDirectory(path);
        string fileName = $"{GetType().Name}.json";
        File.WriteAllText(Path.Combine(path, fileName), json);
    }
    public virtual object Clone()
    {
        // 创建当前对象类型的实例
        object copy = Activator.CreateInstance(this.GetType());

        // 获取所有公共的属性和字段
        foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // 仅复制可写的属性
            if (property.CanWrite)
            {
                object value = property.GetValue(this);
                if (value is ICloneable cloneable)
                {
                    // 如果属性实现了ICloneable接口，则调用它的Clone方法
                    value = cloneable.Clone();
                }
                property.SetValue(copy, value);
            }
        }

        foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            object value = field.GetValue(this);
            if (value is ICloneable cloneable)
            {
                // 如果字段实现了ICloneable接口，则调用它的Clone方法
                value = cloneable.Clone();
            }
            field.SetValue(copy, value);
        }

        return copy;
    }
}

