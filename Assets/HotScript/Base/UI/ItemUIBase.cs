
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
        ResetDiff();
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
    public void ResetDiff()
    {
        // 获取 Image 组件和子物体
        Image bg = gameObject.GetComponent<Image>();
        Image jewel = transform.GetChild(0).GetComponent<Image>();
        GameObject upgrade = transform.RecursiveFind("Upgrade").gameObject;

        // 恢复背景透明度
        if (bg != null)
        {
            Color bgColor = bg.color;
            bgColor.a = 1f;  // 恢复 alpha 为 1 (完全不透明)
            bg.color = bgColor;
        }

        // 恢复宝石透明度
        if (jewel != null)
        {
            Color jewelColor = jewel.color;
            jewelColor.a = 1f;  // 恢复 alpha 为 1 (完全不透明)
            jewel.color = jewelColor;
        }

        // 如果升级显示被禁用，恢复其状态
        if (upgrade != null)
        {
            upgrade.SetActive(false);  // 恢复为隐藏状态
        }
    }


}



