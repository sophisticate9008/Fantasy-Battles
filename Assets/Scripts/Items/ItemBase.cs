
using System;


namespace MyBase
{

    [Serializable]
    public class ItemBase
    {
        //资源名字
        public string resName;
        //中文名字
        public string simpleName;
        //过期时间
        public DateTime expireTime;
        //id 同质物品 比如效果同质的宝石 装备，前500预留给宝石
        public int id; 
        //堆叠之后的物品数量
        public int count;
        public string place;
        //品质等级
        public int level;
        //物品描述
        public string description;
        //宝石是否镶嵌
        public bool isEmbedded = false;
        //是否锁定
        public bool isLock = false;
        // 重写 == 运算符
        public static bool operator ==(ItemBase item1, ItemBase item2)
        {
            if (ReferenceEquals(item1, item2)) return true; // 同一引用
            if (item1 is null || item2 is null) return false; // 任一为 null

            // 除 count 外，其他属性必须都相等
            return item1.expireTime == item2.expireTime &&
                   item1.id == item2.id &&
                   item1.place == item2.place &&
                   item1.level == item2.level &&
                   item1.description == item2.description &&
                   item1.isEmbedded == item2.isEmbedded &&
                   item1.isLock == item2.isLock;
        }

        // 重写 != 运算符
        public static bool operator !=(ItemBase item1, ItemBase item2)
        {
            return !(item1 == item2);
        }

        // 重写 Equals 方法
        public override bool Equals(object obj)
        {
            if (obj is ItemBase item)
            {
                return this == item;
            }
            return false;
        }

        // 重写 GetHashCode 方法
        public override int GetHashCode()
        {
            return HashCode.Combine(expireTime, id, place, level, description, isEmbedded, isLock);
        }

    }
}