
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public class TheUIBase : MonoBehaviour
{
    // private void Start()
    // {
    //     BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
    //     collider.isTrigger = true;
    // }


    public PlayerDataConfig PlayerDataConfig { get => ConfigManager.Instance.GetConfigByClassName("PlayerData") as PlayerDataConfig; set { } }
    public GlobalConfig GlobalConfig { get => ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig; set { } }
    public virtual void Init()
    {

    }
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     // Debug.Log("OnPointerClick"+ name);
    //     // Prevent closing when clicking inside the UI
    //     // This method will only be triggered for clicks on the background
    //     // if (eventData.pointerPress == gameObject)
    //     // {
    //     //     UIManager.Instance.CloseUI(); // Assuming you have a singleton instance
    //     // }
    // }
    public virtual void OnDestroy()
    {

    }
    private void FindAndReturnItemUI(Transform parent)
    {
        // 用于记录已处理的对象，避免重复
        HashSet<GameObject> processedObjects = new();

        // 获取当前物体和所有子物体中的 ItemUIBase
        ItemUIBase[] itemUIs = parent.GetComponentsInChildren<ItemUIBase>();

        // 遍历所有找到的 ItemUIBase，并将它们返回对象池
        foreach (var itemUI in itemUIs)
        {
            GameObject itemGameObject = itemUI.gameObject;

            // 检查对象是否已处理
            if (processedObjects.Contains(itemGameObject))
            {
                continue;
            }

            // 标记为已处理
            processedObjects.Add(itemGameObject);

            // 如果是 RectTransform，进行特定操作
            RectTransform rectTransform = itemUI.transform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.rotation = Quaternion.Euler(0, 0, 0); // 重置 Z 轴旋转
            }
            else
            {
                Debug.LogWarning("Transform is not a RectTransform!");
            }

            // 将对象返回到对象池
            ToolManager.Instance.ReturnItemUIToPool(itemGameObject);
        }
        if (processedObjects.Count > 0)
        {
            Debug.Log("返回了 " + processedObjects.Count + " 个物品UI.");
        }
    }

    public virtual void SelfDestory()
    {
        FindAndReturnItemUI(transform);
        gameObject.SetActive(false);
        ToolManager.Instance.SetTimeout(() =>
        {
            Destroy(gameObject);
        }, 10f);

    }


    #region  自动注入字段
    /// <summary>
    /// 自动注入字段方法。
    /// 该方法会通过反射获取当前类中的字段，递归查找场景中的对象，并自动将字段与场景中的对象关联。
    /// 要求：场景中的对象名称与字段名称一致。
    /// </summary>
    protected void AutoInjectFields()
    {
        // 获取当前类的所有字段（包括 public 和 private 的实例字段）
        FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            // 检查字段类型是否是 Unity 的组件类型或 GameObject 类型
            if (typeof(Component).IsAssignableFrom(field.FieldType) || field.FieldType == typeof(GameObject))
            {
                // 使用 RecursiveFind 方法，根据字段名称查找对应的 Transform
                Transform targetTransform = transform.RecursiveFind(field.Name);

                // 如果找到了对应的 Transform
                if (targetTransform != null)
                {
                    // 如果字段类型是 GameObject，直接赋值 GameObject
                    if (field.FieldType == typeof(GameObject))
                    {
                        field.SetValue(this, targetTransform.gameObject);
                    }
                    // 如果字段类型是组件，则获取组件并赋值
                    else
                    {
                        field.SetValue(this, targetTransform.GetComponent(field.FieldType));
                    }
                }
            }
        }
    }
    #endregion
    #region  UI控制效果
    private Vector3 initialPosition; // 记录初始位置
    private CanvasGroup canvasGroup; // 控制透明度
    private bool isAnimating = false; // 动画状态标志

    /// <summary>
    /// 初始化方法，记录 UI 的初始位置并获取 CanvasGroup。
    /// </summary>
    protected void InitializeUIBase()
    {
        initialPosition = transform.position; // 记录当前 UI 的位置
        canvasGroup = GetComponent<CanvasGroup>(); // 获取 CanvasGroup 组件

        if (canvasGroup == null)
        {
            // 如果没有 CanvasGroup，则自动添加
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// 从屏幕指定方向移动到初始位置，同时淡入透明度。
    /// </summary>
    /// <param name="direction">移动方向，例如 "left", "right", "up", "down"。</param>
    /// <param name="duration">移动持续时间（秒）。</param>
    public void OpenFromDirection(string direction, float duration)
    {
        if (isAnimating) return; // 如果动画正在进行，直接返回
        gameObject.SetActive(true);
        InitializeUIBase();
        Vector3 startPosition = CalculateStartPosition(direction);
        if (startPosition == Vector3.zero) return;

        // 将 UI 设置到起始位置，并将透明度置为 0
        transform.position = startPosition;
        canvasGroup.alpha = 0;

        // 标记为正在动画
        isAnimating = true;

        // 启动协程，移动到目标位置并调整透明度
        StartCoroutine(MoveToPosition(initialPosition, 1, duration, () => isAnimating = false));
    }

    /// <summary>
    /// 从当前位置移动到屏幕外的指定方向，同时淡出透明度并关闭对象。
    /// </summary>
    /// <param name="direction">移动方向，例如 "left", "right", "up", "down"。</param>
    /// <param name="duration">移动持续时间（秒）。</param>
    public void CloseToDirection(string direction, float duration)
    {
        if (isAnimating) return; // 如果动画正在进行，直接返回

        InitializeUIBase();
        Vector3 endPosition = CalculateStartPosition(direction);
        if (endPosition == Vector3.zero) return;

        // 标记为正在动画
        isAnimating = true;

        // 启动协程，移动到目标位置并调整透明度
        StartCoroutine(MoveToPosition(endPosition, 0, duration, () =>
        {
            // 在完成移动后关闭对象
            gameObject.SetActive(false);
            transform.position = initialPosition; // 恢复位置
            isAnimating = false; // 动画结束
        }));
    }

    /// <summary>
    /// 计算屏幕外的起始或结束位置。
    /// </summary>
    /// <param name="direction">方向字符串。</param>
    /// <returns>计算后的起始或结束位置。</returns>
    private Vector3 CalculateStartPosition(string direction)
    {
        Vector3 position = initialPosition;

        switch (direction.ToLower())
        {
            case "left":
                position.x = -Screen.width; // 屏幕左侧
                break;
            case "right":
                position.x = Screen.width; // 屏幕右侧
                break;
            case "up":
                position.y = Screen.height; // 屏幕上方
                break;
            case "down":
                position.y = -Screen.height; // 屏幕下方
                break;
            default:
                Debug.LogError("Invalid direction. Use 'left', 'right', 'up', or 'down'.");
                return Vector3.zero;
        }

        return position;
    }

    /// <summary>
    /// 协程：移动到目标位置并调整透明度。
    /// </summary>
    /// <param name="targetPosition">目标位置。</param>
    /// <param name="targetAlpha">目标透明度。</param>
    /// <param name="duration">移动持续时间（秒）。</param>
    /// <param name="onComplete">移动完成后的回调（可选）。</param>
    private IEnumerator MoveToPosition(Vector3 targetPosition, float targetAlpha, float duration, System.Action onComplete = null)
    {
        Vector3 startPosition = transform.position; // 起始位置
        float startAlpha = canvasGroup.alpha; // 起始透明度
        float elapsed = 0f; // 已经过时间

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 线性插值移动位置
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // 线性插值调整透明度
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            yield return null; // 等待下一帧
        }

        // 确保最终位置和透明度准确
        transform.position = targetPosition;
        canvasGroup.alpha = targetAlpha;

        // 调用完成回调（如果有）
        onComplete?.Invoke();
    }
    #endregion

}
