
using MyBase;
using UnityEngine;

namespace ArmConfigs
{
    public class LaserConfig : ArmConfigBase
    {
        [SerializeField] private bool isFlame;
        public virtual bool IsFlame { get => isFlame; set => isFlame = value; }
        public string FindType { get; set; } = "scope";
        public virtual bool IsMainDamageUp {get;set;} = false; 
        public override void Init()
        {
            base.Init();
            Tlc = 0.5f;
            Name = "Laser";
            Description = "激光";
            RangeFire = 10;
            Cd = 4f;
            AttackCd = 0.2f;
            AttackCount = 1;
            Duration = 3f;
            TriggerType = "stay";
            DamageType = "energy";
            CritRate = 0.5f;
            ScopeRadius = 12f;
            IsLineCast = true;

        }
    }
}