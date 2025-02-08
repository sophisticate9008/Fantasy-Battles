using System.Collections.Generic;

public static class MissionFactory
{

    private static readonly Dictionary<int, (string[] enemyTypes, float fixInterval, float noiseScale, float bloodRatio,
     float attackRatio, int A1_D, int mapId, int eliteIdx, int bossIdx)> missionConfigs = new()
    {
        { 0, (new[] { "Monster1","Monster2","Monster3" }, 2f, 0.8f, 1f, 1, 1, 0, 0, 0) },
        { 1, (new[] { "Monster1" }, 2f, 0.8f, 1f, 1, 1, 1, 0, 0) },
        { 2, (new[] { "Monster1" }, 2f, 0.8f, 1f, 1, 1, 2, 0, 0) },
    // Add more mission configurations as needed
    };

    public static MissionBase Create(int missionId)
    {
        if (missionConfigs.TryGetValue(missionId, out var config))
        {
            return new MissionBase(
                missionId,
                new List<string>(config.enemyTypes),
                config.fixInterval,
                config.noiseScale,
                config.bloodRatio,
                config.attackRatio,
                config.A1_D,
                config.mapId,
                config.eliteIdx,
                config.bossIdx
            );
        }

        throw new System.NotImplementedException($"Mission ID {missionId} is not configured.");
    }
}