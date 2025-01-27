using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YooAsset;
public class JewelHandleUIBase : TheUIBase
{
    private Image pic;
    private TextMeshProUGUI simpleName;
    private TextMeshProUGUI desContent;
    private Material material;
    private Button lockButton;
    private Button unlockButton;
    private Button wash;
    private Button embed;
    private TextMeshProUGUI countText;
    private TextMeshProUGUI placeText;
    public ItemBase itemInfo;
    private JewelBase JewelInfo => itemInfo as JewelBase;
    private List<JewelBase> PlaceJewels => (List<JewelBase>)PlayerDataConfig.GetValue("place" + itemInfo.placeId);
    List<Button> indexButtons;
    List<Button> unembedButtons;
    private List<Button> IndexButtons
    {
        get
        {
            if (indexButtons != null)
            {
                return indexButtons;
            }
            else
            {
                indexButtons = transform.RecursiveFind("PlaceJewels").GetComponentsInDirectChildren<Button>();
                return indexButtons;
            }
        }
    }
    List<Button> UnembedButtons
    {
        get
        {
            if (unembedButtons != null)
            {
                return unembedButtons;
            }
            else
            {
                List<Button> _ = new();
                foreach (Button b in IndexButtons)
                {
                    _.Add(b.transform.RecursiveFind("Unembed").GetComponent<Button>());
                }
                unembedButtons = _;
                return unembedButtons;
            }
        }
    }
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
    public override void OnDestroy()
    {
        PlayerDataConfig.OnDataChanged -= OnJewelChange;
    }
    public void FindNecessary()
    {

        pic = transform.RecursiveFind("Pic").GetComponent<Image>();
        simpleName = transform.RecursiveFind("SimpleName").GetComponent<TextMeshProUGUI>();
        desContent = transform.RecursiveFind("Content").GetComponent<TextMeshProUGUI>();
        material = transform.RecursiveFind("Title").GetComponent<Image>().material;
        lockButton = transform.RecursiveFind("Lock").GetComponent<Button>();
        unlockButton = transform.RecursiveFind("Unlock").GetComponent<Button>();
        wash = transform.RecursiveFind("Wash").GetComponent<Button>();
        embed = transform.RecursiveFind("Embed").GetComponent<Button>();
        countText = transform.RecursiveFind("Count").GetComponent<TextMeshProUGUI>();
        placeText = transform.RecursiveFind("Place").GetComponent<TextMeshProUGUI>();
    }
    void ChangeTextsColor() {
        ItemUtil.ChangeTextColor(simpleName.transform, itemInfo.level);
        ItemUtil.ChangeTextColor(desContent.transform, itemInfo.level);
        ItemUtil.ChangeTextColor(countText.transform, itemInfo.level);
        ItemUtil.ChangeTextColor(placeText.transform, itemInfo.level);
    }
    public override void Init()
    {
        FindNecessary();
        wash.onClick.RemoveAllListeners();
        embed.onClick.RemoveAllListeners();
        wash.onClick.AddListener(OnWash);
        embed.onClick.AddListener(OnEmbed);
        lockButton.onClick.RemoveAllListeners();
        lockButton.onClick.AddListener(OnUnlock);
        unlockButton.onClick.RemoveAllListeners();
        unlockButton.onClick.AddListener(OnLock);
        ChangeInfo();
        ShowJewelsOnPlace();
        if (itemInfo.level < 5)
        {
            wash.gameObject.SetActive(false);
        }
        else
        {
            wash.gameObject.SetActive(true);
        }
    }
    public void InitByEquipment()
    {
        FindNecessary();
        transform.RecursiveFind("JewelDes").gameObject.SetActive(false);
        ShowJewelsOnPlace();
        BindUnembedButton();
    }
    private void ChangeInfo()
    {
        pic.sprite = CommonUtil.GetAssetByName<Sprite>(itemInfo.resName);
        simpleName.text = itemInfo.simpleName;
        desContent.text = itemInfo.description;
        ChangeTextsColor();
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
        countText.text = "" + itemInfo.count;
        placeText.text = "" + ItemUtil.PlaceIdToPlaceName(itemInfo.placeId);
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
            b.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
            ItemUtil.ChangeTextColor(b.transform.GetChild(1), level);
        }

    }
    void OnWash()
    {
        gameObject.SetActive(false);
        GameObject WashPanelPrefab = CommonUtil.GetAssetByName<GameObject>("Wash");
        Wash washUI = Instantiate(WashPanelPrefab).AddComponent<Wash>();
        washUI.originItem = JewelInfo;
        washUI.Init();
        UIManager.Instance.ShowUI(washUI);
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
    void BindUnembedButton()
    {
        int idx = 0;
        foreach (Button b in UnembedButtons)
        {
            b.onClick.RemoveAllListeners();//防止重复添加
            b.gameObject.SetActive(true);
            int _ = idx++;
            b.onClick.AddListener(() =>
            {
                OnUnembedButtonClick(_);
            });
        }
    }
    void OnUnembedButtonClick(int index)
    {
        try
        {
            JewelBase origin = PlaceJewels[index];
            PlayerDataConfig.jewels.Add(origin);
            PlaceJewels.Remove(origin);
            PlayerDataConfig.UpdateValueAdd("jewelChange", 1);
        }
        catch
        {

        }

    }

    //装备界面复用

    //镶嵌的时候copy并更改ui
    private void GenerateSelectUI()
    {
        GameObject backup = Instantiate(gameObject);
        JewelHandleUIBase theUIBase = backup.GetComponent<JewelHandleUIBase>();
        theUIBase.itemInfo = itemInfo;
        theUIBase.Init();
        UIManager.Instance.ShowUI(theUIBase);
        backup.transform.RecursiveFind("Embed").gameObject.SetActive(false);
        backup.transform.RecursiveFind("Wash").gameObject.SetActive(false);
        backup.transform.RecursiveFind("Msg").gameObject.SetActive(true);
        theUIBase.BeginSelect();
    }

    private void BeginSelect()
    {
        for (int i = 0; i < IndexButtons.Count; i++)
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
        ToolManager.Instance.SetTimeout(() => UIManager.Instance.CloseUI(), 1f);
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