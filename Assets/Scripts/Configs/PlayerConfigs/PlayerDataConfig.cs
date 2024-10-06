using System;
using System.Collections.Generic;
using System.Reflection;
using MyBase;

[Serializable]
public class PlayerDataConfig : ConfigBase
{
    public List<ItemBase> items = new();
    public int diamondCount = 25000;
    public int keyPurpleCount = 100;
    public int keyblueCount = 100;

    // 事件，用于通知外部某个字段已更新
    public event Action<string> OnDataChanged;

    // 更新字段的通用方法
    public void UpdateValue(string fieldName, int newValue)
    {
        // 通过反射获取字段
        FieldInfo field = GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        if (field != null)
        {
            // 获取字段当前的值
            int currentValue = (int)field.GetValue(this); // 这里使用 this
            if (currentValue != newValue)
            {
                // 设置新的值
                field.SetValue(this, newValue); // 这里也使用 this
                OnDataChanged?.Invoke(fieldName); // 触发事件
            }
        }
    }

    // 获取字段的值
    public int GetValue(string fieldName)
    {
        FieldInfo field = GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        return field != null ? (int)field.GetValue(this) : 0; // 这里使用 this
    }
}
