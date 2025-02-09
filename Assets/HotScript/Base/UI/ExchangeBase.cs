
using UnityEngine;
using UnityEngine.UI;
using YooAsset;
public class ExchangeBase : SingleConsumeBase
{
    private int buyCount;
    public string goodName;
    public string currencyName;
    public int price;
    public int goodCount;
    public override string ItemName => currencyName;
    public override int ConsumeCount => price * buyCount;
    private Text goodNameText;
    public Image needItem;
    private Text needNum;
    private Button addButton;
    private Button subtractButton;
    private InputField inputField;
    public Button good;
    public Button confirm;
    private ItemBase itemInfo;
    private const int maxBuyCount = 99;
    private int CanBuyMaxCount => (int)PlayerDataConfig.GetValue(currencyName) / price;
    // 初始化方法
    public override void Init()
    {
        FindNecessary();
        BindButton();
        GenerateItem();
        inputField.text = buyCount.ToString();
        goodNameText.text = itemInfo.simpleName;
        needItem.sprite = CommonUtil.GetAssetByName<Sprite>(currencyName);
        // 限制输入框只能输入数字
        inputField.contentType = InputField.ContentType.IntegerNumber;

        // 当输入框的值变化时更新 buyCount
        inputField.onValueChanged.AddListener(OnInputValueChanged);

        // 让输入框在启动时验证当前值
        ValidateBuyCount();
    }

    // 查找必要的 UI 元素
    public void FindNecessary()
    {
        goodNameText = transform.RecursiveFind("GoodName").GetComponent<Text>();
        addButton = transform.RecursiveFind("Add").GetComponent<Button>();
        subtractButton = transform.RecursiveFind("Subtract").GetComponent<Button>();
        inputField = transform.RecursiveFind("Input").GetComponent<InputField>();
        good = transform.RecursiveFind("Good").GetComponent<Button>();
        confirm = transform.RecursiveFind("Confirm").GetComponent<Button>();
        needItem = transform.RecursiveFind("NeedItem").GetComponent<Image>();
        needNum = transform.RecursiveFind("NeedNum").GetComponent<Text>();
    }

    // 生成物品
    public void GenerateItem()
    {
        Transform currentItem = transform.RecursiveFind("Good"); // 找到现有部分

        if (currentItem != null)
        {
            // 保存原有物体的 RectTransform 设置
            RectTransform originalRectTransform = currentItem.GetComponent<RectTransform>();

            // 销毁现有物体
            
            // 加载新Prefab并实例化                                                                 
            ItemUIBase itemUI = ToolManager.Instance.GetItemUIFromPool();
            itemInfo = ItemFactory.Create(goodName, goodCount);
            itemUI.itemInfo = itemInfo;
            itemUI.Init();
            itemUI.transform.CopyRectTransform(originalRectTransform);
            // 复制 RectTransform 的属性
            Destroy(currentItem.gameObject);
        }
    }



    // 绑定按钮
    public override void BindButton()
    {
        // 绑定之前先移除旧的监听器，避免重复绑定
        addButton.onClick.RemoveAllListeners();
        subtractButton.onClick.RemoveAllListeners();
        confirm.onClick.RemoveAllListeners();

        // 绑定确认按钮的点击事件
        confirm.onClick.AddListener(PreConsume);

        // 绑定加减按钮的点击事件
        addButton.onClick.AddListener(IncreaseCount);
        subtractButton.onClick.AddListener(DecreaseCount);
    }

    // 增加购买数量
    private void IncreaseCount()
    {
        buyCount = Mathf.Clamp(buyCount + 1, 0, Mathf.Min(CanBuyMaxCount,maxBuyCount));  // 确保不超过上限
        inputField.onValueChanged.RemoveAllListeners();  // 先移除所有监听，避免递归调用
        inputField.text = buyCount.ToString();  // 更新输入框的值
        inputField.onValueChanged.AddListener(OnInputValueChanged);  // 重新绑定监听
        needNum.text = (price * buyCount).ToString();  // 更新价格显示
    }

    // 减少购买数量
    private void DecreaseCount()
    {
        buyCount = Mathf.Clamp(buyCount - 1, 0, Mathf.Min(CanBuyMaxCount,maxBuyCount));  // 确保不低于0
        inputField.onValueChanged.RemoveAllListeners();  // 先移除所有监听，避免递归调用
        inputField.text = buyCount.ToString();  // 更新输入框的值
        inputField.onValueChanged.AddListener(OnInputValueChanged);  // 重新绑定监听
        needNum.text = (price * buyCount).ToString();  // 更新价格显示
    }

    // 输入框的值变化时更新 buyCount
    private void OnInputValueChanged(string value)
    {
        // 尝试解析输入框的值
        if (int.TryParse(value, out int parsedValue))
        {
            buyCount = Mathf.Clamp(parsedValue, 0, Mathf.Min(CanBuyMaxCount,maxBuyCount));  // 限制 buyCount 范围
        }
        else
        {
            // 如果输入非法值，将其重置为当前的 buyCount
            buyCount = 0;  // 默认重置为 0，如果输入无效
        }

        inputField.onValueChanged.RemoveAllListeners();  // 先移除监听避免递归
        inputField.text = buyCount.ToString();  // 更新输入框
        inputField.onValueChanged.AddListener(OnInputValueChanged);  // 重新添加监听
        needNum.text = (price * buyCount).ToString();  // 更新显示的价格
    }

    // 验证并更新输入框和购买数量
    private void ValidateBuyCount()
    {
        buyCount = Mathf.Clamp(buyCount, 0, Mathf.Min(CanBuyMaxCount,maxBuyCount));  // 确保购买数量在合法范围内
        inputField.text = buyCount.ToString();  // 更新输入框显示的值
        needNum.text = (price * buyCount).ToString();  // 更新显示的价格
    }

    public override bool PostConsume()
    {
        PlayerDataConfig.UpdateValueAdd(goodName, buyCount * goodCount);
        ToolManager.Instance.SetTimeout(ValidateBuyCount, 0.5f);
        UIManager.Instance.OnMessage("购买成功");
        return true;
    }
}
