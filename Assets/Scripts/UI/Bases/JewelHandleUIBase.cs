using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YooAsset;

public class JewelHandleUIBase : TheUIBase
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
    private JewelBase JewelInfo => itemInfo as JewelBase;
    private List<JewelBase> PlaceJewels => (List<JewelBase>)PlayerDataConfig.GetValue("place" + itemInfo.placeId);
    private Button[] IndexButtons => transform.RecursiveFind("PlaceJewels").GetComponentsInChildren<Button>();
    private void Start()
    {
        PlayerDataConfig.OnDataChanged += OnJewelChange;
    }
    private void OnJewelChange(string fieldName)
    {
        if (fieldName == "jewelChange")
        {
            ShowJewelsOnPlace();
        }
    }
    private void OnDestroy() {
        PlayerDataConfig.OnDataChanged -= OnJewelChange;
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
        if (itemInfo.level < 5)
        {
            refresh.gameObject.SetActive(false);
        }
        else
        {
            refresh.gameObject.SetActive(true);
        }
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
        int num = PlaceJewels.Count;
        for (int i = 0; i < 5; i++)
        {
            Button b = IndexButtons[i];
            Transform pic = b.transform.GetChild(2);
            int level = i < num ? PlaceJewels[i].level : 1;
            string text = i < num ? PlaceJewels[i].description : "暂未镶嵌";
            if (i < num)
            {
                pic.gameObject.SetActive(true);
                ItemUtil.SetSprite(pic, PlaceJewels[i].resName);
            }
            else
            {
                pic.gameObject.SetActive(false);
            }
            ItemUtil.ChangeMetrailColor(b.transform.GetChild(0), level);
            b.transform.GetChild(1).GetComponent<Text>().text = text;
        }

    }
    private void OnEmbed()
    {

        //判断同id 的level
        int idx = 0;
        foreach (JewelBase jewel in PlaceJewels)
        {
            if (jewel.id == JewelInfo.id)
            {
                if (jewel.level >= JewelInfo.level)
                {
                    UIManager.Instance.OnMessage("当前宝石品质不高于镶嵌品质");
                    return;
                }
                else
                {
                    JewelInfo.SubtractCount(1);
                    PlayerDataConfig.jewels.Add(PlaceJewels[idx]);
                    PlaceJewels[idx] = JewelInfo.Clone();
                    PlayerDataConfig.UpdateValueAdd("jewelChange", 1);
                    return;
                }
            }
            idx++;
        }
        //小于5直接镶嵌
        if (PlaceJewels.Count < 5)
        {
            PlaceJewels.Add(JewelInfo.Clone());
            JewelInfo.SubtractCount(1);
            PlayerDataConfig.UpdateValueAdd("jewelChange", 1);
            return;
        }
        //等于5替换
        gameObject.SetActive(false);
        GenerateSelectUI();

    }

    //镶嵌的时候copy并更改ui
    private void GenerateSelectUI()
    {
        GameObject backup = Instantiate(gameObject);
        JewelHandleUIBase theUIBase = backup.GetComponent<JewelHandleUIBase>();
        theUIBase.itemInfo = itemInfo;
        theUIBase.Init();
        UIManager.Instance.ShowUI(theUIBase);
        Debug.Log("生成备份");
        backup.transform.RecursiveFind("Embed").gameObject.SetActive(false);
        backup.transform.RecursiveFind("Refresh").gameObject.SetActive(false);
        backup.transform.RecursiveFind("Msg").gameObject.SetActive(true);
        theUIBase.BeginSelect();
    }

    private void BeginSelect()
    {
        for (int i = 0; i < IndexButtons.Length; i++)
        {
            int index = i; // 创建一个局部变量存储当前的按钮索引
            IndexButtons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }
    void OnButtonClicked(int index)
    {
        JewelBase origin = PlaceJewels[index];
        PlayerDataConfig.jewels.Add(origin);
        JewelInfo.SubtractCount(1);
        PlaceJewels[index] = JewelInfo.Clone();
        PlayerDataConfig.UpdateValueAdd("jewelChange", 1);
        UIManager.Instance.SetTimeout(() => UIManager.Instance.CloseUI(), 1f);
    }
    private void OnRefresh()
    {
    }
    private void OnLock()
    {
        itemInfo.isLock = true;
        lockButton.gameObject.SetActive(true);
        unlockButton.gameObject.SetActive(false);
        PlayerDataConfig.UpdateValueAdd("jewelChange", 1);
    }
    private void OnUnlock()
    {
        lockButton.gameObject.SetActive(false);
        unlockButton.gameObject.SetActive(true);
        itemInfo.isLock = false;
        PlayerDataConfig.UpdateValueAdd("jewelChange", 1);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}