using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // 在编辑模式下执行
[RequireComponent(typeof(RectTransform))]
public class CircularLayoutGroup : LayoutGroup
{
    [Header("布局参数")]
    public float radius = 300f;                // 环形半径
    [Range(0f, 360f)] public float startAngle = 0f; // 起始角度
    public bool clockwise = true;             // 顺时针排列
    public float scaleZoomFactor = 5;

    [Header("视角控制")]
    public float viewAngle = 0f;              // 环形的俯视角度

    private bool layoutDirty = false;         // 标志布局是否需要更新

    protected override void OnEnable()
    {
        base.OnEnable();
        layoutDirty = true; // 在启用时标记布局需要更新
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        layoutDirty = true; // 在参数更改时标记布局需要更新
    }

    void Update()
    {
        // 如果布局需要更新
        if (layoutDirty)
        {
            UpdateLayout(); // 强制更新布局
            layoutDirty = false; // 标记布局更新完成
        }
    }

    public override void CalculateLayoutInputHorizontal()
    {
        layoutDirty = true; // 横向布局时需要更新
    }

    public override void CalculateLayoutInputVertical()
    {
        layoutDirty = true; // 纵向布局时需要更新
    }

    public override void SetLayoutHorizontal()
    {
        layoutDirty = true; // 强制更新横向布局
    }

    public override void SetLayoutVertical()
    {
        layoutDirty = true; // 强制更新纵向布局
    }

    /// <summary>
    /// 强制更新环形布局
    /// </summary>
    private void UpdateLayout()
    {
        // 获取所有子物体（包括不可见和未激活的子物体）
        int childCount = transform.childCount;
        if (childCount == 0) return;

        // 获取父容器的尺寸和对齐参考点
        Vector2 alignmentOffset = GetAlignmentOffset();

        // 计算角度步长
        float angleStep = 360f / childCount;
        float directionMultiplier = clockwise ? -1f : 1f;

        // 记录子物体的位置信息
        List<(Transform child, Vector2 position, float zIndex, Canvas canvas, float scale)> childPositions = new List<(Transform, Vector2, float, Canvas, float)>();

        // 遍历所有子物体
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            RectTransform rectChild = child.GetComponent<RectTransform>();
            if (rectChild == null) continue; // 确保子物体是 RectTransform

            // 计算当前子物体的角度
            float angle = startAngle + i * angleStep * directionMultiplier;

            // 转换角度为弧度
            float radian = angle * Mathf.Deg2Rad;

            // 根据角度和半径计算位置
            float x = Mathf.Sin(radian) * radius + alignmentOffset.x;
            float y = Mathf.Cos(radian) * radius * Mathf.Cos(viewAngle * Mathf.Deg2Rad) + alignmentOffset.y;

            // 计算物体与玩家视角的Z轴深度
            float zIndex = Mathf.Cos(radian) * Mathf.Abs(y);

            // 修正右半部分的ZIndex层级
            if (y < 0)
            {
                zIndex += Mathf.Abs(y) * 1.5f; // 让下方的物体最接近
            }

            // 设置子物体的位置
            rectChild.anchoredPosition = new Vector2(x, y);

            // 获取Canvas组件，用于控制渲染顺序
            Canvas childCanvas = child.GetComponent<Canvas>();
            if (childCanvas == null)
            {
                childCanvas = child.gameObject.AddComponent<Canvas>(); // 如果没有Canvas，添加一个新的Canvas
            }

            // 计算缩放比例，模拟真实的近大远小效果
            // 这里我们假设视角的y位置越低，物体的y位置越大，缩放越小
            // 使用一个线性函数来表示大小，常数因子可根据需要调整

            float distanceFactor = Mathf.Abs(y); // 基于Y轴的高度计算距离
            float scale = 1f / (1.5f - distanceFactor * scaleZoomFactor / 10000); // 线性缩放，调整常数0.15f来调整缩放速率

            // 将子物体的位置、Z轴深度和缩放比例记录下来
            childPositions.Add((child, new Vector2(x, y), zIndex, childCanvas, scale));

            // 重置子物体的旋转
            rectChild.localRotation = Quaternion.identity;
        }

        // 根据Z轴深度排序子物体，Z值越大（越靠近视角的物体）越应该遮挡其他物体
        childPositions.Sort((a, b) => b.zIndex.CompareTo(a.zIndex));

        // 更新排序后的渲染顺序
        for (int i = 0; i < childPositions.Count; i++)
        {
            // 获取每个子物体的Canvas并更新其sortingOrder
            Canvas childCanvas = childPositions[i].canvas;
            childCanvas.overrideSorting = true;
            childCanvas.sortingOrder = i + 1; // 根据排序顺序设置Canvas的sortingOrder

            // 更新子物体的缩放
            RectTransform rectChild = childPositions[i].child.GetComponent<RectTransform>();
            rectChild.localScale = new Vector3(childPositions[i].scale, childPositions[i].scale, 1f);
        }

        // 强制更新布局
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }


    /// <summary>
    /// 根据 childAlignment 计算对齐偏移
    /// </summary>
    /// <returns>对齐偏移量</returns>
    private Vector2 GetAlignmentOffset()
    {
        // 父容器的 pivot 用于计算对齐参考点
        Vector2 pivotOffset = rectTransform.pivot;

        // 父容器的尺寸
        Vector2 size = rectTransform.rect.size;

        // 基于 pivot 和 childAlignment 计算对齐偏移
        Vector2 offset = new Vector2(
            (pivotOffset.x) * size.x,
            -(pivotOffset.y) * size.y
        );

        switch (childAlignment)
        {
            case TextAnchor.UpperLeft:
                offset += new Vector2(-size.x / 4, size.y / 4);
                break;
            case TextAnchor.UpperCenter:
                offset += new Vector2(0, size.y / 4);
                break;
            case TextAnchor.UpperRight:
                offset += new Vector2(size.x / 4, size.y / 4);
                break;
            case TextAnchor.MiddleLeft:
                offset += new Vector2(-size.x / 4, 0);
                break;
            case TextAnchor.MiddleCenter:
                // 已居中，无需额外偏移
                break;
            case TextAnchor.MiddleRight:
                offset += new Vector2(size.x / 4, 0);
                break;
            case TextAnchor.LowerLeft:
                offset += new Vector2(-size.x / 4, -size.y / 4);
                break;
            case TextAnchor.LowerCenter:
                offset += new Vector2(0, -size.y / 4);
                break;
            case TextAnchor.LowerRight:
                offset += new Vector2(size.x / 4, -size.y / 4);
                break;
        }

        return offset;
    }
}
