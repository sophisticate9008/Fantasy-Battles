using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DraggableToTarget : DraggableUIBase
{
    public List<RectTransform> targets = new List<RectTransform>(); // 可选目标 RectTransform 列表
    private Vector2 initialPosition; // 拖拽前的位置
    private float transitionDuration = 0.05f; // 移动动画的持续时间

    // 拖拽到目标时的回调
    public virtual void ArriveTarget(RectTransform target)
    {
        Debug.Log($"{gameObject.name} 到达目标 {target.name}");
    }

    public override void BeginDrag()
    {
        // 记录初始位置
        initialPosition = selfRectTransform.localPosition;
    }

    public override void EndDrag()
    {
        foreach (RectTransform target in targets)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(target, selfRectTransform.position))
            {
                // 检查目标是否已有子对象
                if (target.childCount > 0)
                {
                    // 获取目标的现有子对象
                    RectTransform existingChild = target.GetChild(0) as RectTransform;

                    // 将现有子对象移回拖拽元素的原始位置（带渐变）
                    StartCoroutine(SmoothMove(existingChild, initialPosition));
                    existingChild.SetParent(selfRectTransform.parent); // 设置为当前拖拽元素的父级
                }

                // 将当前拖拽元素设置为目标的子对象
                selfRectTransform.SetParent(target);
                StartCoroutine(SmoothMove(selfRectTransform, Vector2.zero)); // 平滑移动到目标中心

                // 触发回调
                ArriveTarget(target);
                return; // 停止检查其他目标
            }
        }

        // 如果没有重叠任何目标，恢复到原始位置（带渐变）
        StartCoroutine(SmoothMove(selfRectTransform, initialPosition));
    }

    // 平滑移动协程
    private IEnumerator SmoothMove(RectTransform rectTransform, Vector2 targetPosition)
    {
        Vector2 startPosition = rectTransform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            rectTransform.localPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localPosition = targetPosition; // 确保最终精确位置
    }
}
