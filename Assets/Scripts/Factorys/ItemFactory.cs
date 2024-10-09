using MyBase;

namespace Factorys
{
    public class ItemFactory
    {
        public static ItemBase Create(string resName, int count = 1)
        {
            return resName switch
            {
                "keyBlue" => new ItemBase
                {
                    simpleName = "蓝钥匙",
                    resName = resName,
                    id = 501,
                    count = count,
                    level = 3,
                    description = "蓝钥匙，可以打开蓝色宝箱",
                },
                "keyPurple" => new ItemBase {
                    simpleName = "紫钥匙",
                    resName = resName,
                    id = 502,
                    count = count,
                    level = 4,
                    description = "紫钥匙，可以打开紫色宝箱",
                },
                
                _ => throw new System.NotImplementedException(),
            };

        }
        public static JewelBase Create(int id, int level, int placeId) {
            return new JewelBase(id, level, placeId, IdToJewelDesc(id, level));
        }
        private static string IdToJewelDesc(int id, int level) {
            return id switch
            {
                1 => $"攻击力加{level * 10}",
                2 => $"暴击率加{level * 1}%",
                3 => $"暴击伤害加{level * 10}%",
                4 => $"全输出加{level * 3}%",
                5 => $"火系伤害加{level * 4}%",
                6 => $"能量系伤害加{level * 4}%",
                7 => $"冰系伤害加{level * 4}%",
                8 => $"风系伤害加{level * 4}%",
                9 => $"物理伤害加{level * 4}%",
                10 => $"电系伤害加{level * 4}%",
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}