

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Factorys;
using MyBase;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class Draw : ConsumeBase
{
    public enum ProbabilityType
    {
        ProbDict1, // 使用字典1
        ProbDict2  // 使用字典2
    }
    private Text guaranteeText;
    string guaranteeName;
    int minLevel;
    Dictionary<int, float> selectedDict;
    [SerializeField] private ProbabilityType probabilitySelection = ProbabilityType.ProbDict1; // 在 Inspector 面板中选择


    private readonly List<JewelBase> drawList = new();
    public override bool PostConsume()
    {
        DrawJewel();
        return base.PostConsume();

    }
    public override void Start()
    {
        base.Start();
        if (probabilitySelection == ProbabilityType.ProbDict1)
        {
            selectedDict = LevelUtil.probDictBlue;
            guaranteeName = "guaranteeBlue";
            minLevel = 3;
        }
        else
        {
            selectedDict = LevelUtil.probDictPurple;
            guaranteeName = "guaranteePurple";
            minLevel = 4;
        }
        guaranteeText = transform.parent.Find("Guarantee").GetComponent<Text>();
        PlayerDataConfig.OnDataChanged += UpdateGuaranteeText;
        UpdateGuaranteeText(guaranteeName);
    }
    private void OnDestroy()
    {
        // 移除事件监听，避免内存泄漏
        if (PlayerDataConfig != null)
        {
            PlayerDataConfig.OnDataChanged -= UpdateGuaranteeText;
        }
    }
    public void DrawJewel()
    {
        drawList.Clear();
        for (int i = 0; i < ConsumeCount; i++)
        {
            DrawJewelSingle();
        }

        StartCoroutine(GenerateUI());
    }
    private void UpdateGuaranteeText(string fieldName)
    {
        if (fieldName == guaranteeName)
        {
            guaranteeText.text = ReplaceNumber(guaranteeText.text, PlayerDataConfig.GetValue(fieldName).ToString());
        }
    }
    private string ReplaceNumber(string text, string newValue)
    {
        // 使用正则表达式匹配文本中的数字
        return Regex.Replace(text, @"(?<=<size=\d+><color=#FFD700>)(\d+)(?=</color></size>)", newValue);
    }
    public void DrawJewelSingle()
    {
        int guaranteeCount = (int)PlayerDataConfig.GetValue(guaranteeName);
        int level;
        if (guaranteeCount > 1)
        {
            PlayerDataConfig.UpdateValueSubtract(guaranteeName, 1);
            level = LevelUtil.GetRandomLevel(selectedDict);
        }
        else
        {
            PlayerDataConfig.UpdateValue(guaranteeName, 10);
            level = LevelUtil.GetRandomLevel(selectedDict, minLevel);
        }
        int id = Random.Range(1, Constant.JewelMaxId + 1);
        int placeId = Random.Range(1, 7);
        JewelBase jewelBase = ItemFactory.Create(id, level, placeId);
        drawList.Add(jewelBase);
        PlayerDataConfig.jewels.Add(jewelBase);
    }
    public IEnumerator GenerateUI()
    {
        GameObject drawPanelPrefab = YooAssets.LoadAssetSync("DrawPanel").AssetObject as GameObject;
        TheUIBase drawPanel = Instantiate(drawPanelPrefab).AddComponent<TheUIBase>();
        UIManager.Instance.ShowUI(drawPanel);
        GameObject itemBasePrefab = YooAssets.LoadAssetSync("ItemBase").AssetObject as GameObject;
        // ItemUIBase itemUI  = itemBasePrefab.AddComponent<ItemUIBase>();
        for (int i = 0; i < drawList.Count; i++)
        {
            
            ItemUIBase itemUI = Instantiate(itemBasePrefab).AddComponent<ItemUIBase>();
            itemUI.itemInfo = drawList[i];
            itemUI.Init();
            try {
                itemUI.transform.SetParent(drawPanel.transform.RecursiveFind("JewelContent"));
            }catch {
                Destroy(itemUI.gameObject);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }

}