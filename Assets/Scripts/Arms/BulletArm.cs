using System.Collections;
using ArmConfigs;
using Factorys;
using MyBase;
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
                ShootMultipleBullets(); // 多条弹道发射
                yield return new WaitForSeconds(ConcreteConfig.RepeatCd); // 每次连发之间的间隔
            }
        }

        private void ShootMultipleBullets()
        {
            if (TargetEnemy == null) return;

            // 计算从枪口指向敌人的方向向量
            Vector3 baseDirection = (TargetEnemy.transform.position - transform.position).normalized;
            // 发射 MultipleLevel 数量的子弹
            var objs = IMultipleable.MutiInstantiate(Config.Prefab, transform.position, baseDirection);
            IMultipleable.InitObjs(objs);
        }

    }
}