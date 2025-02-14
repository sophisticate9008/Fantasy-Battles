using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class ToolManager : ManagerBase<ToolManager>
{
    public PlayerDataConfig PlayerDataConfig { get => ConfigManager.Instance.GetConfigByClassName("PlayerData") as PlayerDataConfig; set { } }
    public GameObject ItemUIPrefab;
    private void Start()
    {
        ItemUIPrefab = Instantiate(CommonUtil.GetAssetByName<GameObject>("ItemBase"));
        if (ItemUIPrefab.GetComponent<ItemUIBase>() == null)
        {
            ItemUIPrefab.AddComponent<ItemUIBase>();
        }

        if (ItemUIPrefab.GetComponent<JewelUIBase>() == null)
        {
            ItemUIPrefab.AddComponent<JewelUIBase>();
        }

        CreatePool();
    }
    private void CreatePool()
    {
        ObjectPoolManager.Instance.CreatePool("ItemUIPool", ItemUIPrefab, 5, 150);
    }
    public JewelUIBase GetJewelUIFromPool()
    {
        GameObject obj = ObjectPoolManager.Instance.GetFromPool("ItemUIPool", ItemUIPrefab);
        obj.GetComponent<ItemUIBase>().enabled = false;
        JewelUIBase jub = obj.GetComponent<JewelUIBase>();
        if (jub != null)
        {
            return jub;
        }
        else
        {
            return obj.AddComponent<JewelUIBase>();
        }
    }
    public ItemUIBase GetItemUIFromPool()
    {

        GameObject obj = ObjectPoolManager.Instance.GetFromPool("ItemUIPool", ItemUIPrefab);
        obj.GetComponent<JewelUIBase>().enabled = false;
        ItemUIBase iub = obj.GetComponent<ItemUIBase>();
        if (iub != null)
        {
            return iub;
        }
        else
        {
            return obj.AddComponent<ItemUIBase>();
        }

    }
    public void ReturnItemUIToPool(GameObject obj)
    {
        ObjectPoolManager.Instance.ReturnToPool("ItemUIPool", obj);
    }
    public Coroutine SetTimeout(Action action, float delay)
    {
        return StartCoroutine(TimeoutCoroutine(action, delay));
    }

    private IEnumerator TimeoutCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
    public Coroutine SetTimeoutUnScaled(Action action, float delay)
    {
        return StartCoroutine(TimeoutCoroutineUnScaled(action, delay));
    }

    private IEnumerator TimeoutCoroutineUnScaled(Action action, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }
    public void TransmitByStep(float t, Vector3 targetPosition, GameObject obj, bool isRotate = true)
    {

        StartCoroutine(TransmitByStepCoroutine(t, targetPosition, obj, isRotate));
    }
    private IEnumerator TransmitByStepCoroutine(float t, Vector3 targetPosition, GameObject obj, bool isRotate)
    {
        if (isRotate)
        {
            Vector3 direction = (targetPosition - obj.transform.position).normalized;
            // 先旋转物体
            ChangeRotation(direction, obj);
        }

        float elapsed = 0;
        float totalDistance = Vector3.Distance(obj.transform.position, targetPosition);
        float speed = totalDistance / t;

        while (elapsed < t)
        {
            elapsed += Time.deltaTime;

            // 计算剩余距离并设置步长，避免略过目标
            float remainingDistance = Vector3.Distance(obj.transform.position, targetPosition);
            float step = Mathf.Min(speed * Time.deltaTime, remainingDistance);

            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, step);

            // 确保在接近目标时逐渐减速
            if (remainingDistance < 0.01f)
                break;

            yield return null;
        }

        // 确保最终位置到达目标
        obj.transform.position = targetPosition;
    }
    public void ChangeRotation(Vector3 direction, GameObject obj)
    {
        float rotateZ = Mathf.Atan2(direction.y, direction.x);
        obj.transform.rotation = Quaternion.Euler(0, 0, rotateZ * Mathf.Rad2Deg);
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<ParticleSystem>(out ParticleSystem ps))
            {
                var main = ps.main;

                main.startRotation = -rotateZ;
            }
        }
    }
    public void GetReward(List<(string resName, int count)> itemData, List<ItemBase> jewels = null)
    {
        List<ItemBase> items = new();
        foreach (var (resName, count) in itemData)
        {
            PlayerDataConfig.UpdateValueAdd(resName, count);
            items.Add(ItemFactory.Create(resName, count));
        }
        if (jewels != null)
        {
            items.AddRange(jewels);
        }
        UIManager.Instance.OnItemUIShow("获得奖励", items);
    }

    // 生成需要数量的字符串，不够则是红色
    public string GenerateNeedCountText(string fieldName, int need)
    {
        int currentCount = (int)PlayerDataConfig.GetValue(fieldName);
        string preText = need > currentCount ? CommonUtil.ChangeTextColor(need.ToString(), "red") : need.ToString();
        string intactText = preText + "/" + currentCount;
        return intactText;
    }

    #region 寻找范围内敌人
    public List<GameObject> FindEnemyInScope(Vector3 detectionCenter, float scopeRadius, int num = -1, bool isRandomSel = false, List<GameObject> exceptObjs = null)
    {
        if (num == 0)
        {
            return null;
        }

        // 获取范围内的所有碰撞体，按离底部远近排序
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(detectionCenter, scopeRadius);

        //排除exceptObjs
        if (exceptObjs != null)
        {
            collidersInRange = collidersInRange.Where(collider => !exceptObjs.Contains(collider.gameObject)).ToArray();
        }

        // 筛选出所有包含 EnemyBase 组件的敌人
        List<EnemyBase> enemiesInRange = collidersInRange
            .Select(collider => collider.GetComponent<EnemyBase>())
            .Where(enemy => enemy != null)
            .OrderBy(enemy => Vector2.Distance(detectionCenter, enemy.transform.position))
            .ToList();

        // 如果没有敌人，则返回空列表
        if (enemiesInRange.Count == 0)
        {
            return new List<GameObject>();
        }

        // 如果 isRandom 为 true，随机选择 num 个敌人
        List<GameObject> selectedEnemies;
        if(num < 0) {
             return enemiesInRange.Select(x => x.gameObject).ToList();
        }
        if (isRandomSel)
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
        return selectedEnemies;
    }
    #endregion


}