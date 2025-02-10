
using System;

[Serializable]
public class JewelBase : ItemBase
{
    public override string simpleName => ItemUtil.LevelToJewelSimpleName(level);
    public override string resName => ItemUtil.LevelToJewelResName(level);
    public override string description => ItemUtil.IdLevelToJewelDesc(id, level);
    public JewelBase(int id, int level, int placeId, int count = 1)
    {
        this.id = id;
        this.level = level;
        this.placeId = placeId;
        this.count = count;
    }

    /// <summary>
    /// 克隆宝石，参数为是否克隆数量，否则为1
    /// </summary>
    public JewelBase Clone(bool isCloneCount = false)
    {
        int cloneCount = isCloneCount ? count : 1;
        return new JewelBase(id, level, placeId, cloneCount);
    }

    public void SubtractCount(int num)
    {
        count -= num;
        if (count < 1)
        {
            PlayerDataConfig.jewels.Remove(this);
        }
    }
}


