using System.Collections;
using ArmConfigs;
using MyBase;
using UnityEngine;

public class Tornado : ArmChildBase
{
    public TornadoConfig tornadoConfig => Config as TornadoConfig;
    public override void Init()
    {
        base.Init();
        ChangeScale();
    }

    void ChangeScale()
    {
        gameObject.transform.localScale *= Config.SelfScale;
        foreach (Transform child in gameObject.transform)
        {
            child.localScale *= Config.SelfScale;
        }
    }

    public override void Move()
    {

    }

    public override void OnTriggerStay2D(Collider2D collider)
    {
        OnDrag(collider);
        base.OnTriggerStay2D(collider);

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


