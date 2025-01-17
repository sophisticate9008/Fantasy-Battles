
using System.Collections.Generic;
using TMPro;


public class ItemCountShow : TheUIBase
{
    public List<string> listenItem;

    private void Start()
    {

        // 获取 PlayerDataConfig 的实例，假设它是通过某种方式创建或获取的
        PlayerDataConfig.OnDataChanged += UpdateSingleItem;
        UpdateInitialUI();
    }
    private void UpdateInitialUI()
    {
        for (int i = 0; i < listenItem.Count; i++)
        {
            UpdateSingleItem(listenItem[i]);
        }
        // 更新所有 UI 元素，确保它们显示当前值
    }

    public override void OnDestroy()
    {
        // 移除事件监听，避免内存泄漏
        if (PlayerDataConfig != null)
        {
            PlayerDataConfig.OnDataChanged -= UpdateSingleItem;
        }
    }

    // 更新单个 UI 元素的方法
    private void UpdateSingleItem(string fieldName)
    {
        int idx = listenItem.IndexOf(fieldName);
        // 使用反射获取 PlayerDataConfig 中的最新值
        int newValue = (int)PlayerDataConfig.GetValue(fieldName);

        // 根据 fieldName 更新相应的 UI 元素

        transform.GetChild(idx).GetComponent<TextMeshProUGUI>().text = newValue.ToString();

    }
}
