
using System.Collections;
using System.Collections.Generic;
using Factorys;
using UnityEngine;
namespace MyBase
{


    public class ArmBase : MonoBehaviour, IArms
    {

        private float lastFireTime = -10000f;
        public GameObject TargetEnemy { get; set; }


        public ArmConfigBase Config => ConfigManager.Instance.GetConfigByClassName(GetType().Name.Replace("Arm", "")) as ArmConfigBase;

        public void FindTargetNearestOrElite()
        {
            EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (EnemyBase enemy in enemies)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance && distanceToEnemy <= Config.RangeFire)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy.gameObject;
                }
            }

            if (nearestEnemy != null)
            {
                TargetEnemy = nearestEnemy;
            }
            else
            {
                TargetEnemy = null;
            }
        }
        protected virtual void Start()
        {
        }
        public virtual void Update()
        {
            AttackLogic();
        }
        public virtual void AttackLogic()
        {
            if (TargetEnemy == null)
            {
                FindTargetNearestOrElite();
            }

            if (TargetEnemy != null && Time.time - lastFireTime > Config.Cd)
            {
                lastFireTime = Time.time + 100000;//设为较大值，避免再次进入
                StartCoroutine(AttackSequence()); // 发射
            }
        }

        public virtual IEnumerator AttackSequence()
        {

            for (int i = 0; i < Config.AttackCount; i++)
            {
                if (i == 0)
                {
                    FisrtFindTarget();
                }
                else
                {
                    OtherFindTarget();
                }
                lastFireTime = Time.time;
                if (TargetEnemy != null)
                {
                    Attack();
                }


                yield return new WaitForSeconds(Config.AttackCd);

            }
            TargetEnemy = null;
        }
        public virtual void Attack()
        {
            throw new System.NotImplementedException();
        }
        public virtual List<GameObject> FindRandomTarget(int count = 1)
        {
            EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
            List<GameObject> selectedEnemies = new();
            int length = enemies.Length;
            for (int i = 0; i < count; i++)
            {
                int _ = Random.Range(0, length);

                selectedEnemies.Add(enemies[_].gameObject);
            }
            if (count == 1)
            {
                TargetEnemy = selectedEnemies[0];
            }
            return selectedEnemies;
        }

        public virtual void FisrtFindTarget()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OtherFindTarget()
        {
            throw new System.NotImplementedException();
        }
        public virtual ArmChildBase GetOneFromPool()
        {
            ArmChildBase obj = ObjectPoolManager.Instance.GetFromPool(GetType().Name.Replace("Arm", "") + "Pool", Config.Prefab).GetComponent<ArmChildBase>();
            return obj;
        }
    }
}