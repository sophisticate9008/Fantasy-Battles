using System.Collections.Generic;
using ArmConfigs;
using MyBase;
using UnityEngine;

namespace ArmsChild
{
    public class Laser : ArmChildBase
    {
        public LaserConfig ConcreteConfig => Config as LaserConfig;
        private GameObject expectEnemy;
        public override void Move()
        {
            GameObject indeedEnemy = AllKindFindTarget();

            if (indeedEnemy == null)
            {
                transform.localScale = new Vector3(0, 1, 1);
                return;
            }
            else
            {
                expectEnemy = indeedEnemy;
            }
            float distance = Vector3.Distance(transform.position, indeedEnemy.transform.position);
            Vector3 direction = indeedEnemy.transform.position - transform.position;
            Direction = direction;
            Vector3 scale = transform.localScale;
            scale.x = distance / 20;
            scale.y = 0.3f;
            transform.localScale = scale;

        }
        public virtual GameObject AllKindFindTarget()
        {
            GameObject indeedEnemy;
            if (TargetEnemyByArm != null)
            {
                indeedEnemy = TargetEnemyByArm;
                if (!TargetEnemyByArm.activeSelf)
                {
                    TargetEnemyByArm = null;
                    indeedEnemy = null;
                }
            }
            else
            {
                if (TargetEnemy == null)
                {

                    FindTargetInScope();
                }
                indeedEnemy = TargetEnemy;
                if (indeedEnemy == null)
                {
                    FindTargetRandom(null);
                }
                indeedEnemy = TargetEnemy;
            }
            return indeedEnemy;
        }
        public override void TriggerByTypeCallBack(string type)
        {
            if (type != Config.TriggerType) { return; }
            LaserFission();
            PathDamage();
            PathFlame();
        }
        public void LaserFission()
        {
            if (GetType().Name == "Laser")
            {
                LaserFissionConfig laserFissionConfig = ConfigManager.Instance.GetConfigByClassName("LaserFission") as LaserFissionConfig;
                List<GameObject> enemys = FindTargetInScope(laserFissionConfig.FissionLevel, expectEnemy);
                if (enemys == null)
                {
                    return;
                }

                foreach (var temp in enemys)
                {
                    ArmChildBase armChildBase = ObjectPoolManager.Instance.GetFromPool("LaserFissionPool", laserFissionConfig.Prefab).GetComponent<ArmChildBase>();
                    armChildBase.gameObject.transform.position = expectEnemy.transform.position;
                    (armChildBase as LaserFission).TargetEnemyByArm = temp;
                    armChildBase.Init();
                }
            }
        }
        public void PathDamage()
        {
            List<GameObject> hitEnemys = LineCastAll(transform.position, expectEnemy.transform.position);
            foreach (var hit in hitEnemys)
            {
                //排除第一个接触的也就是次级产生的敌人或者首次的目标敌人
                if(hit == hitEnemys[0]) {
                    continue;
                }
                if (hit != expectEnemy)
                {
                    Debug.Log("造成了路径伤害");
                    CreateDamage(hit);
                }

            }
        }
        public void PathFlame()
        {
            if (ConcreteConfig.IsFlame)
            {

            }
        }
        public override sealed void CreateDamage(GameObject enemyObj)
        {
            float tlc = Config.Tlc;
            // if(ConcrateConfig.IsMainDamageUp) {
            //     tlc *= 3;
            // }
            CreateDamage(enemyObj, tlc);
        }

    }

}