using System.Collections.Generic;

public static class MissionFactory
{
    private static readonly Dictionary<int, (string[] enemyTypes, float fixInterval, float noiseScale, float bloodRatio, float attackRatio, int A1_D, int mapId)> missionConfigs = new()
    {
        { 0, (new[] { "monster1" }, 6f, 0.8f, 1f, 1, 1, 5) },
        { 1, (new[] { "monster1" }, 6f, 0.8f, 1f, 1, 1, 5) },
        { 2, (new[] { "monster1" }, 6f, 0.8f, 1f, 1, 1, 5) },
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
                config.mapId
            );
        }

        throw new System.NotImplementedException($"Mission ID {missionId} is not configured.");
    }
}