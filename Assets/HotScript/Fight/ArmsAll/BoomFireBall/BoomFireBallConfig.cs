using FightBases;
using UnityEngine;

namespace ArmConfigs
{
    public class BoomFireBallConfig : ArmConfigBase, IPenetrable, IBoomable,IHoldable
    {
        public int PenetrationLevel { get; set; } = 2;
        public ArmConfigBase BoomChildConfig => ConfigManager.Instance.GetConfigByClassName("BoomFireBallBoom") as BoomFireBallBoomConfig;

        public ArmConfigBase HoldChildConfig => ConfigManager.Instance.GetConfigByClassName("BoomFireBallHold") as BoomFireBallHoldConfig;

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
            ComponentStrs.Add("Hold");
            AttackCount = 5;
            AttackCd = 0.5f;
        }
    }
}