


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class JewelUIBase : ItemUIBase
{
    public override void Init()
    {
        base.Init();
        IsEmbeded();
    }

    public void IsEmbeded()
    {
        List<JewelBase> placeList = (List<JewelBase>)PlayerDataConfig.GetValue("place" + itemInfo.placeId);
        foreach (JewelBase jewel in placeList)
        {
            if (itemInfo.id == jewel.id)
            {
                SetDiff(jewel.level);
                return;
            }
        }
    }

    public void SetDiff(int embedLevel)
    {
        Image bg = gameObject.GetComponent<Image>();
        Image jewel = transform.GetChild(0).GetComponent<Image>();
        GameObject upgrade = transform.RecursiveFind("Upgrade").gameObject;
        if(itemInfo.level <= embedLevel) {
            if (bg != null)
            {
                Color bgColor = bg.color;
                bgColor.a = 80f / 255f;  // 将 alpha 设置为 80/255
                bg.color = bgColor;
            }

            if (jewel != null)
            {
                Color jewelColor = jewel.color;
                jewelColor.a = 80f / 255f;  // 将 alpha 设置为 80/255
                jewel.color = jewelColor;
            }            
        }else {
            upgrade.SetActive(true);
        }

    }
    public override void ShowDes()
    {
        JewelHandleUIBase jewelHandle = GameObject.Find("PlayerPage").transform.RecursiveFind("JewelHandle").GetComponent<JewelHandleUIBase>();
        jewelHandle.itemInfo = itemInfo;
        jewelHandle.gameObject.SetActive(true);
        jewelHandle.Init();
        UIManager.Instance.OnListenedToclose(jewelHandle.gameObject, new string[] { "JewelUIBase" });
    }


}

