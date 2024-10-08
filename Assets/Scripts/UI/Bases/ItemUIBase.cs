using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace UIBase
{
    public class ItemUIBase : TheUIBase
    {


        public string resName;
        public int level;
        public int num = 1;
        public override void Init()
        {
            ResNameToLevel();
            string color = LevelToColor();
            Sprite background = YooAssets.LoadAssetSync<Sprite>(color).AssetObject as Sprite;
            Prefab.GetComponent<Image>().sprite = background;
            Transform children = Prefab.transform.GetChild(0);
            children.GetComponent<Image>().sprite = YooAssets.LoadAssetSync<Sprite>(resName).AssetObject as Sprite;
            children.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = num.ToString();
        }
        public string LevelToColor()
        {
            return level switch
            {
                1 => "gray",
                2 => "green",
                3 => "blue",
                4 => "purple",
                5 => "orange",
                6 => "red",
                7 => "muticolour",
                _ => throw new System.NotImplementedException(),
            };
        }
        public void ResNameToLevel()
        {
            if(PlayerDataConfig.ResNameToLevel.ContainsKey(resName))
            level = PlayerDataConfig.ResNameToLevel[resName];
        }


    }


}
