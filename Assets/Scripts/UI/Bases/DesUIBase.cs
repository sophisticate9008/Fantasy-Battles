
using MyBase;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class DesUIBase : TheUIBase
{
    public ItemBase itemInfo;

    private Text simpleName;
    private Text desContent;
    public override void Init()
    {
        
        FindNecessary();
        simpleName.text = itemInfo.simpleName;
        desContent.text = itemInfo.description;
        ItemUtil.ChangeMetrailColor(transform.RecursiveFind("Title"), itemInfo.level);
        ItemUtil.SetSprite(transform.RecursiveFind("Pic"), itemInfo.resName);
    }
    public void FindNecessary() {
        simpleName = transform.RecursiveFind("SimpleName").GetComponent<Text>();
        desContent = transform.RecursiveFind("Content").GetComponent<Text>();
    }
}