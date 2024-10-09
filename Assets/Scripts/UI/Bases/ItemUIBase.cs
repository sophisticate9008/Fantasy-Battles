using MyBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;



public class ItemUIBase : TheUIBase
{

    public ItemBase itemInfo;
    private string ResName => itemInfo.resName;
    public int Level => itemInfo.level;
    public int Count => itemInfo.count;
    public int PlaceId => itemInfo.placeId;
    public int Id => itemInfo.id;
    public override void Init()
    {
        string color = LevelUtil.LevelToColorString(Level);
        Sprite background = YooAssets.LoadAssetSync<Sprite>(color).AssetObject as Sprite;
        Prefab.GetComponent<Image>().sprite = background;
        Transform children = Prefab.transform.GetChild(0);
        children.GetComponent<Image>().sprite = YooAssets.LoadAssetSync<Sprite>(ResName).AssetObject as Sprite;
        children.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Count.ToString();
        if (Id < 500)
        {
            children = Prefab.transform.GetChild(1);
            children.GetComponent<Image>().sprite = YooAssets.LoadAssetSync<Sprite>("Place" + PlaceId).AssetObject as Sprite;
            children.GetComponent<Image>().gameObject.SetActive(true);
        }
        GetComponent<Button>().onClick.AddListener(ShowDes);
    }
    public void ShowDes()
    {
        GameObject desPrefab = YooAssets.LoadAssetSync("Des").AssetObject as GameObject;
        DesUIBase des = Instantiate(desPrefab).AddComponent<DesUIBase>();
        des.itemInfo = itemInfo;
        des.Init();
        UIManager.Instance.ShowUI(des);
    }



}



