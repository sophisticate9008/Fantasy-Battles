public class MissionManager : ManagerBase<MissionManager>
{
    //流程选择关卡时先获取一个记录的实例,战斗时全程携带，胜利后填充数据，然后保存该记录到通关记录字典中并序列化
    public MissionRecord currentRecord;
    public MissionPassRecordConfig mprc;
    private void Start()
    {
        mprc = ConfigManager.Instance.GetConfigByClassName("MissionPassRecord") as MissionPassRecordConfig;
    }
    public MissionRecord GetMissionRecordById(int id)
    {
        if (mprc.PassRecords.ContainsKey(id))
        {
            return mprc.PassRecords[id];
        }
        else
        {
            MissionRecord newRecord = new(id);
            currentRecord = newRecord;
            return newRecord;//通关了才给记录到PassRecords
        }
    }


    public void SaveRecord()
    {
        mprc.PassRecords[currentRecord.missionId] = currentRecord;
        mprc.SaveConfig();
    }

    public void GetRecord(MissionRecord record, int rewardId) {
        if(!record.isGetReward[rewardId] && record.successPercent >=  0.5 * rewardId){
            record.isGetReward[rewardId] = true;
            mprc.SaveConfig();
        }
    }

}