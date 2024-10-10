using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YooAsset;

public class JewelHandleUIBase : TheUIBase, IPointerClickHandler
{
    private Image pic;
    private Text simpleName;
    private Text desContent;
    private Material material;
    private Button lockButton;
    private Button unlockButton;
    private Button refresh;
    private Button embed;
    private Text countText;
    private Text placeText;
    public ItemBase itemInfo;
    public void OnPointerClick(PointerEventData eventData)
    {
        // 阻止事件传播
        eventData.Use();
    }
    public void FindNecessary()
    {

        pic = transform.RecursiveFind("Pic").GetComponent<Image>();
        simpleName = transform.RecursiveFind("SimpleName").GetComponent<Text>();
        desContent = transform.RecursiveFind("Content").GetComponent<Text>();
        material = transform.RecursiveFind("Title").GetComponent<Image>().material;
        lockButton = transform.RecursiveFind("Lock").GetComponent<Button>();
        unlockButton = transform.RecursiveFind("Unlock").GetComponent<Button>();
        refresh = transform.RecursiveFind("Refresh").GetComponent<Button>();
        embed = transform.RecursiveFind("Embed").GetComponent<Button>();
        countText = transform.RecursiveFind("Count").GetComponent<Text>();
        placeText = transform.RecursiveFind("Place").GetComponent<Text>();
    }
    public override void Init()
    {
        FindNecessary();
        refresh.onClick.RemoveAllListeners();
        embed.onClick.RemoveAllListeners();
        refresh.onClick.AddListener(OnRefresh);
        embed.onClick.AddListener(OnEmbed);
        lockButton.onClick.RemoveAllListeners();
        lockButton.onClick.AddListener(OnUnlock);
        unlockButton.onClick.RemoveAllListeners();
        unlockButton.onClick.AddListener(OnLock);
        ChangeInfo();
        ShowJewelsOnPlace();
    }

    private void ChangeInfo()
    {
        pic.sprite = YooAssets.LoadAssetSync<Sprite>(itemInfo.resName).AssetObject as Sprite;
        simpleName.text = itemInfo.simpleName;
        desContent.text = itemInfo.description;
        Material newMaterial = new(material);
        newMaterial.SetColor("_EndColor", ItemUtil.LevelToColor(itemInfo.level));
        transform.RecursiveFind("Title").GetComponent<Image>().material = newMaterial;
        if (itemInfo.isLock)
        {
            lockButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(false);
        }
        else
        {
            lockButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(true);
        }
        countText.text = "数量:" + itemInfo.count;
        placeText.text = "部位:" + ItemUtil.PlaceIdToPlaceName(itemInfo.placeId);
    }
    private void ShowJewelsOnPlace()
    {

    }
    private void OnEmbed()
    {

    }
    private void OnRefresh()
    {

    }
    private void OnLock()
    {
        itemInfo.isLock = false;
        lockButton.gameObject.SetActive(true);
        unlockButton.gameObject.SetActive(false);
        PlayerDataConfig.SaveConfig();
    }
    private void OnUnlock()
    {
        lockButton.gameObject.SetActive(false);
        unlockButton.gameObject.SetActive(true);
        itemInfo.isLock = true;
        PlayerDataConfig.SaveConfig();
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}