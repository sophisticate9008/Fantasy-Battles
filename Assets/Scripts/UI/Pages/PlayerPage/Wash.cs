
using Factorys;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class Wash : ConsumeBase
{
    public JewelBase originItem;
    public JewelBase ItemInfo => originItem.Clone();
    public override void Start()
    {
        base.Start();
        ItemName = "washWater";
        
    }
    public override void Init()
    {
        ItemName = "washWater";
        ConsumeCount = (ItemInfo.level - 4) * 6;
        GameObject itemBasePrefab = YooAssets.LoadAssetSync("ItemBase").AssetObject as GameObject;
        ItemUIBase itemUI = Instantiate(itemBasePrefab).AddComponent<ItemUIBase>();
        itemUI.name = "ItemBase";
        itemUI.itemInfo = ItemInfo;
        itemUI.Init();
        //替换掉原来的物品
        GameObject originItem = transform.RecursiveFind("ItemBase").gameObject;
        itemUI.transform.SetParent(originItem.transform.parent, false);
        itemUI.transform.CopyRectTransform(originItem.transform);
        Destroy(originItem);
        Text desText = transform.RecursiveFind("DesText").GetComponent<Text>();
        desText.text = ItemInfo.description;
        ShowWaterNum();
    }
    void ShowWaterNum()
    {
            Text num = transform.RecursiveFind("Num").GetComponent<Text>();
            num.text = ConsumeCount + "/" + PlayerDataConfig.washWater;
    }

    public override void BindButton()
    {
        Button confirm = transform.RecursiveFind("Confirm").GetComponent<Button>();
        confirm.onClick.RemoveAllListeners();
        confirm.onClick.AddListener(PreConsume);
    }
    public override void AfterConsume()
    {
        base.AfterConsume();
        ConfirmWash();
    }
    void ConfirmWash() {
        int id = Random.Range(1, Constant.JewelMaxId + 1);
        JewelBase newJewel = ItemFactory.Create(id, ItemInfo.level, ItemInfo.placeId);
        PlayerDataConfig.jewels.Add(newJewel);
        originItem.SubtractCount(1);
        originItem = newJewel;
        PlayerDataConfig.UpdateValueAdd("jewelChange", 1);
        Init();
    }
}