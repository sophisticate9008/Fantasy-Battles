
using System.Collections;
using System.Collections.Generic;
using Factorys;
using Unity.VisualScripting;
using UnityEngine;

namespace FightBases
{
    public class EnemyBase : MonoBehaviour, IEnemy
    {
        public Vector2 minBoundary; // 设定左下角的边界
        public Vector2 maxBoundary; // 设定右上角的边界
        public AnimatorManager animatorManager;
        public Animator animator;
        public bool CanAction { get; set; } = true;
        public EnemyConfigBase ConstConfig => ConfigManager.Instance.GetConfigByClassName(GetType().Name) as EnemyConfigBase;
        public EnemyConfigBase Config { get; set; }
        public GlobalConfig GlobalConfig => ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
        public bool IsInit { get; set; }
        public Queue<IBuff> Buffs { get; } = new();
        public Dictionary<string, IComponent> InstalledComponents { get; } = new();
        public float ControlEndTime { get; set; } = 0f;
        public Dictionary<string, float> BuffEndTimes { get; set; } = new();
        //硬控总结束时间
        public float HardControlEndTime { get; set; }

        public int ImmunityCount { get; set; }
        public int MaxLife { get; set; }
        public int NowLife { get; set; }
        public float EasyHurt { get; set; }
        public bool isDead;
        public virtual void Init()
        {
            minBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            maxBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

            Config = ConstConfig.Clone() as EnemyConfigBase;
            isDead = false;
            NowLife = Config.Life;
            MaxLife = Config.Life;
            ImmunityCount = Config.ImmunityCount;
            TransmitBack(y: 0, returnSpawn: true);
            CanAction = true;
            IsInit = true;
            try
            {
                animatorManager.SetAnimParameter(animator, "isRunning", true);
                animatorManager.PlayAnim(animator, 1f);
            }
            catch { }


        }
        public virtual void TransmitBack(float y, bool returnSpawn = false)
        {
            if (returnSpawn)
            {
                // 获取主摄像机
                Camera mainCamera = Camera.main;

                if (mainCamera != null)
                {
                    // 获取物体当前的视口位置
                    Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

                    // 修改 y 为 1，使物体回到视口顶部
                    viewportPos.y = 1.0f;

                    // 将视口位置转换回世界坐标
                    Vector3 worldPos = mainCamera.ViewportToWorldPoint(viewportPos);

                    // 更新物体的位置
                    Vector3 newPosition = transform.position;
                    newPosition.y = worldPos.y;  // 将物体的 y 坐标设为视口顶部
                    StartCoroutine(TransmitBackByStep(0.5f,newPosition));
                }
            }
            else
            {
                // 正常情况下，只减少 y 坐标
                Vector3 newPosition = transform.position;
                newPosition.y += y;
                StartCoroutine(TransmitBackByStep(0.5f,newPosition));
            }
        }

        private IEnumerator TransmitBackByStep(float t,Vector3 targetPosition) {
            float elapsed = 0;
            float totalDistance = Vector3.Distance(transform.position, targetPosition);
            float speed = totalDistance / t; 
            float step = speed * Time.deltaTime;
            while(elapsed < t) {
                elapsed += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                yield return null;
            }
        }
        protected virtual void Start()
        {
            animatorManager = AnimatorManager.Instance;
            animator = GetComponent<Animator>();
            Init();
        }

        public void FixedUpdate()
        {
            if (!CanAction && !isDead)
            {
                animatorManager.StopAnim(animator);
            }
            // else
            // {
            //     animatorManager.PlayAnim(animator, 1f);
            // }
        }
        public void Update()
        {
            if (!IsInit)
            {
                return;
            }
            BuffEffect();
            if (CanAction)
            {
                Move();
            }
            else
            {
                //防止休眠
                PreventSleep();
            }
        }


        public virtual void Move()
        {

            Vector3 position = transform.position;
            float bottomEdge = -Camera.main.orthographicSize;
            if (position.y > bottomEdge + Config.RangeFire)
            {
                transform.Translate(Config.Speed * Time.deltaTime * Vector3.down);
            }
            else
            {
                PreventSleep();
            }
            //限定在盒子内
            ClampMonsterPosition(transform);

        }
        private void PreventSleep()
        {
            Vector3 originalPosition = transform.position;

            // 施加微小的移动
            transform.position = originalPosition + new Vector3(0.001f, 0, 0); // 在X轴上施加微小的移动

            // 立即恢复到原始位置
            transform.position = originalPosition;
        }
        public virtual void Attack()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Skill()
        {
            throw new System.NotImplementedException();
        }

        public virtual void CalLife(int damage)
        {

            NowLife -= damage;
            if (NowLife <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            if (!isDead)
            {
                isDead = true;
                OnByType("die", gameObject);
                FighteManager.Instance.AddExp(1);
                animatorManager.PlayAnimWithCallback(animator, "Die", () => ReturnToPool());

            }


            // animatorManager.SetAnimParameter(animator, "isDead", true);

        }

        void OnByType(string type, GameObject obj)
        {
            foreach (var component in InstalledComponents)
            {
                foreach (var _ in component.Value.Type)
                {
                    if (_ == type)
                    {
                        component.Value.Exec(obj);
                    }
                }
            }
        }

        public void BuffEffect()
        {
            while (Buffs.Count > 0)
            {
                var buff = Buffs.Dequeue();
                buff.EffectAndAutoRemove();
            }
        }

        public void ReturnToPool()
        {
            //存活数量-1
            EnemyManager.Instance.liveCount--;
            //移除所有buff
            ObjectPoolManager.Instance.ReturnToPool(GetType().Name + "Pool", gameObject);
            //移除子特效


        }

        public void RelaseExp()
        {

        }

        public void AddBuff(string buffName, GameObject selfObj, float duration, params object[] args)
        {
            //全局异常状态增强
            duration *= GlobalConfig.EnemyBuffTimeAddition;

            //免疫指定控制buff
            if (Config.ControlImmunityList.IndexOf(buffName) != -1)
            {
                return;
            }
            //伤害位置对不上不能上buff
            if (Config.ActionType == "land" && selfObj.GetComponent<ArmChildBase>().Config.DamageType != "all")
            {
                return;
            }
            if (!BuffEndTimes.ContainsKey(buffName))
            {
                Buffs.Enqueue(BuffFactory.Create(buffName, duration, selfObj, gameObject,args));
            }
            else
            {
                float endTime = BuffEndTimes[buffName];
                float now = Time.time;
                //buff已经结束
                if (now > endTime)
                {
                    Buffs.Enqueue(BuffFactory.Create(buffName, duration, selfObj, gameObject, args));
                }
                else
                {
                    //当前+持续时间大于buff结束时间，设置新的结束时间，小于则不用管，被上个buff效果包含了
                    if (now + duration > endTime)
                    {
                        BuffEndTimes[buffName] = now + duration;
                    }
                }

            }
        }
        private void ClampMonsterPosition(Transform monsterTransform)
        {
            Vector3 position = monsterTransform.position;
            position.x = Mathf.Clamp(position.x, minBoundary.x, maxBoundary.x);
            position.y = Mathf.Clamp(position.y, minBoundary.y, maxBoundary.y);
            monsterTransform.position = position;
        }
    }
}
