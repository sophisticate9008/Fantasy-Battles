using UnityEngine;

public class DraggableUIBase : TheUIBase
{
    public static GameObject SingleDrag;
    public RectTransform selfRectTransform;
    public Vector2 offset;
    public bool isDragging = false;
    public RectTransform parentRectTransform;
    public virtual void Start()
    {
        // 获取 RectTransform 组件，并缓存到字段
        selfRectTransform = GetComponent<RectTransform>();
        parentRectTransform = selfRectTransform.parent.GetComponent<RectTransform>();
    }

    public virtual void BeginDrag()
    {
        // 可以在这里添加拖拽开始的逻辑
    }

    public virtual void EndDrag()
    {
        // 可以在这里添加拖拽结束的逻辑
    }

    public virtual void DragWithMouse()
    {
        Vector2 mousePosition = Input.mousePosition;

        // 判断鼠标是否在 UI 元素范围内
        if (RectTransformUtility.RectangleContainsScreenPoint(selfRectTransform, mousePosition))
        {
            // 按下鼠标左键时，开始拖拽
            if (Input.GetMouseButtonDown(0))
            {
                if (!isDragging && !SingleDrag)
                {
                    isDragging = true;
                    SingleDrag = gameObject;
                    parentRectTransform = selfRectTransform.parent.GetComponent<RectTransform>();
                    BeginDrag();
                }
                // 获取鼠标相对于 UI 元素的偏移量
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, mousePosition, null, out localPoint);
                offset = selfRectTransform.localPosition - (Vector3)localPoint;  // 将 Vector2 转换为 Vector3
                // 设置拖拽状态
            }
        }

        // 鼠标拖动时，更新 UI 元素的位置
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, mousePosition, null, out localPoint);

            // 更新元素的位置，使用偏移量保持与鼠标的相对位置
            selfRectTransform.localPosition = localPoint + offset;  // 将 Vector2 转换为 Vector3
        }

        // 松开鼠标左键时，停止拖拽
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            if (SingleDrag == gameObject)
            {
                SingleDrag = null;
                EndDrag();
            }


        }
    }

    public virtual void Update()
    {
        if (!SingleDrag || SingleDrag == gameObject)
        {
            DragWithMouse();
        }
        // 每帧调用拖拽处理

    }
}
