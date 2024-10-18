using System;
using ArmsChild;
using ArmConfigs;
using MyBase;
using R3;
using UnityEngine;
namespace MyComponents
{
    public class PenetrableComponent : ComponentBase, IPenetrable
    {

        private readonly ReactiveProperty<int> _penetrationLevel = new();
        public PenetrableComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
        {

        }
        public override void Init()
        {
            base.Init();
            PenetrationLevel = (ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig).AllPenetrationLevel;
            if (SelfObj.GetComponent<Bullet>() != null)
            {
                PenetrationLevel += (ConfigManager.Instance.GetConfigByClassName("Bullet") as BulletConfig).BulletPenetrationLevel;
            }
        }
        public int PenetrationLevel
        {
            get => _penetrationLevel.Value;
            set
            {
                _penetrationLevel.Value = value;
            }
        }
        public void HandleDestruction()
        {
            SelfObj.GetComponent<ArmChildBase>().ReturnToPool();
        }

        public override void Exec(GameObject enemyObj)
        {
            // PenetrationLevel -= enemyObj.GetComponent<EnemyBase>().Config.blocks;
            if (PenetrationLevel <= 0)
            {
                HandleDestruction();
            }
        }
    }

}
