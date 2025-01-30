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
    public int eliteIdx;
    public int bossIdx;
    public Dictionary<int, float> JewelProbDict
    {
        get
        {
            float p5 = 0.0001f + level * 0.00001f;
            float p4 = 0.006f + level * 0.0001f;
            float p3 = 0.01f + level * 0.001f;
            float p2 = 0.05f + level * 0.01f;
            float p1 = 1 - p5 - p4 - p3 - p2;
            return new() {
                        { 1,p1 },  // Level 1, 概率 60%
                        { 2, p2 },  // Level 2, 概率 30%
                        { 3, p3 },  // Level 3, 概率 9.5%
                        { 4, p4}, // Level 4, 概率 0.35%
                        { 5,p5 }  // Level 5, 概率 0.15%
            };
        }

    }
    public MissionBase(int level, List<string> enemyTypes,
        float fixInterval, float noiseScale, float bloodRatio,
        float attackRatio, int A1_D, int mapId, int eliteIdx, int bossIdx)
    {
        this.level = level;
        this.enemyTypes = enemyTypes;
        this.fixInterval = fixInterval;
        this.noiseScale = noiseScale;
        this.bloodRatio = bloodRatio;
        this.attackRatio = attackRatio;
        this.A1_D = A1_D;
        this.mapId = mapId;
        this.eliteIdx = eliteIdx;
        this.bossIdx = bossIdx;
    }
    public string LevelToName()
    {
        return "第" + (level + 1) + "关";
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