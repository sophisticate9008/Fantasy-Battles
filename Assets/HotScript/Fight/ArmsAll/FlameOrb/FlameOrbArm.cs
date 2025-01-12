


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameOrbArm : ArmBase
{
    public GameObject ball;
    public float moveTime = 0.5f;
    protected override void Start() {
        base.Start();
        CreateBallPool();
    }
    private void CreateBallPool() {
        ObjectPoolManager.Instance.CreatePool("FlameOrbBallPool", ball, 5, 5);
    }
    private GameObject GetBallFromPool() {
        GameObject ballClone = ObjectPoolManager.Instance.GetFromPool("FlameOrbBallPool", ball);
        return ballClone;
    }
    public override void Attack()
    {
        GameObject ball = GetBallFromPool();
        ball.transform.position = transform.position;
        StartCoroutine(Throw(ball));
        
    }

    public IEnumerator Throw(GameObject ballClone)
    {
        Vector3 targetPos = TargetEnemy.transform.position;
        ToolManager.Instance.TransmitByStep(moveTime, targetPos, ballClone);
        yield return new WaitForSeconds(moveTime);
        // 到达目标位置后触发 IndeedAttack
        IndeedAttack(targetPos);
        Destroy(ballClone);
    }
    public void IndeedAttack(Vector3 targetPos)
    {
        // 执行实际攻击逻辑
        ArmChildBase obj = GetOneFromPool();
        obj.transform.position = targetPos;
        obj.TargetEnemyByArm = TargetEnemy;
        obj.Init();
        // 这里可以实现伤害处理等逻辑
    }
}
