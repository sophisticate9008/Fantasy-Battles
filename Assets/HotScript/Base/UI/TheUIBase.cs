
using System.Collections.Generic;
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
        if(processedObjects.Count > 0) {
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

}