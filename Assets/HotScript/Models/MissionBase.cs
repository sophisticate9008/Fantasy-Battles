using System.Collections.Generic;

public class MissionBase
{
    public int level;//关卡层
    public List<string> enemyTypes = new();
    public int MaxCount => A1_D * 10 * 11; // 最大生成数量
    public float fixInterval; // 固定时间间隔
    public float noiseScale; // 噪声比例
    public float bloodRatio;//血量比例
    public float attackRatio;
    public int A1_D;//初始怪物数量，等差数列，同时为为A1和d
    public int mapId;
    public MissionBase(int level, List<string> enemyTypes,
        float fixInterval, float noiseScale, float bloodRatio,
        float attackRatio, int A1_D, int mapId)
    {
        this.level = level;
        this.enemyTypes = enemyTypes;
        this.fixInterval = fixInterval;
        this.noiseScale = noiseScale;
        this.bloodRatio = bloodRatio;
        this.attackRatio = attackRatio;
        this.A1_D = A1_D;
        this.mapId = mapId;
    }
    public string LevelToName()
    {
        return "第" + level + "关";
    }
    public string MapIdToMapName()
    {
        return mapId switch
        {
            0 => "grass",
            1 => "road",
            2 => "city",
            3 => "day",
            4 => "night",
            5 => "circle",
            6 => "snow",
            7 => "desert",
            _ => throw new System.NotImplementedException(),
        };
    }

}