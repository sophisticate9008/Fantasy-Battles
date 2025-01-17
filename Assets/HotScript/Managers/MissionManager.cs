
using System.Linq;
using UnityEngine;

public class MissionManager : ManagerBase<MissionManager>
{
    //流程选择关卡时先获取一个记录的实例,战斗时全程携带，胜利后填充数据，然后保存该记录到通关记录字典中并序列化
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


}