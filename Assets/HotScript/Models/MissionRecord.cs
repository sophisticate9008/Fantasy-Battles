using System;

[Serializable]
public class MissionRecord
{
    public bool[] isGetReward;

    public float successPercent;//成功的时候血量剩余百分比
    public int missionId;
    public float passTime;
    public MissionRecord(int id)
    {
        successPercent = 0;
        missionId = id;
        isGetReward = new bool[3] { false, false, false };
        passTime = 0;
    }
    public void Save()
    {
        FighteManager.Instance.PlayerDataConfig.PassRecords.Add(this);
        FighteManager.Instance.PlayerDataConfig.SaveConfig();
    }
    

}