using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class FightPage : TheUIBase
{

    public Button next;
    public Button prev;
    public PageSwitcher ps;
    public MissionBase mb => MissionManager.Instance.mb;
    public MissionRecord mr => MissionManager.Instance.mr;
    public int cmpi => MissionManager.Instance.CurrentMaxPassId;
    public TextMeshProUGUI LevelName;
    public Transform progressBar;
    public Transform rewards;
    public TextMeshProUGUI percentText;
    public Button BeginButton;
    private void Start()
    {
        FindNecessary();
        BindButton();
        LoadMapImage(0);
        ChangeNeed();
    }
    void FindNecessary()
    {
        prev = transform.RecursiveFind("上一关").GetComponent<Button>();
        next = transform.RecursiveFind("下一关").GetComponent<Button>();
        ps = transform.RecursiveFind("maps").GetComponent<PageSwitcher>();
        LevelName = transform.RecursiveFind("LevelName").GetComponent<TextMeshProUGUI>();
        progressBar = transform.RecursiveFind("进度条");
        rewards = transform.RecursiveFind("宝箱");
        percentText = transform.RecursiveFind("百分比").GetComponent<TextMeshProUGUI>();
        BeginButton = transform.RecursiveFind("开始").GetComponent<Button>();
    }
    void BindButton()
    {
        BeginButton.onClick.AddListener(BeginFight);
        prev.onClick.AddListener(PreLevel);
        next.onClick.AddListener(NextLevel);
    }
    void BeginFight()
    {
        var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
        YooAssets.LoadSceneAsync("Fight", sceneMode);
    }
    public void PreLevel()
    {
        if (mb.level == 0)
        {
            UIManager.Instance.OnMessage("已经是第一关了");
            return;
        }
        SwitchLevel(mb.level - 1);
        ps.PreviousPage();
        ChangeNeed();
    }
    public void NextLevel()
    {
        if (mb.level > cmpi)
        {
            UIManager.Instance.OnMessage("新一关还未解锁");
            return;
        }
        if(mb.level >= Constant.MissionMaxId) {
            UIManager.Instance.OnMessage("最后一关了");
            return;
        }
        SwitchLevel(mb.level + 1);
        ps.NextPage();
        ChangeNeed();

    }
    public void SwitchLevel(int id)
    {
        MissionManager.Instance.mr = MissionManager.Instance.GetMissionRecordById(id);
        ChangeNextImage();
    }

    void LoadMapImage(int childId)
    {
        ps.transform.GetChild(childId).GetComponent<Image>().sprite = CommonUtil.GetAssetByName<Sprite>("map_" + mb.MapIdToMapName());
    }
    void ChangeNextImage()
    {
        var npi = (ps.currentPageIndex + 1) % 2;
        LoadMapImage(npi);
    }
    void ChangeNeed()
    {
        RecoverRewardsUI();
        LevelName.text = mb.LevelToName();
        percentText.text = ((int)(mr.successPercent * 100)) + "%";
        UpdateRewards();
        UpdateProgressBar();

    }
    void RecoverRewardsUI()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform reward = rewards.GetChild(i);
            reward.GetChild(0).gameObject.SetActive(false); // 未领取状态
            reward.GetChild(1).gameObject.SetActive(false);  // 已领取状态
        }
    }
    void UpdateRewards()
    {
        int rewardCount = 0;
        // 遍历范围
        if (mr.successPercent > 0 && mr.successPercent < 0.5f)
        {
            rewardCount = 1;
        }
        else if (mr.successPercent >= 0.5f && mr.successPercent < 1f)
        {
            rewardCount = 2;
        }
        else if (mr.successPercent == 1f)
        {
            rewardCount = 3;
        }
        for (int i = 0; i < rewardCount; i++)
        {
            UpdateRewardState(i, mr.isGetReward[i]);
        }
    }
    void UpdateRewardState(int rewardIndex, bool isGetReward)
    {
        Transform reward = rewards.GetChild(rewardIndex);

        // 获取奖励逻辑
        reward.GetChild(0).gameObject.SetActive(!isGetReward); // 未领取状态
        reward.GetChild(1).gameObject.SetActive(isGetReward);  // 已领取状态
        if (!isGetReward)
        {
            Button b = reward.GetChild(0).GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() =>
            {
                GetReward(rewardIndex);
            });
        }
    }
    void UpdateProgressBar()
    {
        foreach (Transform t in progressBar.transform)
        {
            t.GetComponent<Image>().fillAmount = mr.successPercent;
        }
    }

    void GetReward(int index)
    {
        int diamondNum = Constant.diamondRewardBaseNum * (index + Constant.rewardAdditon);
        int goldRewardBaseNum = Constant.goldRewardBaseNum * (index + Constant.rewardAdditon);
        int washWaterRewardBaseNum = Constant.washWaterRewardBaseNum * (index + Constant.rewardAdditon);
        mr.isGetReward[index] = true;
        PlayerDataConfig.SaveConfig();
        List<(string resName, int count)> rewards = new()
        {
            ("diamond", diamondNum),
            ("money", goldRewardBaseNum),
            ("washWater", washWaterRewardBaseNum)
        };
        ToolManager.Instance.GetReward(rewards);
        UpdateRewards();

    }
}