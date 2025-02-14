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

    // public static List<String> prefabFromScene = CommonUtil.AsList<String>(
    //         "ItemBase", "Message", "ExchangeBase", "Des", "DrawPanel", "CommonUI",
    //         "ItemUIShow", "Wash", "Tornado", "SkillSingle", "UIMask"
    // );
    public static int diamondRewardBaseNum = 50;
    public static int goldRewardBaseNum = 500;
    public static int washWaterRewardBaseNum = 1;
    public static int rewardAdditon = 2;
    public static (int a1, int d) upgradeMoneyNeed = (500, 100);
    public static (int a1, int d) upgradeChipNeed = (20, 5);
    public static (int a1, int d) equipmentAttack = (0, 10);
    public static (int a1, int d) upLevelExpNeed = (50,50);

    public static float moveOneAnimatorSpeed = 0.25f;//移动速度为0.25时动画播放速度为1;

}