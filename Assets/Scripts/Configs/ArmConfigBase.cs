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
    [SerializeField] private int level;
    [SerializeField] private float tlc;
    [SerializeField] private float speed;
    [SerializeField] private int rangeFire;
    [SerializeField] private float cd;
    [SerializeField] private float attackCd;
    [SerializeField] private int attackCount;
    [SerializeField] private List<string> componentStrs = new List<string>();
    [SerializeField] private float buffDamageTlc;
    [SerializeField] private float selfScale = 1;
    //持续时间
    [SerializeField] private float duration = 20f;

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
    //技能最大持续时间
    public virtual float Duration { get => duration; set => duration = value; }
    //技能当前剩余时间,因为持续时间的武器都是同一时间同时释放,所以全局配置即可
    public virtual float RestDuration{get; set;} = 0;
    public virtual string Owner { get; set; }
    //伤害类型 
    public virtual string DamageType { get; set; }

    //伤害位置 all / land
    public virtual string DamagePos { get; set; } = "all";
    // 触发类型
    public virtual string OnType { get; set; }

    public virtual string DamageExtraType { get ; set ; } = "";
    //自身索敌半径
    public virtual float ScopeRadius { get; set; } = 3f;
    //线段路径伤害
    public virtual bool IsLineCast { get; set; } = false;
    //射线路径伤害
    public virtual bool IsRayCast { get; set; } = false;
    public virtual int CurrentAttackedNum {get;set;} = 0;
    public virtual float CurrentCd { get; set; } = 0;

    public CdTypes CdType { get; set; } = CdTypes.AtOnce;
    public ControlBy ControlBy{ get; set; } = ControlBy.Self;
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

