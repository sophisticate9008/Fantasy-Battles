
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;




public class ArmChildBase : MonoBehaviour, IArmChild
{
    public bool isUseComponent = true;
    private float alreadyStayTime;
    ArmConfigBase config;
    public ArmConfigBase Config
    {
        get
        {
            config ??= ConfigManager.Instance.GetConfigByClassName(GetType().Name) as ArmConfigBase;
            return config;
        }
        set
        {
            config = value;
        }
    }
    public GlobalConfig GlobalConfig => ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
    // public Dictionary<string, float> DamageAddition => GlobalConfig.GetDamageAddition();
    public virtual GameObject TargetEnemyByArm { get; set; }
    private GameObject targetEnemy;

    public virtual GameObject TargetEnemy
    {
        get
        {
            if (targetEnemy == null)
            {
                targetEnemy = TargetEnemyByArm;
                return targetEnemy;
            }
            else
            {
                return targetEnemy;
            }
        }
        set => targetEnemy = value;
    }
    public bool IsInit { get; set; }
    public Dictionary<string, ComponentBase> InstalledComponents { get; set; } = new();
    private Vector3 direction;
    public virtual Vector3 Direction
    {
        get { return direction; }
        set
        {
            direction = value;
            ChangeRotation();
        }
    }
    public Queue<GameObject> FirstExceptQueue { get; set; } = new();
    private readonly Dictionary<string, Queue<GameObject>> collideObjs = new() {
            {"enter", new()},
            {"stay", new()},
            {"exit", new()}
        };
    public Dictionary<string, Queue<GameObject>> CollideObjs => collideObjs;
    public bool IsOutOfScreen()
    {
        // 获取子弹在屏幕上的位置
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // 如果子弹超出屏幕边界，返回 true
        return viewportPosition.x < -0.1 || viewportPosition.x > 1.1 || viewportPosition.y < -0.1 || viewportPosition.y > 1.1;
    }
    public virtual void OnEnter2D(Collider2D collision)
    {
        ApplyForce(collision);
        while (FirstExceptQueue.Count > 0)
        {
            var obj = FirstExceptQueue.Dequeue();
            if (obj == collision.gameObject)
            {
                return;
            }
        }
        CollideObjs["enter"].Enqueue(collision.gameObject);


    }
    public bool PlaceMatch(Collider2D collision)
    {
        if (Config.DamagePos == "all")
        {
            return true;
        }
        if (Config.DamagePos == "land" && collision.gameObject.GetComponent<EnemyBase>().Config.ActionType == "sky")
        {
            return false;
        }
        return true;
    }
    // public virtual void OnCollisionEnter2D(Collision2D collision)
    // {
    //     OnEnter2D(collision.collider);

    // }
    public virtual bool BeforeTirgger(Collider2D collision)
    {
        if (!IsNotSelf(collision))
        {
            return false;
        }
        if (!PlaceMatch(collision))
        {
            return false;
        }
        return true;
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!BeforeTirgger(collision))
        {
            return;
        }
        OnEnter2D(collision);
    }
    //排除自身
    private bool IsNotSelf(Collider2D collision)
    {
        IArmChild self = collision.GetComponent<IArmChild>();
        return self == null;
    }
    public virtual void OnExit2D(Collider2D collision)
    {
        CollideObjs["exit"].Enqueue(collision.gameObject);
    }
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        OnExit2D(collision);
    }
    // public virtual void OnCollisionExit2D(Collision2D collision)
    // {
    //     OnExit2D(collision.collider);
    // }
    public virtual void OnStay2D(Collider2D collision)
    {
        if (!BeforeTirgger(collision))
        {
            return;
        }
        if (Time.time - alreadyStayTime > Config.AttackCd)
        {
            alreadyStayTime = Time.time;
            CollideObjs["stay"].Enqueue(collision.gameObject);
        }
    }
    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        OnStay2D(collision);
    }

    // public virtual void OnCollisionStayr2D(Collision2D collision)
    // {
    //     Debug.Log("stay");
    //     OnStay2D(collision.collider);
    // }
    public void OnByType(string type, GameObject obj)
    {
        OnByTypeCallBack(type);
        foreach (var component in InstalledComponents)
        {

            foreach (var _ in component.Value.Types)
            {

                if (_ == type && isUseComponent)
                {
                    component.Value.Exec(obj);
                }
            }
        }
    }
    public virtual void OnByTypeCallBack(string type)
    {

    }
    public virtual void OnByQueue()
    {
        OnByType("update", null);
        foreach (var kvp in collideObjs)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(ProcessQueueByKey(kvp.Key, kvp.Value));
            }
            // 为每个 key 单独启动一个协程来处理队列

        }
    }
    private IEnumerator ProcessQueueByKey(string key, Queue<GameObject> queue)
    {
        // 获取当前触发类型
        var onType = Config.OnType;

        // 当队列中有对象时，逐个处理
        while (queue.Count > 0)
        {
            var obj = queue.Dequeue();

            // 如果当前 key 匹配触发类型，则创建伤害
            if (onType == key)
            {
                FighteManager.Instance.AddTriggerCount(GetType().Name, gameObject);
                CreateDamage(obj);
            }
            foreach (var item in Config.typeActions)
            {
                if (item.Key == key)
                {
                    foreach (var action in item.Value)
                    {
                        action.Invoke(gameObject, obj);
                    }
                }
            }

            // 调用触发处理
            OnByType(key, obj);

            // 可以选择在每次处理后等待一帧，以免阻塞主线程
            yield return null;
        }
    }
    public virtual void Update()
    {
        if (IsInit)
        {
            Move();
            OnByQueue();
        }
    }
    //重写自定义传入tlc，比如说区域中心伤害翻倍之类的
    public virtual void CreateDamage(GameObject enemyObj)
    {
        if (enemyObj.GetComponent<EnemyBase>().AcceptHarm(enemyObj, gameObject))
        {
            for (int i = 0; i < Config.harmCount; i++)
            {
                FighteManager.Instance.SelfDamageFilter(enemyObj, gameObject, percentage: Config.percentage);
                CreateDamageOther(enemyObj);
            }
        }

    }
    public virtual void CreateDamageOther(GameObject enemyObj) {

    }
    public virtual void CreateDamage(GameObject enemyObj, float tlc)
    {
        if (enemyObj.GetComponent<EnemyBase>().AcceptHarm(enemyObj, gameObject))
        {
            for (int i = 0; i < Config.harmCount; i++)
            {
                FighteManager.Instance.SelfDamageFilter(enemyObj, gameObject, tlc: tlc, percentage: Config.percentage);
            }
        }
    }
    public virtual void Move()
    {

        transform.Translate(Config.Speed * Time.deltaTime, 0, 0);

        // 超出屏幕范围时销毁
        if (IsOutOfScreen())
        {
            ReturnToPool();
        }
    }
    public virtual void ChangeRotation()
    {
        float rotateZ = Mathf.Atan2(Direction.y, Direction.x);
        transform.rotation = Quaternion.Euler(0, 0, rotateZ * Mathf.Rad2Deg);
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<ParticleSystem>(out ParticleSystem ps))
            {
                var main = ps.main;

                main.startRotation = -rotateZ;
            }
        }
    }
    public virtual void Init()
    {
        ChangeScale(Config.SelfScale);
        if (Config.ControlBy == MyEnums.ControlBy.Arm)
        {
            StartCoroutine(BeginCuntDown());
        }
        CreateComponents();
        foreach (var component in InstalledComponents)
        {
            component.Value.Init();
        }
        IsInit = true;

    }


    public GameObject FindTargetRandom(GameObject nowEnemy, bool setTargetEnemy = true)
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        GameObject randomEnemy;
        if (enemies.Length > 0)
        {
            // 将所有敌人添加到列表中，并移除当前敌人
            List<EnemyBase> enemyList = new(enemies);
            if (nowEnemy != null && nowEnemy.activeSelf)
            {
                enemyList.Remove(nowEnemy.GetComponent<EnemyBase>());  // 移除当前敌人
            }
            if (enemyList.Count > 0)
            {
                // 随机选择一个敌人
                int randomIndex = Random.Range(0, enemyList.Count);
                randomEnemy = enemyList[randomIndex].gameObject;
            }
            else
            {
                // 没有其他敌人，设置为null
                randomEnemy = null;

            }
        }
        else
        {
            // 如果没有找到敌人，设置为null
            randomEnemy = null;
        }
        if (setTargetEnemy)
        {
            TargetEnemy = randomEnemy;
        }
        return randomEnemy;
    }
    public List<GameObject> FindTargetInScope(int num = 1, GameObject centerObj = null,
       bool setTargetEnemy = true, float scopeRadius = -1, bool isRandom = false, List<GameObject> exceptObjs = null)
    {
        if (num == 0)
        {
            return null;
        }

        Vector3 detectionCenter;
        if (scopeRadius < 0)
        {
            scopeRadius = Config.ScopeRadius;
        }

        // 如果 centerObj 不为空，使用其位置作为检测中心，否则使用当前物体的碰撞体中心
        if (centerObj != null && centerObj.activeSelf)
        {
            detectionCenter = centerObj.transform.position;
        }
        else
        {
            Collider2D collider = GetComponent<Collider2D>();
            detectionCenter = collider.bounds.center;
        }

        // 获取范围内的所有碰撞体，按离底部远近排序
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(detectionCenter, scopeRadius);

        // 排除 centerObj 本身
        if (exceptObjs != null)
        {
            exceptObjs.Add(centerObj);
        }
        else
        {
            exceptObjs = new() { centerObj };
        }
        var enemys = ToolManager.Instance.FindEnemyInScope(detectionCenter, scopeRadius, num, isRandom, exceptObjs);

        // 如果 num == 1，将唯一敌人设置为 TargetEnemy
        if (num == 1 && enemys.Count > 0)
        {
            if (setTargetEnemy)
            {
                TargetEnemy = enemys[0];
            }
        }

        return enemys;
    }

    public void ReturnToPool()
    {
        if (Config != null)
        {
            foreach (var action in Config.typeActions["return"])
            {
                action.Invoke(gameObject, null);
            }
            foreach (var component in InstalledComponents)
            {

                foreach (var _ in component.Value.Types)
                {

                    if (_ == "return" && isUseComponent)
                    {
                        Debug.Log("退出组件触发");
                        component.Value.Exec(null);
                    }
                }
            }
            ChangeScale(1 / Config.SelfScale);
            Config = null;
            ObjectPoolManager.Instance.ReturnToPool(GetType().Name + "Pool", gameObject);
        }

    }

    public virtual void CreateComponents()
    {
        foreach (var componentStr in Config.ComponentStrs)
        {
            if (!InstalledComponents.ContainsKey(componentStr))
            {
                var component = ComponentFactory.Create(componentStr, gameObject);
                InstalledComponents.Add(component.ComponentName, component);
            }


        }

    }
    public List<GameObject> LineCastAll(Vector3 startPoint, Vector3 endPoint)
    {
        // Debug.DrawLine(startPoint, endPoint, Color.red, 1.0f); 
        List<GameObject> list = new();
        if (Config.IsLineCast)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(startPoint, endPoint);
            foreach (RaycastHit2D hit in hits)
            {
                var enemy = hit.collider.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    list.Add(enemy.gameObject);
                }
            }
        }
        return list;

    }

    public virtual void OnDisable()
    {
        foreach (var temp in collideObjs)
        {
            temp.Value.Clear();
        }
        CancelInvoke();
        TargetEnemy = null;
    }
    public virtual void OnEnable()
    {

        alreadyStayTime = -10;
        if (Config.ControlBy == MyEnums.ControlBy.Self)
        {
            Invoke(nameof(ReturnToPool), Config.Duration);
        }

    }
    public virtual void ChangeScale(float scaleFactor)
    {
        gameObject.transform.localScale *= scaleFactor;
    }
    //技能持续时间结束后销毁

    //路径伤害
    public virtual void SetDirectionToTarget()
    {
        if (TargetEnemy != null)
        {
            Direction = (TargetEnemy.transform.position - transform.position).normalized;

        }
    }
    public IEnumerator BeginCuntDown()
    {
        Config.RestDuration = Config.Duration;
        while (Config.RestDuration > 0)
        {
            Config.RestDuration -= Time.deltaTime;
            yield return null;
        }
        ReturnToPool();

    }
    public virtual ArmChildBase GetOneFromPool()
    {
        ArmChildBase obj = ObjectPoolManager.Instance.GetFromPool(GetType().Name + "Pool", Config.Prefab).GetComponent<ArmChildBase>();
        return ProcessObj(obj);
    }
    public virtual ArmChildBase ProcessObj(ArmChildBase obj)
    {
        return obj;
    }
    public void ApplyForce(Collider2D collider)
    {
        Vector2 center = transform.position; // 龙卷风中心
        float maxForce = Config.MaxForce; // 最大施加力
        float maxDistance = Config.ForceBaseDistance * Config.SelfScale; // 最大影响距离
        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // 保存原始速度和旋转角度
            Vector2 originalVelocity = rb.velocity;
            float originalAngularVelocity = rb.angularVelocity;

            // 锁定物体的旋转
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            float distance = Vector2.Distance(center, rb.position);
            float forceMagnitude = Mathf.Lerp(maxForce, 0, distance / maxDistance);
            Vector2 direction = (rb.position - center).normalized;

            // 施加龙卷风力
            rb.AddForce(forceMagnitude * direction);

            // 启动协程来恢复状态
            StartCoroutine(ResetStateAfterDelay(rb, originalVelocity, originalAngularVelocity, 0.5f)); // 0.5秒后恢复状态
        }
    }
    private IEnumerator ResetStateAfterDelay(Rigidbody2D rb, Vector2 originalVelocity, float originalAngularVelocity, float delay)
    {
        yield return new WaitForSeconds(delay);
        // 恢复速度和旋转角速度
        rb.velocity = originalVelocity; // 恢复原始速度
        rb.angularVelocity = originalAngularVelocity; // 恢复原始旋转角速度
    }

}
