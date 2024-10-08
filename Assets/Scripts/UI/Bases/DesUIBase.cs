
using MyBase;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class DesUIBase : TheUIBase
{
    public ItemBase itemInfo;
    private Image pic;
    private Text simpleName;
    private Text desContent;
    private Material material;
    public override void Init()
    {
        
        FindNecessary();
        pic.sprite = YooAssets.LoadAssetSync<Sprite>(itemInfo.resName).AssetObject as Sprite;
        simpleName.text = itemInfo.simpleName;
        desContent.text = itemInfo.description;
        Material newMaterial = new Material(material);
        newMaterial.SetColor("_EndColor", Tool.LevelToColor(itemInfo.level));
        transform.RecursiveFind("Title").GetComponent<Image>().material = newMaterial;
    }
    public void FindNecessary() {
        pic = transform.RecursiveFind("Pic").GetComponent<Image>();
        simpleName = transform.RecursiveFind("SimpleName").GetComponent<Text>();
        desContent = transform.RecursiveFind("Content").GetComponent<Text>();
        material = transform.RecursiveFind("Title").GetComponent<Image>().material;
    }
}