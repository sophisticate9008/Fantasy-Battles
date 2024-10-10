using System;
using System.Collections.Generic;
using System.Linq;
using Factorys;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class Bag : TheUIBase
{
    private readonly List<JewelBase> newJewels = new();
    GameObject itemUIPrefab;
    private void BindButton()
    {
        Button upgrade = transform.RecursiveFind("Upgrade").GetComponent<Button>();
        upgrade.onClick.RemoveAllListeners();
        upgrade.onClick.AddListener(UpgradeJewel);
    }
    private void OnEnable()
    {
        itemUIPrefab = YooAssets.LoadAssetSync("ItemBase").AssetObject as GameObject;
        BindButton();
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
        PlayerDataConfig.jewels.Sort((jewel1, jewel2) =>
        {
            // 先比较等级，降序
            int levelComparison = jewel2.level.CompareTo(jewel1.level);

            if (levelComparison != 0)
            {
                return levelComparison;
            }

            // 如果等级相同，比较 placeId，降序
            int placeIdComparison = jewel2.placeId.CompareTo(jewel1.placeId);

            if (placeIdComparison != 0)
            {
                return placeIdComparison;
            }

            // 如果 placeId 也相同，比较 count，降序
            return jewel2.count.CompareTo(jewel1.count);
        });
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
        parent.ClearChildren();
        itemUIPrefab = YooAssets.LoadAssetSync("ItemBase").AssetObject as GameObject;
        foreach (var jewel in PlayerDataConfig.jewels)
        {
            JewelUIBase itemUI = Instantiate(itemUIPrefab).AddComponent<JewelUIBase>();
            itemUI.itemInfo = jewel;
            itemUI.Init();
            itemUI.transform.SetParent(parent);
        }
    }
    private void UpgradeJewel()
    {
        newJewels.Clear();
        //相同位置相同等级
        Dictionary<(int level, int placeId), List<JewelBase>> originJewelDict = new Dictionary<(int, int), List<JewelBase>>();
        foreach (var jewel in PlayerDataConfig.jewels)
        {
            // 创建联合键 (level, placeId)
            var key = (jewel.level, jewel.placeId);
            // 如果字典中已存在该键，则将宝石加入对应列表
            if (originJewelDict.TryGetValue(key, out List<JewelBase> jewelList))
            {
                //锁定的跳过
                if (!jewel.isLock)
                {
                    jewelList.Add(jewel);
                }

            }
            else
            {
                // 否则创建新的列表并添加到字典中
                originJewelDict[key] = new List<JewelBase> { jewel };
            }
        }
        // 遍历字典，找到符合升级条件的宝石，数量总和大于五进一步处理
        List<JewelBase> ConsumeList = new();
        foreach (var kvp in originJewelDict)
        {
            List<JewelBase> jewelsToUpgrade = kvp.Value;
            int totalCount = jewelsToUpgrade.Sum(jewel => jewel.count);

            if (totalCount >= 5)
            {
                // 先预览
                ConsumeList.AddRange(HandleList(jewelsToUpgrade, totalCount));
            }
        }
        GenerateUpgradeUI(ConsumeList);

    }
    private List<JewelBase> HandleList(List<JewelBase> originList, int totalCount)
    {
        List<JewelBase> backupList = new();
        for (int i = 0; i < originList.Count; i++)
        {
            backupList.Add(originList[i].Clone());
        }
        //多出来的
        int restCount = totalCount % 5;
        int newCount = totalCount / 5;
        for (int i = 0; i < newCount; i++)
        {
            int id = UnityEngine.Random.Range(1, Constant.JewelMaxId + 1);
            // 创建新宝石，默认等级为1，位置为1
            JewelBase newJewel = ItemFactory.Create(id, originList[0].level + 1, originList[0].placeId);
            // 将新宝石添加到列表中
            newJewels.Add(newJewel);
        }
        //倒着去除
        for (int i = backupList.Count - 1; i >= 0; i--)
        {
            Debug.Log("" + backupList[i].count);
            //大于restCount
            if (backupList[i].count > restCount)
            {
                backupList[i].count -= restCount;
                break;
            }
            else if (backupList[i].count == restCount)
            {
                backupList.RemoveAt(i);
                break;
            }
            else
            {
                restCount -= backupList[i].count;
                backupList.RemoveAt(i);
            }
        }
        return backupList;
    }
    private void ConfirmUpgrade(List<JewelBase> ConsumeList)
    {
        foreach (JewelBase jewel in PlayerDataConfig.jewels.ToList())
        {
            foreach (JewelBase consumeJewel in ConsumeList)
            {
                if (jewel == consumeJewel)
                {
                    jewel.count -= consumeJewel.count;
                    if (jewel.count <= 1)
                    {
                        PlayerDataConfig.jewels.Remove(jewel);
                    }
                    break;
                }
            }
        }
        PlayerDataConfig.jewels.AddRange(newJewels);
        PlayerDataConfig.SaveConfig();
        GenerateUpgradeUI(newJewels, 1);
        newJewels.Clear();
        MergeJewel();

    }
    private void GenerateUpgradeUI(List<JewelBase> jewelList, int mode = 0)
    {
        if (jewelList.Count == 0)
        {
            UIManager.Instance.OnMessage("没有升级条件");
            return;
        }
        GameObject upgradeJewel = YooAssets.LoadAssetSync("UpgradeJewel").AssetObject as GameObject;
        TheUIBase theUIBase = Instantiate(upgradeJewel).AddComponent<TheUIBase>();
        Transform parent = theUIBase.transform.RecursiveFind("Content");
        foreach (JewelBase jewel in jewelList)
        {
            JewelUIBase itemUI = Instantiate(itemUIPrefab).AddComponent<JewelUIBase>();
            itemUI.itemInfo = jewel;
            itemUI.Init();
            itemUI.transform.SetParent(parent);
        }
        if (mode == 0)
        {
            Action action = () =>
            {
                UIManager.Instance.CloseUI();
                ConfirmUpgrade(jewelList);
            };
            UIManager.Instance.OnCommonUI("消耗以下宝石", theUIBase, action);
        }
        else
        {
            UIManager.Instance.OnCommonUI("获得以下宝石", theUIBase);
        }

    }
}
