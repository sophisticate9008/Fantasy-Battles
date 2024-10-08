using MyBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace UIBase
{
    public class ItemUIBase : TheUIBase
    {

        public ItemBase itemInfo;
        private string ResName => itemInfo.resName;
        public int Level => itemInfo.level;
        public int Count => itemInfo.count;
        public string Place => itemInfo.place;
        public int Id => itemInfo.id;
        public override void Init()
        {
            string color = LevelToColor();
            Sprite background = YooAssets.LoadAssetSync<Sprite>(color).AssetObject as Sprite;
            Prefab.GetComponent<Image>().sprite = background;
            Transform children = Prefab.transform.GetChild(0);
            children.GetComponent<Image>().sprite = YooAssets.LoadAssetSync<Sprite>(ResName).AssetObject as Sprite;
            children.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Count.ToString();
            if(Id < 500) {
                children = Prefab.transform.GetChild(1);
                children.GetComponent<Image>().sprite = YooAssets.LoadAssetSync<Sprite>("Flag" + Place).AssetObject as Sprite;
            }
        }
        public string LevelToColor()
        {
            return Level switch
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


    }


}
