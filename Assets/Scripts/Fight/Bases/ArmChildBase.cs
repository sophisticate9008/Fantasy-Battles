
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Factorys;
using MyBase;
using UnityEngine;

namespace MyBase
{


    public class ArmChildBase : MonoBehaviour, IClone, IArmChild
    {
        private float stayTriggerTime;
        public ArmConfigBase Config => ConfigManager.Instance.GetConfigByClassName(GetType().Name) as ArmConfigBase;
        public GlobalConfig GlobalConfig => ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
        // public Dictionary<string, float> DamageAddition => GlobalConfig.GetDamageAddition();
        public virtual GameObject TargetEnemyByArm { get; set; }
        public virtual GameObject TargetEnemy { get; set; }
        public bool IsInit { get; set; }
        public Dictionary<string, IComponent> InstalledComponents { get; set; } = new();
        private Vector3 direction;
        public Vector3 Direction
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
        public bool IsOutOfBounds()
        {
            // 获取子弹在屏幕上的位置
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

            // 如果子弹超出屏幕边界，返回 true
            return viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsNotSelf(collision))
            {
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
        }
        //排除自身
        private bool IsNotSelf(Collider2D collision)
        {
            IArmChild self = collision.GetComponent<IArmChild>();
            return self == null;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsNotSelf(collision))
            {
                CollideObjs["exit"].Enqueue(collision.gameObject);

            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {

            if (IsNotSelf(collision))
            {
                if (Time.time - stayTriggerTime > Config.AttackCd)
                {
                    stayTriggerTime = Time.time;
                    CollideObjs["stay"].Enqueue(collision.gameObject);
                }

            }
        }
        public void TriggerByType(string type, GameObject obj)
        {
            TriggerByTypeCallBack(type);
            foreach (var component in InstalledComponents)
            {
                foreach (var _ in component.Value.Type)
                {
                    if (_ == type)
                    {
                        component.Value.TriggerExec(obj);
                    }
                }
            }
        }
        public virtual void TriggerByTypeCallBack(string type)
        {

        }
        private void OnTriggerByQueue()
        {
            TriggerByType("update", null);
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
            var triggerType = Config.TriggerType;

            // 当队列中有对象时，逐个处理
            while (queue.Count > 0)
            {
                var obj = queue.Dequeue();

                // 如果当前 key 匹配触发类型，则创建伤害
                if (triggerType == key)
                {
                    CreateDamage(obj);
                }

                // 调用触发处理
                TriggerByType(key, obj);

                // 可以选择在每次处理后等待一帧，以免阻塞主线程
                yield return null;
            }
        }
        public virtual void Update()
        {
            if (IsInit)
            {

                Move();
                OnTriggerByQueue();
            }
        }
        //重写自定义传入tlc，比如说区域中心伤害翻倍之类的
        public virtual void CreateDamage(GameObject enemyObj)
        {
            FighteManager.Instance.SelfDamageFilter(enemyObj, gameObject);
        }
        public virtual void CreateDamage(GameObject enemyObj, float tlc)
        {
            FighteManager.Instance.SelfDamageFilter(enemyObj, gameObject, tlc: tlc);
        }
        public virtual void Move()
        {

            transform.Translate(Config.Speed * Time.deltaTime, 0, 0);

            // 超出屏幕范围时销毁
            if (IsOutOfBounds())
            {
                ReturnToPool();
            }

        }
        public void ChangeRotation()
        {
            float rotateZ = Mathf.Atan2(Direction.y, Direction.x);
            transform.rotation = Quaternion.Euler(0, 0, rotateZ * Mathf.Rad2Deg);

        }
        public virtual void Init()
        {

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
        public List<GameObject> FindTargetInScope(int num = 1, GameObject expectObj = null,
           bool setTargetEnemy = true, float scopeRadius = -1, bool isRandom = false)
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

            // 如果 expectObj 不为空，使用其位置作为检测中心，否则使用当前物体的碰撞体中心
            if (expectObj != null && expectObj.activeSelf)
            {
                detectionCenter = expectObj.transform.position;
            }
            else
            {
                Collider2D collider = GetComponent<Collider2D>();
                detectionCenter = collider.bounds.center;
            }

            // 获取范围内的所有碰撞体，按离底部远近排序
            Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(detectionCenter, scopeRadius);

            // 排除 expectObj 本身
            if (expectObj != null)
            {
                collidersInRange = collidersInRange.Where(collider => collider.gameObject != expectObj).ToArray();
            }

            // 筛选出所有包含 EnemyBase 组件的敌人
            List<EnemyBase> enemiesInRange = collidersInRange
                .Select(collider => collider.GetComponent<EnemyBase>())
                .Where(enemy => enemy != null)
                .OrderBy(enemy => Vector2.Distance(detectionCenter, enemy.transform.position))
                .ToList();

            // 打印敌人位置调试信息
            foreach (EnemyBase enemy in enemiesInRange)
            {
                Debug.Log(enemy.transform.position);
            }

            // 如果没有敌人，则返回空列表
            if (enemiesInRange.Count == 0)
            {
                return new List<GameObject>();
            }

            // 如果 isRandom 为 true，随机选择 num 个敌人
            List<GameObject> selectedEnemies;
            if (isRandom)
            {
                selectedEnemies = enemiesInRange
                    .OrderBy(e => UnityEngine.Random.value) // 随机打乱顺序
                    .Take(num) // 选择 num 个敌人
                    .Select(e => e.gameObject) // 获取对应的 GameObject
                    .ToList();
            }
            else
            {
                // 按顺序选择最近的 num 个敌人
                selectedEnemies = enemiesInRange
                    .Take(num) // 选择 num 个敌人
                    .Select(e => e.gameObject) // 获取对应的 GameObject
                    .ToList();
            }

            // 如果 num == 1，将唯一敌人设置为 TargetEnemy
            if (num == 1 && selectedEnemies.Count > 0)
            {
                if (setTargetEnemy)
                {
                    TargetEnemy = selectedEnemies[0];
                }
            }

            return selectedEnemies;
        }

        public void ReturnToPool()
        {

            ObjectPoolManager.Instance.ReturnToPool(GetType().Name + "Pool", gameObject);
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
        }
        public virtual void OnEnable()
        {

            stayTriggerTime = -10;
            Invoke(nameof(ReturnToPool), Config.Duration);
        }
        //路径伤害

    }
}