using System;
using System.Collections.Generic;
using System.Reflection;


[Serializable]
public class PlayerDataConfig : ConfigBase,IFlagInjectFromFile
{
    public override bool IsCreatePool { get; set; } = false;
    public List<MissionRecord> PassRecords = new();
    public int money = 200000;
    public int diamond = 25000;
    public int keyPurple = 100;
    public int keyBlue = 200;
    public int guaranteeBlue = 10;//蓝色保底
    public int guaranteePurple = 10;//紫色保底
    public int washWater = 120;
    public List<JewelBase> place1 = new();
    public List<JewelBase> place2 = new();
    public List<JewelBase> place3 = new();
    public List<JewelBase> place4 = new();
    public List<JewelBase> place5 = new();
    public List<JewelBase> place6 = new();
    public List<ItemBase> items = new();
    public List<JewelBase> jewels = new();

    #region  装备或技能等级
    public int levelPlace1 = 1;
    public int levelPlace2 = 1;
    public int levelPlace3 = 1;
    public int levelPlace4 = 1;
    public int levelPlace5 = 1;
    public int levelPlace6 = 1;
    public int levelArm0 = 1;
    public int levelArm1 = 1;
    public int levelArm2 = 1;
    public int levelArm3 = 1;
    public int levelArm4 = 1;
    public int levelArm5 = 1;
    public int levelArm6 = 1;
    public int levelArm7 = 1;
    public int levelArm8 = 1;
    public int levelArm9 = 1;
    public int levelArm10 = 1;
    public int levelArm11 = 1;
    public int levelArm12 = 1;
    public int levelArm13 = 1;

    
    #endregion


    //该字段用来手动通知宝石变动更新
    public int jewelChange = 0;
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

                field.SetValue(this, newValue); // 这里也使用 this
                OnDataChanged?.Invoke(fieldName); // 触发事件
                SaveConfig();
            }

        }
    }

    // 获取字段的值
    public Object GetValue(string fieldName)
    {
        FieldInfo field = GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        return field?.GetValue(this); // 这里使用 this
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
