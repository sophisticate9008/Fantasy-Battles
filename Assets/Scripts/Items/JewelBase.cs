
using System;

[Serializable]
public class JewelBase : ItemBase
{
    public JewelBase(int id, int level, int placeId, string description, int count = 1)
    {
        this.id = id;
        this.level = level;
        this.placeId = placeId;
        this.description = description;
        simpleName = ItemUtil.LevelToJewelSimpleName(level);
        resName = ItemUtil.LevelToJewelResName(level);
        this.count = count;
    }
    public JewelBase Clone()
    {
        
        return new JewelBase(id, level, placeId, description,count);
    }
}


