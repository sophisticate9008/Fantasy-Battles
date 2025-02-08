using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitcherBottom : PageSwitcher
{
    // 存储所有子对象的 Animator
    private List<Animator> childAnimators;

    // 当按钮被点击时执行的逻辑
    public override void ClickCallBack(int idx)
    {
        // 遍历所有子对象，关闭其他动画，只开启点击的那个动画
        for (int i = 0; i < btns.Count; i++)
        {
            if (i == idx)
            {
                btns[i].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                btns[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
