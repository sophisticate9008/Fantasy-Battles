
using System.Collections;
using UnityEngine;



public class DragonLaunchArm : ArmBase
{
    public GameObject dragon;
    public GameObject bomb;
    private float bombDropHeight = 10f; // 炸弹投放高度
    private float dragonSpeed = 15f; // 飞机移动速度
    public float bombSpeed = 7f; // 炸弹下落速度

    public override void Attack()
    {
        GameObject dragonClone = GetTempFromPool(0);
        Vector3 pos = transform.position;
        pos.x = TargetEnemy.transform.position.x;
        pos.y = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0, 0)).y; // 视口底部
        dragonClone.transform.position = pos;
        StartCoroutine(DragonMove(dragonClone));
    }
    protected override void Start()
    {
        base.Start();
        CreatePool();
    }
    private void CreatePool()
    {
        ObjectPoolManager.Instance.CreatePool("DragonShadowPool", dragon, 5, 5);
        ObjectPoolManager.Instance.CreatePool("DragonThrowPool", bomb, 5, 5);
    }
    private void ReturnToPool(GameObject obj, int poolIndex)
    {
        if (poolIndex == 0)
        {
            ObjectPoolManager.Instance.ReturnToPool("DragonShadowPool", obj);
        }
        else if (poolIndex == 1)
        {
            ObjectPoolManager.Instance.ReturnToPool("DragonThrowPool", obj);

        }
    }
    private GameObject GetTempFromPool(int poolIndex)
    {
        return poolIndex switch
        {
            0 => ObjectPoolManager.Instance.GetFromPool("DragonShadowPool", dragon),
            1 => ObjectPoolManager.Instance.GetFromPool("DragonThrowPool", bomb),
            _ => null,
        };
    }
    private IEnumerator DragonMove(GameObject dragonClone)
    {
        while (dragonClone.transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y)
        {
            dragonClone.transform.Translate(0, dragonSpeed * Time.deltaTime, 0);
            yield return null;
        }

        // 当飞机超出视口时销毁
        ReturnToPool(dragonClone, 0);
        // 投放炸弹
        GameObject bombClone = GetTempFromPool(1);
        Vector3 targetPos = TargetEnemy.transform.position;
        bombClone.transform.position = new Vector3(dragonClone.transform.position.x, targetPos.y + bombDropHeight, 0);
        StartCoroutine(ThrowBomb(bombClone));
    }

    public IEnumerator ThrowBomb(GameObject bombClone)
    {
        Vector3 targetPos = TargetEnemy.transform.position;

        while (bombClone.transform.position.y > targetPos.y)
        {
            bombClone.transform.Translate(bombSpeed * Time.deltaTime, 0, 0);
            yield return null;
        }

        // 到达目标位置后触发 IndeedAttack
        IndeedAttack(bombClone);
        ReturnToPool(bombClone, 1);
    }

    public void IndeedAttack(GameObject bombClone)
    {
        // 执行实际攻击逻辑
        ArmChildBase obj = GetOneFromPool();
        obj.transform.position = bombClone.transform.position;
        obj.TargetEnemyByArm = TargetEnemy;
        obj.Init();
        // 这里可以实现伤害处理等逻辑
    }
}
