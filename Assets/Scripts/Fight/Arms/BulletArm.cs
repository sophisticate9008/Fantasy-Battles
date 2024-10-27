using System.Collections;
using ArmConfigs;
using Factorys;
using FightBases;
using UnityEngine;
using YooAsset;
namespace Arms
{
    public class BulletArm : ArmBase, IRepeatable
    {


        public BulletConfig ConcreteConfig => Config as BulletConfig;
        //锁定的敌人

        public int RepeatLevel { get; set; }

        protected override void Start()
        {
            base.Start();
            RepeatLevel = ConcreteConfig.RepeatLevel;

        }

        // [Inject]
        // public void Inject(Bullet prefab)
        // {
        //     Debug.Log("Inject Bullet");
        //     this.prefab = prefab;
        // }

        public override void FisrtFindTarget()
        {
            FindTargetNearestOrElite();
        }
        public override void OtherFindTarget()
        {
            FindTargetNearestOrElite();
            
        }
        public override void Attack()
        {
            StartCoroutine(ShootAtTarget());
        }
        // 发射子弹
        private IEnumerator ShootAtTarget()
        {
            for (int i = 0; i < RepeatLevel; i++) // 连发逻辑
            {
                AttackMultipleOnce(); // 多条弹道发射
                yield return new WaitForSeconds(ConcreteConfig.RepeatCd); // 每次连发之间的间隔
            }
        }


    }
}