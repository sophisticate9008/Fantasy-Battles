
using UnityEngine;
public class TheUIBase : MonoBehaviour
{
    // private void Start()
    // {
    //     BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
    //     collider.isTrigger = true;
    // }
    public GameObject Prefab
    {
        get { return gameObject; }
        set { }
    }

    public PlayerDataConfig PlayerDataConfig { get => ConfigManager.Instance.GetConfigByClassName("PlayerData") as PlayerDataConfig; set { } }

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
        FindAndReturnItemUI(transform);
    }
    private void FindAndReturnItemUI(Transform parent)
    {
        // 获取当前物体和所有子物体中的 ItemUIBase
        ItemUIBase[] itemUIs = parent.GetComponentsInChildren<ItemUIBase>();

        // 遍历所有找到的 ItemUIBase，并将它们返回对象池
        foreach (var itemUI in itemUIs)
        {
            // itemUI.transform.localPosition = Vector3.zero; // 重置位置
            // itemUI.transform.localRotation = Quaternion.identity; // 重置旋转
            // itemUI.transform.localScale = Vector3.one; // 重置缩放
            RectTransform rectTransform = itemUI.transform as RectTransform;
            if (rectTransform != null)
            {
                // 这是一个 RectTransform，可以安全地进行操作
                rectTransform.rotation = Quaternion.Euler(0, 0, 0); // 重置 Z 轴旋转
            }
            else
            {
                // 不是 RectTransform，可以进行相应处理
                Debug.LogWarning("Transform is not a RectTransform!");
            }

            ToolManager.Instance.ReturnItemUIToPool(itemUI.gameObject);
        }
    }
}