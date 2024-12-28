using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CircularLayoutGroup : LayoutGroup, IDragHandler, IEndDragHandler
{
    [Header("布局参数")]
    public float radius = 300f;
    [Range(0f, 360f)] public float startAngle = 0f;
    public bool clockwise = true;
    public float scaleZoomFactor = 5;

    [Header("视角控制")]
    public float viewAngle = 0f;

    [Header("拖拽参数")]
    public float dragSpeed = 0.1f; // 拖拽速度控制系数
    public float inertia = 0.95f; // 惯性系数，范围[0, 1)

    private bool isDragging = false;
    private float currentVelocity = 0f; // 当前的旋转速度
    private Vector2 dragStartPosition; // 拖拽开始时的位置

    private bool layoutDirty = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        layoutDirty = true;
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        layoutDirty = true;
    }

    void Update()
    {
        if (layoutDirty)
        {
            UpdateLayout();
            layoutDirty = false;
        }

        // 如果不是拖拽状态，应用惯性效果
        if (!isDragging && Mathf.Abs(currentVelocity) > 0.01f)
        {
            startAngle += currentVelocity * Time.deltaTime;

            // 非线性减速公式：速度按平方根或指数缓慢衰减
            currentVelocity *= Mathf.Pow(inertia, Time.deltaTime * 100f);

            layoutDirty = true;
        }
    }

    public override void CalculateLayoutInputHorizontal()
    {
        layoutDirty = true;
    }

    public override void CalculateLayoutInputVertical()
    {
        layoutDirty = true;
    }

    public override void SetLayoutHorizontal()
    {
        layoutDirty = true;
    }

    public override void SetLayoutVertical()
    {
        layoutDirty = true;
    }

    private void UpdateLayout()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        Vector2 alignmentOffset = GetAlignmentOffset();

        float angleStep = 360f / childCount;
        float directionMultiplier = clockwise ? -1f : 1f;

        List<(Transform child, Vector2 position, float zIndex, Canvas canvas, float scale)> childPositions = new List<(Transform, Vector2, float, Canvas, float)>();

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            RectTransform rectChild = child.GetComponent<RectTransform>();
            if (rectChild == null) continue;

            float angle = startAngle + i * angleStep * directionMultiplier;
            float radian = angle * Mathf.Deg2Rad;

            float x = Mathf.Sin(radian) * radius + alignmentOffset.x;
            float y = Mathf.Cos(radian) * radius * Mathf.Cos(viewAngle * Mathf.Deg2Rad) + alignmentOffset.y;

            float zIndex = Mathf.Cos(radian) * Mathf.Abs(y);

            if (y < 0)
            {
                zIndex += Mathf.Abs(y) * 1.5f;
            }

            rectChild.anchoredPosition = new Vector2(x, y);

            Canvas childCanvas = child.GetComponent<Canvas>();
            if (childCanvas == null)
            {
                childCanvas = child.gameObject.AddComponent<Canvas>();
            }

            float distanceFactor = Mathf.Abs(y);
            float scale = 1f / (1.5f - distanceFactor * scaleZoomFactor / 10000);

            childPositions.Add((child, new Vector2(x, y), zIndex, childCanvas, scale));

            rectChild.localRotation = Quaternion.identity;
        }

        childPositions.Sort((a, b) => b.zIndex.CompareTo(a.zIndex));

        for (int i = 0; i < childPositions.Count; i++)
        {
            Canvas childCanvas = childPositions[i].canvas;
            childCanvas.overrideSorting = true;
            childCanvas.sortingOrder = i + 1;

            RectTransform rectChild = childPositions[i].child.GetComponent<RectTransform>();
            rectChild.localScale = new Vector3(childPositions[i].scale, childPositions[i].scale, 1f);
        }

        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }

    private Vector2 GetAlignmentOffset()
    {
        Vector2 pivotOffset = rectTransform.pivot;
        Vector2 size = rectTransform.rect.size;

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

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            // 记录拖拽开始时的位置
            RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
            if (parentRect == null) return;

            dragStartPosition = parentRect.InverseTransformPoint(eventData.position);
            isDragging = true;
        }

        // 获取当前拖拽位置
        RectTransform currentParentRect = transform.parent.GetComponent<RectTransform>();
        if (currentParentRect == null) return;

        Vector2 currentLocalPosition = currentParentRect.InverseTransformPoint(eventData.position);
        float delta = dragStartPosition.x - currentLocalPosition.x;


        // 更新角度，根据拖拽的位移改变
        startAngle += delta * dragSpeed;
        currentVelocity = delta / Time.deltaTime;  // 更新旋转速度

        dragStartPosition = currentLocalPosition;  // 更新起始位置

        layoutDirty = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}
