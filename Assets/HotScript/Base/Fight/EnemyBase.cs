
using System;
using System.Collections.Generic;
using System.Linq;
using MyEnums;
using UnityEngine;


public class EnemyBase : MonoBehaviour, IEnemy
{

    public float GBHRate = 0;
    public bool isFire => buffEffects.Any(buff => buff.Contains("点燃"));
    public bool isFreezen => buffEffects.Contains("冰冻");
    public float lastAddBloodTime = 0;
    public float moveAnimatorSpeed = 1;
    public SpriteRenderer sr;
    private bool isIdle = false;
    public AnimatorManager animatorManager => AnimatorManager.Instance;
    private Animator _animator;

    public Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                return _animator;
            }
            else
            {
                return _animator;
            }
        }
    }
    public bool CanAction { get; set; } = true;
    public EnemyConfigBase ConstConfig => ConfigManager.Instance.GetConfigByClassName(GetType().Name) as EnemyConfigBase;
    public EnemyConfigBase Config { get; set; }
    public GlobalConfig GlobalConfig => ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
    public bool IsInit { get; set; }
    public Queue<BuffBase> Buffs { get; } = new();
    public float ControlEndTime { get; set; } = 0f;
    public Dictionary<string, float> BuffEndTimes { get; set; } = new();
    public List<string> buffEffects;
    public Vector2 leftBottomBoundary;
    public Vector2 rightBottomBoundary;
    public Dictionary<string, List<Action>> allTypeActions;
    //硬控总结束时间
    public float HardControlEndTime { get; set; }

    public int ImmunityCount { get; set; }
    public int MaxLife { get; set; }
    public int NowLife { get; set; }
    public float EasyHurt { get; set; } = 0;
    public int FrozenHurtCount { get; set; } = 0;
    public bool isDead;
    public virtual void Init()
    {
        EasyHurt = 0;
        lastAddBloodTime = Time.time;
        allTypeActions = new() { { "die", new() } };
        ControlEndTime = 0;
        Config ??= ConstConfig.Clone() as EnemyConfigBase;
        ChangeScale(Config.SelfScale);
        ChangeMass();
        ChangeAnimatorSpeed();
        NowLife = (int)(Config.Life * FighteManager.Instance.mb.bloodRatio);
        MaxLife = NowLife;
        if (Config.CharacterType == "elite")
        {
            Config.BloodBarCount = 10;
            NowLife *= 10;
            MaxLife *= 10;
        }
        if (Config.CharacterType == "boss")
        {
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
            animatorManager.PlayAnim(animator, moveAnimatorSpeed);
        }
        catch{}


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
        if (isDead)
        {
            ReturnToPool();
        }
        // else
        // {
        //     animatorManager.PlayAnim(animator, 1f);
        // }
    }
    public virtual void Update()
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
    #region 移动
    public virtual void IndeedMove() {
        transform.Translate(Config.Speed * Time.deltaTime * Vector3.down);
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
            IndeedMove();
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
        transform.position = originalPosition + new Vector3(0.00001f, 0, 0); // 在X轴上施加微小的移动

        // 立即恢复到原始位置
        transform.position = originalPosition;
    }
    #endregion 

    #region  攻击技能
    public virtual void Attack()
    {
        if (Config.attackRange == AttackRange.near)
        {
            IndeedAttack();
        }
        else
        {
            TelAttack();
        }

    }
    public virtual void TelAttack()
    {
        GameObject bullet = ObjectPoolManager.Instance.GetFromPool(Config.GetType().Name.Replace("Config", "") + "BulletPool", Config.BulletPrefab);
        bullet.transform.position = transform.position;
        Vector3 targetPos = new(transform.position.x, FighteManager.Instance.leftBottomBoundary.y, 0);
        ToolManager.Instance.TransmitByStep(Config.telAttackArriveTime, targetPos, bullet, false);
        bullet.transform.localScale *= Config.SelfScale;
        ToolManager.Instance.SetTimeout(() =>
        {
            bullet.transform.localScale /= Config.SelfScale;
            IndeedAttack();
            ObjectPoolManager.Instance.ReturnToPool(Config.GetType().Name.Replace("Config", "") + "BulletPool", bullet);
        }, Config.telAttackArriveTime);
    }
    public virtual void IndeedAttack()
    {
        FighteManager.Instance.EnemyDamegeFilter(Config.Damage, Config.AttackCount);
    }

    #endregion
    public virtual bool CalLifeAndIsKill(int damage, string owner)
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
            return true;
        }
        return false;
    }
    public virtual void AddLife(int count)
    {
        NowLife = Math.Min(NowLife + count, MaxLife);
        FighteManager.Instance.ShowBloodAdd(count, gameObject);
    }
    #region 死亡
    public virtual void Die(string owner)
    {
        if (!isDead)
        {
            isDead = true;
            foreach(var action in allTypeActions["die"]) {
                action.Invoke();
            }
            FighteManager.Instance.RecordKill(owner);
            FighteManager.Instance.AddExp(1);
            if (Config.CharacterType == "elite")
            {
                FighteManager.Instance.DefeatElite();
            }
            if (Config.CharacterType == "normal")
            {
                ReturnToPool();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        // animatorManager.SetAnimParameter(animator, "isDead", true);
    }
    #endregion

    public void BuffEffect()
    {
        while (Buffs.Count > 0)
        {
            var buff = Buffs.Dequeue();
            UseBuff(buff);
        }
    }
    public virtual void UseBuff(BuffBase buff)
    {
        buff.EffectAndAutoRemove();
    }
    void ChangeScale(float scaleFactor)
    {
        gameObject.transform.localScale *= scaleFactor;
        foreach (Transform child in gameObject.transform)
        {
            child.localScale *= scaleFactor;
        }
    }
    public void ReturnToPool()
    {
        ChangeScale(1 / Config.SelfScale);
        GetComponent<Rigidbody2D>().mass = 1;
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
    public virtual void AddBuff(string buffName, GameObject selfObj, float duration, params object[] args)
    {
        //全局异常状态增强
        duration *= GlobalConfig.EnemyBuffTimeAddition;

        //免疫指定buff
        if (Config.BuffImmunityList.Any(immunity => buffName.Contains(immunity)))
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

    public void ChangeMass()
    {

        GetComponent<Rigidbody2D>().mass = Config.Mass;
    }
    public void ChangeAnimatorSpeed()
    {
        moveAnimatorSpeed = Config.Speed / Constant.moveOneAnimatorSpeed;

    }
    #region 范围索敌
    public List<GameObject> FindEnemyInScope()
    {
        return ToolManager.Instance.FindEnemyInScope(transform.position, Config.ScopeRadius, exceptObjs: new List<GameObject>() { gameObject });
    }
    #endregion


    #region 计算距离防线的屏幕y距离
    public float ScreenDistanceToWall
    {
        get
        {
            var wallScreenPos = Camera.main.ViewportToScreenPoint(Constant.leftBottomViewBoundary);
            var selfScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            return selfScreenPos.y - wallScreenPos.y;
        }
    }
    #endregion

    public virtual bool AcceptHarm(GameObject enemy, GameObject armChild)
    {
        return true;
    }
}

