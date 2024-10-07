using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using YooAsset;

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

    public PlayerDataConfig PlayerDataConfig { get => ConfigManager.Instance.GetConfigByClassName("PlayerDataConfig") as PlayerDataConfig; set { } }

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

}