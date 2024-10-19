using System.Collections;
using System.Collections.Generic;
using ArmConfigs;
using MyBase;
using UnityEngine;

public class Tornado : ArmChildBase
{
    private float smoothDamp = 1f; 
    private Vector3 velocity = Vector3.zero;
    public TornadoConfig tornadoConfig => Config as TornadoConfig;
    public List<GameObject> exceptObjs = new();
    public override void Init()
    {
        base.Init();
        exceptObjs.Clear();
        ChangeScale(Config.SelfScale);
        ChangeParticalSpeed();
    }
    public override void OnDisable()
    {
        ChangeScale(1 / Config.SelfScale);
        base.OnDisable();

    }
    void ChangeScale(float scaleFactor)
    {
        gameObject.transform.localScale *= scaleFactor;
        foreach (Transform child in gameObject.transform)
        {
            child.localScale *= scaleFactor;
        }
    }

    void ChangeParticalSpeed()
    {
        foreach (Transform child in gameObject.transform)
        {
            ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
            var main = particleSystem.main;
            main.simulationSpeed = 3;
        }
    }

    public override void Move()
    {
        int maxExcept = (EnemyManager.Instance.liveCount + 100) / 4;
        if (TargetEnemy == null)
        {
            //排除当前目标
            FindTargetInScope(exceptObjs: exceptObjs);
            exceptObjs.Add(TargetEnemy);
            //找不到敌人了
            if (TargetEnemy == null || exceptObjs.Count > maxExcept)
            {
                exceptObjs.Clear();
            }

        }

        //设置朝向目标的方向一直移动
        SetDirectionToTarget();

        Vector3 targetPosition = transform.position + Config.Speed * Time.deltaTime * Direction;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Config.Speed * Time.deltaTime);

    }
    public override UnityEngine.Vector3 Direction
    {
        get; set;
    }

    public override void OnTriggerStay2D(Collider2D collider)
    {

        OnDrag(collider);
        base.OnTriggerStay2D(collider);
        if (TargetEnemy == collider.gameObject)
        {
            TargetEnemy = null;

        }

    }
    public override void SetDirectionToTarget()
    {
        if (TargetEnemy != null)
        {
            Vector3 targetDirection = (TargetEnemy.transform.position - transform.position).normalized;
            Direction = Vector3.Lerp(Direction, targetDirection, Time.deltaTime * smoothDamp);
        }

    }
    private void OnDrag(Collider2D collider)
    {
        Vector2 tornadoCenter = transform.position; // 龙卷风中心
        float maxForce = 10f; // 最大施加力
        float maxDistance = 5f * Config.SelfScale; // 最大影响距离
        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // 保存原始速度和旋转角度
            Vector2 originalVelocity = rb.velocity;
            float originalAngularVelocity = rb.angularVelocity;

            // 锁定物体的旋转
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            float distance = Vector2.Distance(tornadoCenter, rb.position);
            float forceMagnitude = Mathf.Lerp(maxForce, 0, distance / maxDistance);
            Vector2 direction = (rb.position - tornadoCenter).normalized;

            // 施加龙卷风力
            rb.AddForce(direction * -forceMagnitude * tornadoConfig.DragDegree);

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


