using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Constant
{
    public static readonly string ConfigsPath = Path.Combine(Application.persistentDataPath, "Configs");
    public static readonly string DataPath = Path.Combine(Application.persistentDataPath, "Data");
    public static readonly int JewelMaxId = 10;
    public static readonly int MissionMaxId = 2;
    public static Vector2 leftBottomViewBoundary = new(0, 0.2f);
    public static Vector2 rightTopViewBoundary = new(1, 1f);
    public static Vector2 leftBottomBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.2f, Camera.main.nearClipPlane)); // 设定左下角的边界
    public static Vector2 rightTopBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)); // 设定右上角的边界

    public static List<String> prefabFromScene = CommonUtil.AsList<String>(
            "ItemBase", "Message", "ExchangeBase", "Des", "DrawPanel", "CommonUI", "ItemUIShow", "Wash", "Tornado"
    );
    public static int diamondRewardBaseNum = 50;
    public static int goldRewardBaseNum = 500;
    public static int washWaterRewardBaseNum = 1;
    public static int rewardAdditon = 2;
}