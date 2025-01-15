


using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DesUIBase : TheUIBase
{
    public Color color;
    public ItemBase itemInfo;

    private TextMeshProUGUI simpleName;
    private TextMeshProUGUI desContent;
    private Image bloom;
    public override void Init()
    {

        FindNecessary();
        simpleName.text = itemInfo.simpleName;
        desContent.text = itemInfo.description;
        ItemUtil.ChangeTextColor(simpleName.transform, itemInfo.level);
        ItemUtil.ChangeTextColor(desContent.transform, itemInfo.level);
        ItemUtil.SetSprite(transform.RecursiveFind("Pic"), itemInfo.resName);
        transform.RecursiveFind("Bloom").GetComponent<Image>().sprite =
            CommonUtil.GetAssetByName<Sprite>(ItemUtil.LevelToColorString(itemInfo.level) + "Border");

    }
    public void FindNecessary()
    {
        simpleName = transform.RecursiveFind("SimpleName").GetComponent<TextMeshProUGUI>();
        desContent = transform.RecursiveFind("Content").GetComponent<TextMeshProUGUI>();
        bloom = transform.RecursiveFind("Bloom").GetComponent<Image>(); ;
    }
}