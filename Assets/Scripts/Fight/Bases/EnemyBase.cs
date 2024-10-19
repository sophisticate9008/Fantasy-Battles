
using System.Collections.Generic;
using Factorys;
using UnityEngine;

namespace MyBase
{
    public class EnemyBase : MonoBehaviour, IEnemy, IClone
    {
        public AnimatorManager animatorManager;
        public Animator animator;
        public bool CanAction { get; set; } = true;
        public EnemyConfigBase Config => ConfigManager.Instance.GetConfigByClassName(GetType().Name) as EnemyConfigBase;
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
            isDead = false;
            NowLife = Config.Life;
            MaxLife = Config.Life;
            ImmunityCount = Config.ImmunityCount;
            TransmitBack(y: 0, returnSpawn: true);
            CanAction = true;
            IsInit = true;
            try {
                animatorManager.SetAnimParameter(animator, "isRunning", true);
            }catch {}
            

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
                    transform.position = newPosition;
                }
            }
            else
            {
                // 正常情况下，只减少 y 坐标
                Vector3 newPosition = transform.position;
                newPosition.y += y;
                transform.position = newPosition;
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
            else
            {
                animatorManager.PlayAnim(animator, 1f);
            }
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
            ObjectPoolManager.Instance.ReturnToPool(GetType().Name + "Pool", gameObject);
        }

        public void AddBuff(string buffName, GameObject selfObj, float duration)
        {
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
                Buffs.Enqueue(BuffFactory.Create(buffName, duration, selfObj, gameObject));
            }
            else
            {
                float endTime = BuffEndTimes[buffName];
                float now = Time.time;
                //buff已经结束
                if (now > endTime)
                {
                    Buffs.Enqueue(BuffFactory.Create(buffName, duration, selfObj, gameObject));
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
    }
}
