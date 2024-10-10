using System;
using System.Collections.Generic;
using System.Reflection;
using MyBase;

[Serializable]
public class PlayerDataConfig : ConfigBase
{

    public int diamond = 25000;
    public int keyPurple = 100;
    public int keyBlue = 200;
    public int guaranteeBlue = 10;//蓝色保底
    public int guaranteePurple = 10;//紫色保底
    public List<JewelBase> place1 = new();
    public List<JewelBase> place2 = new();
    public List<JewelBase> place3 = new();
    public List<JewelBase> place4 = new();
    public List<JewelBase> place5 = new();
    public List<JewelBase> place6 = new();
    public List<ItemBase> items = new();
    public List<JewelBase> jewels = new();
    // 事件，用于通知外部某个字段已更新
    public event Action<string> OnDataChanged;

    // 更新字段的通用方法
    public void UpdateValue(string fieldName, Object newValue)
    {
        // 通过反射获取字段
        FieldInfo field = GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        if (field != null)
        {
            // 获取字段当前的值
            Object currentValue = field.GetValue(this); // 这里使用 this
            if (currentValue != newValue)
            {
                // 设置新的值
                field.SetValue(this, newValue); // 这里也使用 this
                OnDataChanged?.Invoke(fieldName); // 触发事件
            }

        }
    }

    // 获取字段的值
    public Object GetValue(string fieldName)
    {
        FieldInfo field = GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        return field != null ? field.GetValue(this) : 0; // 这里使用 this
    }
    public void UpdateValueAdd(string fieldName, int val)
    {
        int orginValue = (int)GetValue(fieldName);
        UpdateValue(fieldName, orginValue + val);
    }
    public void UpdateValueSubtract(string fieldName, int val)
    {
        int orginValue = (int)GetValue(fieldName);
        UpdateValue(fieldName, orginValue - val);
    }
}
