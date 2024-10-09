
using System;

namespace MyBase
{
    [Serializable]
    public class JewelBase : ItemBase
    {
        public JewelBase(int id, int level, int placeId, string description)
        {
            this.id = id;
            this.level = level;
            this.placeId = placeId;
            this.description = description;
            simpleName = LevelUtil.LevelToJewelSimpleName(level);
            resName = LevelUtil.LevelToJewelResName(level);
            count = 1;
        }
    }
}

