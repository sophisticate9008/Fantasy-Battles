
using System;
using System.Collections.Generic;
using System.Linq;


using UnityEngine;


public class EnemyBase : MonoBehaviour, IEnemy
{
    public SpriteRenderer sr;
    private bool isIdle = false;
    public AnimatorManager animatorManager;
    public Animator animator => GetComponent<Animator>();
    public bool CanAction { get; set; } = true;
    public EnemyConfigBase ConstConfig => ConfigManager.Instance.GetConfigByClassName(GetType().Name) as EnemyConfigBase;
    public EnemyConfigBase Config { get; set; }
    public GlobalConfig GlobalConfig => ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
    public bool IsInit { get; set; }
    public Queue<IBuff> Buffs { get; } = new();
    public Dictionary<string, IComponent> InstalledComponents { get; } = new();
    public float ControlEndTime { get; set; } = 0f;
    public Dictionary<string, float> BuffEndTimes { get; set; } = new();
    public Vector2 leftBottomBoundary;
    public Vector2 rightBottomBoundary;
    //硬控总结束时间
    public float HardControlEndTime { get; set; }

    public int ImmunityCount { get; set; }
    public int MaxLife { get; set; }
    public int NowLife { get; set; }
    public float EasyHurt { get; set; }
    public bool isDead;
    public virtual void Init()
    {
        ControlEndTime = 0;
        Config ??= ConstConfig.Clone() as EnemyConfigBase;
        NowLife = (int)(Config.Life * FighteManager.Instance.mb.bloodRatio);
        MaxLife = NowLife;
        if(Config.CharacterType == "elite") {
            Config.BloodBarCount = 10;
            NowLife *= 10;
            MaxLife *= 10;
        }
        if(Config.CharacterType == "boss") {
            Config.BloodBarCount = 20;

        }
        //清除buff列表
        Buffs.Clear();
        isDead = false;

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
                ToolManager.Instance.TransmitByStep(y, newPosition, gameObject, false);
            }
        }
        else
        {
            // 正常情况下，只减少 y 坐标
            Vector3 newPosition = transform.position;
            newPosition.y += y;
            ToolManager.Instance.TransmitByStep(0.5f, newPosition, gameObject, false);
        }
    }


    protected virtual void Start()
    {
        animatorManager = AnimatorManager.Instance;
        sr = GetComponent<SpriteRenderer>();
    }
    private void RunningAnim()
    {
        animatorManager.SetAnimParameter(animator, "isAttacking", false);
        animatorManager.SetAnimParameter(animator, "isRunning", true);//一直播放跑动动画
        if (!isDead)
        {
            animatorManager.PlaySpecificAnim(animator, "Run");
        }
        isIdle = false;
    }
    public void FixedUpdate()
    {
        if (!CanAction && !isDead)
        {
            RunningAnim();
            animatorManager.StopAnim(animator);

        }
        if(isDead) {
            ReturnToPool();
        }
        // else
        // {
        //     animatorManager.PlayAnim(animator, 1f);
        // }
    }
    public void Update()
    {
        ClampMonsterPosition(transform);
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
        if (isDead)
        {
            return;
        }
        Vector3 position = transform.position;
        float bottomEdge = -Camera.main.orthographicSize;
        if (position.y > FighteManager.Instance.leftBottomBoundary.y + Config.RangeFire)
        {
            transform.Translate(Config.Speed * Time.deltaTime * Vector3.down);
            RunningAnim();
        }
        else //到达射程内
        {
            PreventSleep();
            string currentName = animatorManager.GetCurrentAnimName(animator);
            if (currentName == "Attack" || isIdle)
            {
                return;
            }
            isIdle = true;
            animatorManager.SetAnimParameter(animator, "isRunning", false);//回归idle
            animatorManager.SetAnimParameter(animator, "isAttacking", false);
            animatorManager.PlayAnimWithCallback(animator, "Idle", () =>
            {
                if (isDead)
                {
                    ReturnToPool();
                    return;
                }
                Action _ = () =>
                {
                    animatorManager.SetAnimParameter(animator, "isAttacking", true);//开启技能动画
                    animatorManager.PlayAnimWithCallback(animator, "Attack", () =>
                    {
                        isIdle = false;
                        animatorManager.SetAnimParameter(animator, "isAttacking", false);//关闭技能动画
                        animatorManager.PlaySpecificAnim(animator, "Idle");
                        Attack();

                        if (isDead)
                        {
                            ReturnToPool();
                            return;
                        }

                    });
                };
                ToolManager.Instance.SetTimeout(_, Config.AttackCd);
            });

        }
        //限定在盒子内


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
        FighteManager.Instance.EnemyDamegeFilter(Config.Damage, Config.AttackCount);
    }

    public virtual void Skill()
    {
        throw new System.NotImplementedException();
    }

    public virtual void CalLife(int damage, string owner)
    {
        sr.color = Color.red;
        ToolManager.Instance.SetTimeout(() =>
        {
            sr.color = Color.white;
        }, 0.1f);
        NowLife -= damage;
        
        if (NowLife <= 0)
        {
            Die(owner);
        }
    }

    public virtual void Die(string owner)
    {
        if (!isDead)
        {
            isDead = true;
            OnByType("die", gameObject);
            FighteManager.Instance.RecordKill(owner);
            FighteManager.Instance.AddExp(1);
            if(Config.CharacterType == "elite") {
                FighteManager.Instance.DefeatElite();
            }
            if(Config.CharacterType == "normal") {
                ReturnToPool();
            }else {
                Destroy(gameObject);
            }
            

        }
        // animatorManager.SetAnimParameter(animator, "isDead", true);
    }

    void OnByType(string type, GameObject obj)
    {
        foreach (var component in InstalledComponents)
        {
            foreach (var _ in component.Value.Types)
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
    #region 增加buff
    public void AddBuff(string buffName, GameObject selfObj, float duration, params object[] args)
    {
        //全局异常状态增强
        duration *= GlobalConfig.EnemyBuffTimeAddition;

        //免疫指定控制buff
        if (Config.ControlImmunityList.Any(immunity => buffName.Contains(immunity)))
        {
            return;
        }
        // //伤害位置对不上不能上buff,移到碰撞开始判断
        // if (Config.ActionType == "land" && selfObj.GetComponent<ArmChildBase>().Config.DamagePos != "all")
        // {
        //     return;
        // }
        if (!BuffEndTimes.ContainsKey(buffName))
        {
            Buffs.Enqueue(BuffFactory.Create(buffName, duration, selfObj, gameObject, args));
        }
        else
        {
            float endTime = BuffEndTimes[buffName];
            float now = Time.time;
            //buff已经结束
            if (now >= endTime)
            {
                Buffs.Enqueue(BuffFactory.Create(buffName, duration, selfObj, gameObject, args));
            }
            else
            {
                //当前+持续时间大于等于buff结束时间，设置新的结束时间，小于则不用管，被上个buff效果包含了
                if (now + duration >= endTime)
                {
                    BuffEndTimes[buffName] = now + duration;
                }
            }

        }
    }
    #endregion
    private void ClampMonsterPosition(Transform monsterTransform)
    {

        Vector3 position = monsterTransform.position;
        position.x = Mathf.Clamp(position.x, FighteManager.Instance.leftBottomBoundary.x, FighteManager.Instance.rightTopBoundary.x);
        position.y = Mathf.Clamp(position.y, FighteManager.Instance.leftBottomBoundary.y, FighteManager.Instance.rightTopBoundary.y);
        monsterTransform.position = position;
    }
}

