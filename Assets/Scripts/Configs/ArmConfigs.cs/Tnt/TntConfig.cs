using FightBases;
using UnityEngine;

namespace ArmConfigs
{
    public class TntConfig : ArmConfigBase, IPenetrable, IBoomable
    {
        public int PenetrationLevel { get; set; } = 2;
        public ArmConfigBase BoomChildConfig => ConfigManager.Instance.GetConfigByClassName("TntBoom") as TntBoomConfig;
        
        public override void Init()
        {
            
            base.Init();
            RangeFire = 8;
            Speed = 15;
            OnType = "enter";
            DamageType = "fire";
            DamagePos = "all";
            Cd = 3;
            ScopeRadius = 10f;
            Tlc = 1;
            ComponentStrs.Add("穿透");
            ComponentStrs.Add("爆炸");
        
            AttackCount = 5;
            AttackCd = 0.5f;
        }
    }
}