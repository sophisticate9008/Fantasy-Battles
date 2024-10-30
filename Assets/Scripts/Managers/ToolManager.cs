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
    public IEnumerator TransmitByStep(float t, Vector3 targetPosition, GameObject obj)
    {
        float elapsed = 0;
        float totalDistance = Vector3.Distance(transform.position, targetPosition);
        float speed = totalDistance / t;
        float step = speed * Time.deltaTime;
        while (elapsed < t)
        {
            elapsed += Time.deltaTime;
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, step);
            yield return null;
        }
    }
}