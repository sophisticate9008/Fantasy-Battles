using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ToolManager : ManagerBase<ToolManager>
{
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



}