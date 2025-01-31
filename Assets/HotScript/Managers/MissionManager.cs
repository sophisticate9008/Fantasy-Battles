
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : ManagerBase<MissionManager>
{
    public List<ItemBase> items = new();
    //流程选择关卡时先获取一个记录的实例,战斗时全程携带，胜利后填充数据，然后保存该记录到通关记录中并序列化
    public PlayerDataConfig PlayerDataConfig { get => ConfigManager.Instance.GetConfigByClassName("PlayerData") as PlayerDataConfig; set { } }
    public MissionRecord mr;
    public MissionBase mb => MissionFactory.Create(mr.missionId);
    public int CurrentMaxPassId
    {
        get
        {
            // 检查 PassRecords 是否为 null 或为空
            if (PlayerDataConfig.PassRecords == null || PlayerDataConfig.PassRecords.Count == 0)
            {
                return 0;
            }

            // 返回字典中最大键值
            var maxMissionId = PlayerDataConfig.PassRecords.Max(record => record.missionId);
            return maxMissionId;

        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        mr = GetMissionRecordById(CurrentMaxPassId);
    }
    public MissionRecord GetMissionRecordById(int id)
    {
        foreach (var record in PlayerDataConfig.PassRecords)
        {
            if (record.missionId == id)
            {
                Debug.Log("已存在该关卡的通关记录");
                return record;
            }
        }

        MissionRecord newRecord = new(id);
        // mr = newRecord;
        return newRecord;//通关了才给记录到PassRecords

    }


    public void SaveRecord()
    {
        PlayerDataConfig.PassRecords[mr.missionId] = mr;
        PlayerDataConfig.SaveConfig();
    }

    public void GetRecord(MissionRecord record, int rewardId)
    {
        if (!record.isGetReward[rewardId] && record.successPercent >= 0.5 * rewardId)
        {
            record.isGetReward[rewardId] = true;
            PlayerDataConfig.SaveConfig();
        }
    }
    public void OnReward(int innerLevel)
    {

        Debug.Log("OnReward");
        items.Clear();
        // if (innerLevel < 5)
        // {
        //     return;
        // }
        int killCount = innerLevel * mb.A1_D + innerLevel * (innerLevel - 1) / 2 * mb.A1_D;
        int moneyGet = (int)(killCount * (0.5 + mb.level * 0.01));
        int diamondGet = (int)(killCount * (0.01 + mb.level * 0.001));
        int expGet = (int)(killCount * (0.3 + mb.level * 0.01));
        PlayerDataConfig.UpdateValueAdd("money", moneyGet);
        PlayerDataConfig.UpdateValueAdd("diamond", diamondGet);
        PlayerDataConfig.UpdateValueAdd("exp", expGet);
        items.Add(ItemFactory.Create("money", moneyGet));
        items.Add(ItemFactory.Create("diamond", diamondGet));
        items.Add(ItemFactory.Create("exp", expGet));
        GetJewel();
        GetArmChip(killCount);
        GetEquipmentChip(killCount);
        StartCoroutine(WaitSceneChange(() =>
        {
            UIManager.Instance.OnItemUIShow("获得奖励", items);
        }));
    }
    void GetJewel()
    {
        var dict = mb.JewelProbDict;
        int count = Random.Range(0, 6);
        for (int i = 0; i < count; i++)
        {
            int id = Random.Range(1, Constant.JewelMaxId + 1);
            int placeId = Random.Range(1, 7);
            int level = ItemUtil.GetRandomLevel(dict, 1);
            JewelBase jewelBase = ItemFactory.Create(id, level, placeId);
            PlayerDataConfig.jewels.Add(jewelBase);
            items.Add(jewelBase);
        }
        PlayerDataConfig.SaveConfig();
    }
    void GetArmChip(int killCount)
    {
        
        int chipSum = (int)(killCount * 0.1f);
        int n = 5;
        var ress = CommonUtil.DistributeRandomly(13, chipSum, n);
        Debug.Log(ress);
        foreach(var res in ress) {
            PlayerDataConfig.UpdateValueAdd("armChip" + res.id, res.n);
            items.Add(ItemFactory.CreateArmChip(res));
        }

    }
    void GetEquipmentChip(int killCount)
    {
        int chipSum = (int)(killCount * 0.05f);
        int n = 6;
        var ress = CommonUtil.DistributeRandomly(5, chipSum, n);
        foreach (var res in ress)
        {
            PlayerDataConfig.UpdateValueAdd("equipmentChip" + (res.id + 1), res.n);
            items.Add(ItemFactory.CreateEquipmentChip(res));
        }
    }
    IEnumerator WaitSceneChange(System.Action action)
    {
        Debug.Log("开始等待场景切换");
        string currentScene = SceneManager.GetActiveScene().name;

        while (SceneManager.GetActiveScene().name == currentScene)
        {
            yield return null; // 每帧检测
        }
        Debug.Log("场景切换完成");
        action.Invoke();
    }


}