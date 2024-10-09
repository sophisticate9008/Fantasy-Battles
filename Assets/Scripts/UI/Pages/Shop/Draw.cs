

using System.Collections;
using System.Collections.Generic;
using Factorys;
using MyBase;
using UIBase;
using UnityEngine;
using YooAsset;

public class Draw : ConsumeBase
{

    private readonly List<JewelBase> drawList = new();
    public override bool PostConsume()
    {
        DrawJewel();
        return base.PostConsume();

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

    public void DrawJewelSingle()
    {

        int id = Random.Range(1, Constant.JewelMaxId + 1);
        int level = Random.Range(1, 8);
        int placeId = Random.Range(1, 7);
        JewelBase jewelBase = ItemFactory.Create(id, level, placeId);
        drawList.Add(jewelBase);
        // itemUI.itemInfo = jewelBase;
        // itemUI.Init();
        // itemUI.transform.SetParent(drawPanel.transform);
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
            itemUI.transform.SetParent(drawPanel.transform.RecursiveFind("JewelContent"));
            yield return new WaitForSeconds(0.1f);
        }
    }
}