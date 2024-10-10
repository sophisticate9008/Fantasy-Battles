using System.Collections.Generic;
using MyBase;
using UIBase;
using UnityEngine;
using YooAsset;

public class Bag : TheUIBase
{


    private void OnEnable()
    {
        MergeJewel();
    }
    private void MergeJewel()
    {
        Debug.Log("MergeJewel");
        List<JewelBase> allJewels = PlayerDataConfig.jewels;
        Dictionary<JewelBase, JewelBase> jewelDict = new();

        foreach (var jewel in allJewels)
        {
            // 尝试在字典中查找相同的宝石
            if (jewelDict.TryGetValue(jewel, out JewelBase existingJewel))
            {
                // 如果字典中已经存在相同的宝石，则合并数量
                existingJewel.count += jewel.count;
            }
            else
            {
                // 否则将宝石加入字典
                jewelDict[jewel] = jewel;
            }
        }

        // 将合并后的宝石放入新的列表中
        List<JewelBase> newJewels = new(jewelDict.Values);

        // 更新玩家宝石列表
        PlayerDataConfig.jewels = newJewels;
        SortJewel();
        PlayerDataConfig.SaveConfig();
        GetJewelCount();
        GenerateJewelUI();

    }
    private void SortJewel()
    {
        PlayerDataConfig.jewels.Sort((jewel1, jewel2) => jewel2.level.CompareTo(jewel1.level));
    }
    private int GetJewelCount()
    {
        int count = 0;
        foreach (var jewel in PlayerDataConfig.jewels)
        {
            count += jewel.count;
        }
        Debug.Log("宝石总数量" + count);
        return count;
    }
    private void GenerateJewelUI()
    {
        Transform parent = transform.RecursiveFind("JewelContent");
        GameObject ItemUIPrefab = YooAssets.LoadAssetSync("ItemBase").AssetObject as GameObject;
        foreach (var jewel in PlayerDataConfig.jewels)
        {
            JewelUIBase itemUI = Instantiate(ItemUIPrefab).AddComponent<JewelUIBase>();
            itemUI.itemInfo = jewel;
            itemUI.Init();
            itemUI.transform.SetParent(parent);
        }
    }
}
