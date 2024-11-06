using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// 为 Transform 扩展的递归查找方法，查找指定名称的子物体。
    /// </summary>
    /// <param name="parent">要在其下查找的父物体的 Transform。</param>
    /// <param name="name">要查找的子物体的名称。</param>
    /// <returns>如果找到匹配名称的 Transform，则返回该 Transform；否则返回 null。</returns>
    public static Transform RecursiveFind(this Transform parent, string name)
    {
        // 如果当前物体的名称匹配，返回当前 Transform
        if (parent.name == name)
            return parent;

        // 遍历当前物体的所有子物体
        foreach (Transform child in parent)
        {
            // 递归调用，在每个子物体下继续查找
            Transform result = RecursiveFind(child, name);
            // 如果找到匹配的 Transform，立即返回
            if (result != null)
            {
                return result;
            }
        }

        // 如果在所有子物体中都未找到，返回 null
        return null;
    }

    /// <summary>
    /// 复制原 RectTransform 的布局属性到新 RectTransform 上
    /// </summary>
    /// <param name="newTransform">新创建的 Transform（目标 Transform）</param>
    /// <param name="originalTransform">原始 Transform（源 Transform）</param>
    public static void CopyRectTransform(this Transform newTransform, Transform originalTransform)
    {
        RectTransform originalRectTransform = originalTransform as RectTransform;
        RectTransform newRectTransform = newTransform as RectTransform;

        if (originalRectTransform != null && newRectTransform != null)
        {
            newRectTransform.anchorMin = originalRectTransform.anchorMin;
            newRectTransform.anchorMax = originalRectTransform.anchorMax;
            newRectTransform.pivot = originalRectTransform.pivot;
            newRectTransform.sizeDelta = originalRectTransform.sizeDelta;
            newRectTransform.localPosition = originalRectTransform.localPosition;
            newRectTransform.localScale = originalRectTransform.localScale;
            newRectTransform.localRotation = originalRectTransform.localRotation;
        }
        else
        {
            Debug.LogWarning("Both original and new Transform must be RectTransforms.");
        }
    }

    /// <summary>
    /// 清除当前 Transform 下的所有子物体。
    /// </summary>
    /// <param name="parent">要清除子物体的 Transform。</param>
    public static void ClearChildren(this Transform parent)
    {
        // 遍历当前 Transform 的所有子物体
        foreach (Transform child in parent)
        {
            // 销毁子物体
            Object.Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// 获得直系子代的组件
    /// </summary>
    /// <param name="parent">父物体 Transform。</param>
    public static List<T> GetComponentsInDirectChildren<T>(this Transform parent) where T : Component
    {
        List<T> components = new List<T>();
        // 遍历当前 Transform 的所有子物体
        foreach (Transform child in parent)
        {
            // 获取子物体上的组件
            T component = child.GetComponent<T>();
            // 如果子物体上存在组件，则添加到列表中
            if (component != null) {
                components.Add(component); 
            }
        }
        return components;
    }

}
