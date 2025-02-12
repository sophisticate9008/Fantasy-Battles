using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class ConsumeBase : TheUIBase
{
    public Action afterAction;
    // 使用元组 (string, int) 来存储物品名称和对应的消耗数量
    public List<(string itemName, int consumeCount)> consumeItemsData = new();
    public virtual List<(string itemName, int consumeCount)> ConsumeItemsData {
        get { return consumeItemsData; }
        set { consumeItemsData = value; }
    }
    public virtual void PreConsume()
    {
        // 检查所有物品的消耗条件
        foreach (var itemData in ConsumeItemsData.ToList())
        {
            if ((int)PlayerDataConfig.GetValue(itemData.itemName) < itemData.consumeCount)
            {
                // 如果任何物品不足，显示不足的消息并退出
                UIManager.Instance.OnMessage(ItemUtil.VarNameToSipleName(itemData.itemName) + "不足");
                return;
            }
        }

        // 所有物品都符合条件，统一进行消耗
        if (PostConsume())
        {
            AfterConsume();
        }
    }

    // 统一消耗前的逻辑检查
    public virtual bool PostConsume()
    {
        return true; // 可扩展预处理逻辑
    }

    // 统一进行消耗逻辑
    public virtual void AfterConsume()
    {
        foreach (var (itemName, consumeCount) in ConsumeItemsData.ToList())
        {
            Debug.Log($"消耗 {itemName} {consumeCount}");
            PlayerDataConfig.UpdateValueSubtract(itemName, consumeCount);
        }
        // 可扩展消耗后的其他逻辑
        afterAction?.Invoke();
    }

    public virtual void Start()
    {
        BindButton();
    }

    public virtual void BindButton()
    {
        GetComponent<Button>().onClick.AddListener(PreConsume);
    }
}
