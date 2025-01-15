
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
            ToolManager.Instance.ReturnItemUIToPool(itemUI.gameObject);
        }
    }
}