using System.Collections.Generic;

[System.Serializable]
public class SkillNode
{
    public int id;
    public List<int> prerequisiteIds; // 前置技能的 ID
    public List<int> conflictIds; //冲突技能的Id
    public bool isSatisfied; // 是否满足前置
    public bool isUnlocked; // 是否解锁
    public int maxSelCount; //最大选择数量
    public string desc;//描述
    public string skillName;
    public string resName;
    public string armType;
    
    public SkillNode(int id, List<int> prerequisiteIds, List<int> conflictIds, string name, string desc, int maxSelCount, string resName, bool isUnlocked = false, bool isSatisfied = false,string armType = "MagicBullet")
    {
        this.conflictIds = conflictIds;
        this.id = id;
        this.prerequisiteIds = prerequisiteIds;
        this.isUnlocked = isUnlocked; // 默认未解锁
        this.isSatisfied = isSatisfied;
        this.desc = desc;
        this.maxSelCount = maxSelCount;
        skillName = name;
        this.resName = resName;
        this.armType = armType;
    }
}