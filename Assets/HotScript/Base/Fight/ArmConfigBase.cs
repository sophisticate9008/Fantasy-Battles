using System;
using System.Collections.Generic;

using MyEnums;
using UnityEngine;
using YooAsset;

[System.Serializable] // 标记为可序列化
public class ArmConfigBase : ConfigBase
{
    public string ChineseOwner => ArmUtil.ArmTypeToArmName(Owner);
    public Dictionary<string, List<Action<GameObject, GameObject>>> typeActions = new(){
        {"enter", new()},
        {"stay", new()},
        {"exit", new()},
        {"return", new()}
    };
    public T CreateInitConfig<T>(bool IsClear = true) where T : ArmConfigBase, new()
    {
        T theNew = new T();
        theNew.Owner = Owner;
        if(IsClear) {
            theNew.ComponentStrs.Clear();
        }
        return theNew;
    }
    public ArmBase TheArm { get; set; }
    public PlayerDataConfig PlayerDataConfig => ConfigManager.Instance.GetConfigByClassName("PlayerData") as PlayerDataConfig;
    private GameObject prefab;

    // 使用私有字段来存储属性值
    [SerializeField] private float critRate;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int level = 1;
    [SerializeField] private float tlc;
    [SerializeField] private float speed;
    [SerializeField] private float rangeFire;
    [SerializeField] private float cd;
    [SerializeField] private float attackCd = 0.5f;
    [SerializeField] private int attackCount = 1;
    [SerializeField] private List<string> componentStrs = new();
    [SerializeField] private float buffDamageTlc = 0.1f;//待定
    [SerializeField] private float selfScale = 1;
    //持续时间
    [SerializeField] private float duration = 2f;
    //力的程度
    [SerializeField] private string owner = "";
    [SerializeField] private string damageType;
    [SerializeField] private string damagePos = "all";
    [SerializeField] private string onType;
    [SerializeField] private float scopeRadius = 3f;
    [SerializeField] private bool isLineCast = false;
    [SerializeField] private float maxForce = 0; // 最大力
    [SerializeField] private float forceBaseDistance = 5; // 影响距离
    [SerializeField] private CdTypes cdType = CdTypes.AtOnce;
    [SerializeField] private ControlBy controlBy = ControlBy.Self;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private float slowTime = 2;
    [SerializeField] private float slowDegree = 0.3f;
    [SerializeField] private float palsyTime = 2;
    [SerializeField] private float dizzyTime = 2;
    [SerializeField] private float fireTime = 3;//点燃
    [SerializeField] private float firePercentage = 0;//点燃附带的百分比伤害
    public virtual float FirePercentage { get => firePercentage; set { firePercentage = value; } }
    public virtual float FreezeTime { get => freezeTime; set => freezeTime = value; }
    public virtual float SlowTime { get => slowTime; set => slowTime = value; }
    public virtual float SlowDegree { get => slowDegree; set => slowDegree = value; }
    public virtual float PalsyTime { get => palsyTime; set => palsyTime = value; }
    public virtual float DizzyTime { get => dizzyTime; set => dizzyTime = value; }
    public virtual float FireTime { get => fireTime; set => fireTime = value; }
    public virtual float EasyHurtTime{get;set;} = 5;
    public virtual float EasyHurtDegree{get;set;} = 0;
    public virtual float CrushProb{get;set;} = 0;
    public float fireTlc = 0.1f;
    public int harmCount = 1;//每次触发造成的伤害次数
    public float GBHRate = 0;
    public float DizzyProb = 1;
    public float percentage = 0;//正常附加 的百分比
    public virtual float addition {get;set;} = 0;//伤害加成
    // Prefab 属性
    public override GameObject Prefab
    {
        get
        {
            if (prefab == null)
            {
                prefab = YooAssets.LoadAssetSync(GetType().Name.Replace("Config", "")).AssetObject as GameObject;
            }
            return prefab;
        }
    }
    public virtual float SelfScale
    {
        get { return selfScale; }
        set { selfScale = value; }
    }

    public virtual float BuffDamageTlc
    {
        get { return buffDamageTlc; }
        set { buffDamageTlc = value; }
    }
    // 属性通过字段实现
    public virtual float CritRate
    {
        get => critRate;
        set => critRate = value;
    }

    public virtual string Name
    {
        get => name;
        set => name = value;
    }

    public virtual string Description
    {
        get => description;
        set => description = value;
    }

    public virtual int Level
    {
        get => level;
        set => level = value;
    }

    public virtual float Tlc
    {
        get => tlc;
        set => tlc = value;
    }

    public virtual float Speed
    {
        get => speed;
        set => speed = value;
    }

    public virtual float RangeFire
    {
        get => rangeFire;
        set => rangeFire = value;
    }

    public virtual float Cd
    {
        get => cd;
        set => cd = value;
    }

    public virtual float AttackCd
    {
        get => attackCd;
        set => attackCd = value;
    }

    public virtual int AttackCount
    {
        get => attackCount;
        set => attackCount = value;
    }

    public virtual List<string> ComponentStrs
    {
        get => componentStrs;
        set => componentStrs = value;
    }
    //技能最大持续时间
    public virtual float Duration { get => duration; set => duration = value; }
    public virtual float RestDuration { get; set; } = 0;

    public virtual string Owner
    {
        get => owner;
        set => owner = value;
    }

    public virtual string DamageType
    {
        get => damageType;
        set => damageType = value;
    }

    public virtual string DamagePos
    {
        get => damagePos;
        set => damagePos = value;
    }

    public virtual string OnType
    {
        get => onType;
        set => onType = value;
    }



    public virtual float ScopeRadius
    {
        get => scopeRadius;
        set => scopeRadius = value;
    }

    public virtual bool IsLineCast
    {
        get => isLineCast;
        set => isLineCast = value;
    }

    public virtual int CurrentAttackedNum { get; set; } = 0;

    public virtual float CurrentCd { get; set; } = 0;

    public virtual float MaxForce
    {
        get => maxForce;
        set => maxForce = value;
    }

    public virtual float ForceBaseDistance
    {
        get => forceBaseDistance;
        set => forceBaseDistance = value;
    }

    // 计算最大影响距离

    public CdTypes CdType { get => cdType; set => cdType = value; }
    public ControlBy ControlBy { get => controlBy; set => controlBy = value; }
    // 构造函数
    public ArmConfigBase()
    {
        Init();
    }

    // 初始化方法，允许子类重写
    public virtual void Init()
    {
        AutoGetOwner();
        InjectData();
        // 初始化逻辑可以在子类中进行扩展
    }
    public virtual void AutoGetOwner()
    {
        foreach (var item in SkillUtil.armTypeDict)
        {
            if (GetType().Name.Contains(item.Value))
            {
                Owner = item.Value;
            }
        }
    }
    public virtual void InjectData()
    {
        if (GetType().Name.Replace("Config", "") == owner)
        {
            int level = (int)PlayerDataConfig.GetValue(ArmUtil.ArmTypeToLevelFieldName(owner));
            ArmPropBase armProp = new(level, owner);
            Cd = armProp.cd;
            RangeFire = armProp.rangeFire;
            Tlc = armProp.tlc;
            DamagePos = armProp.damagePos;
            DamageType = armProp.damageType;
            Duration = armProp.duration;
            AttackCd = armProp.attackCd;
            if (CommonUtil.IsImplementsInterface<IPenetrable>(GetType()))
            {
                IPenetrable config = this as IPenetrable;
                config.PenetrationLevel = armProp.penetration;
                Duration = 20f;
            }
        }
    }
}

