using System.Collections.Generic;

public static class MissionFactory
{
    public static MissionBase Create(int missionId)
    {
        return missionId switch
        {
            0 => new MissionBase(missionId, new List<string> { "monster1" }, 6f, 0.8f, 1, 1, 5, 0),
            1 => new MissionBase(missionId, new List<string> { "monster1" }, 6f, 0.8f, 1, 1, 5, 0),
            2 => new MissionBase(missionId, new List<string> { "monster1" }, 6f, 0.8f, 1, 1, 5, 0),
            
            _ => throw new System.NotImplementedException(),
        };
    }
}