


using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DesUIBase : TheUIBase
{
    public Color color;
    public ItemBase itemInfo;

    private TextMeshProUGUI simpleName;
    private TextMeshProUGUI desContent;
    public override void Init()
    {
        
        FindNecessary();
        simpleName.text = itemInfo.simpleName;
        desContent.text = itemInfo.description;
        ItemUtil.ChangeTextColor(simpleName.transform, itemInfo.level);
        ItemUtil.ChangeTextColor(desContent.transform, itemInfo.level);
        ItemUtil.SetSprite(transform.RecursiveFind("Pic"), itemInfo.resName);
    }
    public void FindNecessary() {
        simpleName = transform.RecursiveFind("SimpleName").GetComponent<TextMeshProUGUI>();
        desContent = transform.RecursiveFind("Content").GetComponent<TextMeshProUGUI>();
    }
}