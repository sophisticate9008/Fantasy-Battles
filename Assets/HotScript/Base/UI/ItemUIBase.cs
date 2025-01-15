
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;



public class ItemUIBase : TheUIBase
{

    public ItemBase itemInfo;
    private string ResName => itemInfo.resName;
    public int Level => itemInfo.level;
    public int Count => itemInfo.count;
    public int PlaceId => itemInfo.placeId;
    public int Id => itemInfo.id;
    public override void Init()
    {
        gameObject.SetActive(true);
        string color = ItemUtil.LevelToColorString(Level);
        Sprite background = CommonUtil.GetAssetByName<Sprite>(color);
        Prefab.GetComponent<Image>().sprite = background;
        Transform children = Prefab.transform.GetChild(0);
        children.GetComponent<Image>().sprite = CommonUtil.GetAssetByName<Sprite>(ResName);
        children.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Count.ToString();
        if (Id < 500)
        {
            children = Prefab.transform.GetChild(1);
            children.GetComponent<Image>().sprite = CommonUtil.GetAssetByName<Sprite>("place" + PlaceId);
            children.GetComponent<Image>().gameObject.SetActive(true);
        }
        if (itemInfo.isLock)
        {
            GameObject lockImg = transform.RecursiveFind("Lock").gameObject;
            lockImg.SetActive(true);
        }

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(ShowDes);
    }
    public virtual void ShowDes()
    {
        GameObject desPrefab = CommonUtil.GetAssetByName<GameObject>("Des");
        DesUIBase des = Instantiate(desPrefab).AddComponent<DesUIBase>();
        des.itemInfo = itemInfo;
        des.Init();
        UIManager.Instance.ShowUI(des);
    }
    private void OnDestroy() {
        gameObject.SetActive(false);
        ToolManager.Instance.ReturnItemUIToPool(gameObject);
    }


}



