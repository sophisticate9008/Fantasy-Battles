using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyBase
{
    [System.Serializable]
    public class EnemyConfigBase : ConfigBase
    {
        // 私有字段
        private GameObject prefab;
        // Prefab 属性，允许重写
        public override GameObject Prefab
        {
            get
            {
                if (prefab == null)
                {
                    prefab = Resources.Load<GameObject>(Constant.EnemyPrefabResPath + GetType().Name.Replace("Config", ""));
                    return prefab;
                }
                else
                {
                    return prefab;
                }
            }
        }
        [SerializeField] private int life;
        [SerializeField] private float speed;
        [SerializeField] private int damage;
        //免疫数
        [SerializeField] private int immunityCount;
        // 格挡数
        [SerializeField] private int blocks;
        [SerializeField] private float rangeFire;
        [SerializeField] private float atkSpeed;
        [SerializeField] private float weight;
        [SerializeField] private float derateAd;
        [SerializeField] private float derateIce;
        [SerializeField] private float derateFire;
        [SerializeField] private float derateElec;
        [SerializeField] private float derateWind;
        [SerializeField] private float derateEnergy;
        [SerializeField] private float deratePenetrate;
        [SerializeField] private List<string> controlImmunityList = new();
        [SerializeField] private List<string> damageTypeImmunityList = new();
        [SerializeField] private string attackType;
        [SerializeField] private string actionType;
        [SerializeField] private string characterType;
        [SerializeField] private int attackCount;


        // 公共属性，允许重写
        public int Life
        {
            get => life;
            set => life = value;
        }
        public virtual List<string> DamageTypeImmunityList
        {
            get { return damageTypeImmunityList; }
            set { damageTypeImmunityList = value; }
        }
        public virtual float Speed
        {
            get => speed;
            set => speed = value;
        }

        public virtual int Damage
        {
            get => damage;
            set => damage = value;
        }

        public virtual int ImmunityCount
        {
            get => immunityCount;
            set => immunityCount = value;
        }

        public virtual int Blocks
        {
            get => blocks;
            set => blocks = value;
        }

        public virtual float RangeFire
        {
            get => rangeFire;
            set => rangeFire = value;
        }

        public virtual float AtkSpeed
        {
            get => atkSpeed;
            set => atkSpeed = value;
        }

        public virtual float Weight
        {
            get => weight;
            set => weight = value;
        }


        public virtual float DerateAd
        {
            get => derateAd;
            set => derateAd = value;
        }

        public virtual float DerateIce
        {
            get => derateIce;
            set => derateIce = value;
        }

        public virtual float DerateFire
        {
            get => derateFire;
            set => derateFire = value;
        }

        public virtual float DerateElec
        {
            get => derateElec;
            set => derateElec = value;
        }

        public virtual float DerateWind
        {
            get => derateWind;
            set => derateWind = value;
        }

        public virtual float DerateEnergy
        {
            get => derateEnergy;
            set => derateEnergy = value;
        }

        public virtual List<string> ControlImmunityList
        {
            get => controlImmunityList;
            set => controlImmunityList = value ?? new List<string>();
        }

        public virtual string AttackType
        {
            get => attackType;
            set => attackType = value;
        }

        public virtual string ActionType
        {
            get => actionType;
            set => actionType = value;
        }

        public virtual string CharacterType
        {
            get => characterType;
            set => characterType = value;
        }

        public virtual int AttackCount
        {
            get => attackCount;
            set => attackCount = value;
        }
        public virtual float DeratePenetrate
        {
            get => deratePenetrate;
            set => deratePenetrate = value;
        }

        // 获取伤害减免的字典
        public virtual Dictionary<string, float> GetDamageReduction()
        {
            return new Dictionary<string, float>
            {
                { "ad", DerateAd },
                { "ice", DerateIce },
                { "fire", DerateFire },
                { "electric", DerateElec },
                { "wind", DerateWind },
                { "energy", DerateEnergy }, 
                { "penetrate", DeratePenetrate }
            };
        }

        // 构造函数
        public EnemyConfigBase()
        {
            Init();
        }

        // 初始化方法，允许子类重写
        public virtual void Init()
        {
            // 在子类中扩展初始化逻辑
        }

    }
}
