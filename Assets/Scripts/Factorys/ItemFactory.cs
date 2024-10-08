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
    }
}