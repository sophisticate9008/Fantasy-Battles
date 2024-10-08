using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MyBase
{
    [System.Serializable] // 标记为可序列化
    public class ArmConfigBase: ConfigBase
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
        [SerializeField] private float lastTime;
        [SerializeField] private List<string> componentStrs = new List<string>();
        [SerializeField] private float buffDamageTlc;

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
        public virtual float BuffDamageTlc {
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

        public virtual float LastTime
        {
            get => lastTime;
            set => lastTime = value;
        }

        public virtual List<string> ComponentStrs
        {
            get => componentStrs;
            set => componentStrs = value;
        }
        public virtual float Duration {get;set;} = 20f;
        public virtual string Owner {get; set;}
        //伤害类型 
        public virtual string DamageType {get; set;}

        //伤害位置 all / land
        public virtual string DamagePos {get; set;} = "all";
        // 触发类型
        public virtual string TriggerType{get; set;}

        public virtual string DamageExtraType {get => ""; set{}}
        //自身索敌半径
        public virtual float ScopeRadius {get; set;} = 3f;
        public virtual bool IsLineCast {get;set;} = false;
        public virtual bool IsRayCast {get;set;} = false;
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
}
