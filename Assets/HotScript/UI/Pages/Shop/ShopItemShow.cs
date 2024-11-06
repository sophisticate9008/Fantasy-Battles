using System;
using TMPro;
using UnityEngine;

public class ShopItemShow : TheUIBase
{


    private void Start()
    {
        // 获取 PlayerDataConfig 的实例，假设它是通过某种方式创建或获取的
        PlayerDataConfig.OnDataChanged += UpdateSingleItem;
        UpdateInitialUI();
    }
    private void UpdateInitialUI()
    {
        // 更新所有 UI 元素，确保它们显示当前值
        UpdateSingleItem("diamond");
        UpdateSingleItem("keyPurple");
        UpdateSingleItem("keyBlue");
    }

    private void OnDestroy()
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
        // 使用反射获取 PlayerDataConfig 中的最新值
        int newValue = (int)PlayerDataConfig.GetValue(fieldName);

        // 根据 fieldName 更新相应的 UI 元素
        switch (fieldName)
        {
            case "diamond":
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newValue.ToString();
                break;
            case "keyPurple":
                transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = newValue.ToString();
                break;
            case "keyBlue":
                transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = newValue.ToString();
                break;
        }
    }
}
