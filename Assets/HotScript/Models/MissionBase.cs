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

    public MissionBase()
    {
    
    }
    public string LevelToName() {
        return "第" + level + "关";
    }
}