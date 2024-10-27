using System.Collections.Generic;
using System.IO;
using MyEnums;
using UnityEngine;


[System.Serializable] // 标记为可序列化
public class ArmConfigBase : ConfigBase
{
    private GameObject prefab;

    // 使用私有字段来存储属性值
    [SerializeField] private float critRate;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int level = 1;
    [SerializeField] private float tlc;
    [SerializeField] private float speed;
    [SerializeField] private int rangeFire;
    [SerializeField] private float cd;
    [SerializeField] private float attackCd;
    [SerializeField] private int attackCount = 1;
    [SerializeField] private List<string> componentStrs = new();
    [SerializeField] private float buffDamageTlc;//待定
    [SerializeField] private float selfScale = 1;
    //持续时间
    [SerializeField] private float duration = 20f;
    //力的程度
    [SerializeField] private float forceDegree = 1f;
    [SerializeField] private string owner;
    [SerializeField] private string damageType;
    [SerializeField] private string damagePos = "all";
    [SerializeField] private string onType;
    [SerializeField] private string damageExtraType = "";
    [SerializeField] private float scopeRadius = 3f;
    [SerializeField] private bool isLineCast = false;
    [SerializeField] private bool isRayCast = false;
    [SerializeField] private float maxForce = 10; // 最大力
    [SerializeField] private float forceBaseDistance = 5; // 影响距离
    [SerializeField] private CdTypes cdType = CdTypes.AtOnce;
    [SerializeField] private ControlBy controlBy = ControlBy.Self;
    [SerializeField] private float freezeTime = 1;
    [SerializeField] private float slowTime = 1;
    [SerializeField] private float slowDegree = 0.3f;
    [SerializeField] private float palsyTime = 1;
    [SerializeField] private float dizzyTime = 1;
    [SerializeField] private float fireTime = 1;//点燃
    
    public virtual float FreezeTime {get => freezeTime; set => freezeTime = value; }
    public virtual float SlowTime{get => slowTime; set => slowTime = value; }
    public virtual float SlowDegree{get => slowDegree; set => slowDegree = value; }
    public virtual float PalsyTime{get => palsyTime; set => palsyTime = value; }
    public virtual float DizzyTime{get => dizzyTime; set => dizzyTime = value; }
    public virtual float FireTime{get => fireTime; set => fireTime = value; }

    // Prefab 属性
    public override GameObject Prefab
    {
        get
        {
            if (prefab == null)
            {
                prefab = Resources.Load<GameObject>(Constant.SelfPrefabResPath + GetType().Name.Replace("Config", ""));
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

    public virtual int RangeFire
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
    public virtual float ForceDegree
    {
        get => forceDegree;
        set => forceDegree = value;
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

    public virtual string DamageExtraType
    {
        get => damageExtraType;
        set => damageExtraType = value;
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

    public virtual bool IsRayCast
    {
        get => isRayCast;
        set => isRayCast = value;
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
        // 初始化逻辑可以在子类中进行扩展
    }

}

