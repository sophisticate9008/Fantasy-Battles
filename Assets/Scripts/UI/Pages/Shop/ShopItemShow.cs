using System;
using TMPro;
using UnityEngine;

public class ShopItemShow : MonoBehaviour
{
    private PlayerDataConfig playerDataConfig; // 存储 PlayerDataConfig 的实例

    private void Start()
    {
        // 获取 PlayerDataConfig 的实例，假设它是通过某种方式创建或获取的
        playerDataConfig = new PlayerDataConfig();

        // 监听 PlayerDataConfig 数据变化的事件
        playerDataConfig.OnDataChanged += UpdateSingleItem;
        UpdateInitialUI();
    }
    private void UpdateInitialUI()
    {
        // 更新所有 UI 元素，确保它们显示当前值
        UpdateSingleItem("diamondCount");
        UpdateSingleItem("keyPurpleCount");
        UpdateSingleItem("keyblueCount");
    }

    private void OnDestroy()
    {
        // 移除事件监听，避免内存泄漏
        if (playerDataConfig != null)
        {
            playerDataConfig.OnDataChanged -= UpdateSingleItem;
        }
    }

    // 更新单个 UI 元素的方法
    private void UpdateSingleItem(string fieldName)
    {
        // 使用反射获取 PlayerDataConfig 中的最新值
        int newValue = playerDataConfig.GetValue(fieldName);

        // 根据 fieldName 更新相应的 UI 元素
        switch (fieldName)
        {
            case "diamondCount":
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newValue.ToString();
                break;
            case "keyPurpleCount":
                transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = newValue.ToString();
                break;
            case "keyblueCount":
                transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = newValue.ToString();
                break;
        }
    }
}
