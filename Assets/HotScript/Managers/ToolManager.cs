using System;
using System.Collections;
using UnityEngine;
public class ToolManager : ManagerBase<ToolManager>
{
    public Coroutine SetTimeout(Action action, float delay)
    {
        return StartCoroutine(TimeoutCoroutine(action, delay));
    }

    private IEnumerator TimeoutCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
    public void TransmitByStep(float t, Vector3 targetPosition, GameObject obj)
    {

        StartCoroutine(TransmitByStepCoroutine(t, targetPosition, obj));
    }
    private IEnumerator TransmitByStepCoroutine(float t, Vector3 targetPosition, GameObject obj)
    {
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


}