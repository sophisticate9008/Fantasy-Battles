
using System;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class GlobalConfig : ConfigBase
{
    public PlayerDataConfig PlayerDataConfig => ConfigManager.Instance.GetConfigByClassName("PlayerData") as PlayerDataConfig;
    // 私有字段
    [SerializeField] private float critRate;
    [SerializeField] private float critDamage;
    [SerializeField] private int attackValue;
    [SerializeField] private int life;
    [SerializeField] private int allPenetrationLevel = 5;
    [SerializeField] private float fireAddition;
    [SerializeField] private float iceAddition;
    [SerializeField] private float elecAddition;
    [SerializeField] private float energyAddition;
    [SerializeField] private float windAddition;
    [SerializeField] private float allAddition;
    [SerializeField] private float boomAddition;
    [SerializeField] private float adAddition;
    public override bool IsCreatePool { get; set; } = false;
    // 构造函数
    public GlobalConfig()
    {

        Init();
    }

    // 初始化方法
    public virtual void Init()
    {
        // 可以在这里添加初始化逻辑
        attackValue = PlayerDataConfig.AttackValue;
        CritRate = 0.05f;
        CritDamage = 0;
    }
    public float TransmitRate { get; set; }
    // 公共属性
    public virtual float CritRate
    {
        get => critRate;
        set => critRate = value;
    }

    public virtual float CritDamage
    {
        get => critDamage;
        set => critDamage = value;
    }

    public virtual int AttackValue
    {
        get => attackValue;
        set => attackValue = value;
    }

    public virtual int Life
    {
        get => life;
        set => life = value;
    }

    public virtual int AllPenetrationLevel
    {
        get => allPenetrationLevel;
        set => allPenetrationLevel = value;
    }

    public virtual float FireAddition
    {
        get => fireAddition;
        set => fireAddition = value;
    }

    public virtual float IceAddition
    {
        get => iceAddition;
        set => iceAddition = value;
    }

    public virtual float ElecAddition
    {
        get => elecAddition;
        set => elecAddition = value;
    }

    public virtual float EnergyAddition
    {
        get => energyAddition;
        set => energyAddition = value;
    }

    public virtual float WindAddition
    {
        get => windAddition;
        set => windAddition = value;
    }

    public virtual float AllAddition
    {
        get => allAddition;
        set => allAddition = value;
    }

    public virtual float BoomAddition
    {
        get => boomAddition;
        set => boomAddition = value;
    }

    public virtual float AdAddition
    {
        get => adAddition;
        set => adAddition = value;
    }
    public virtual float RandomAdditonMin { get; set; } = 0;
    public virtual float RandomAdditonMax { get; set; } = 0;
    public virtual float AdditionToEliteOrBoss { get; set; } = 0;

    public virtual float[] CritWithPercentageAndMax { get; set; } = new float[2] { 0, 0 };
    public virtual float[] DamageWithPercentageAndMax { get; set; } = new float[2] { 0, 0 };

    public virtual float EnemyBuffTimeAddition { get; set; } = 1;
    public int freezenHurtMaxLevel = 0;
    // 获取伤害加成的字典
    public virtual Dictionary<string, float> GetDamageAddition()
    {
        return new Dictionary<string, float>
        {
            { "fire", FireAddition },
            { "ice", IceAddition },
            { "elec", ElecAddition },
            { "energy", EnergyAddition },
            { "wind", WindAddition },
            { "boom", BoomAddition },
            { "ad", AdAddition }
        };
    }
    public void LoadJewel()
    {
        for (int i = 1; i <= 6; i++)
        {
            var jewels = PlayerDataConfig.GetValue("place" + i) as List<JewelBase>;
            foreach (var jewel in jewels)
            {
                ItemUtil.CreateJewelAction(jewel.id, jewel.level).Invoke();
            }
        }
    }
    public string MergeJewelDes()
    {
        string text = "";
        List<string> allDes = new();
        for (int i = 1; i <= 6; i++)
        {
            var jewels = PlayerDataConfig.GetValue("place" + i) as List<JewelBase>;
            foreach (var jewel in jewels)
            {
                allDes.Add(ItemUtil.IdLevelToJewelDesc(jewel.id, jewel.level));
            }
        }

        var merger = new AffixMerger();
        var merged = merger.MergeAffixes(allDes);
        foreach(var des in merged) {
            text += des.ToString() + "\n";
        }
        return text;
    }
}
