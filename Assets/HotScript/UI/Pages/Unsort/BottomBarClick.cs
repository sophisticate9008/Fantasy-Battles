using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBarClick : PageSwitcher
{
    // 存储所有子对象的 Animator
    private List<Animator> childAnimators;

    public override void StartCallBack() {
        childAnimators = btnsParent.GetComponentsInDirectChildren<Animator>();
        foreach (var animator in childAnimators)
        {
            animator.Rebind();
            animator.enabled = false;
        }
        float tmp = transitionDuration;
        transitionDuration = 0;
        btns[2].onClick.Invoke();
        transitionDuration = tmp;
    }
    // 当按钮被点击时执行的逻辑
    public override void ClickCallBack(int idx)
    {
        // 遍历所有子对象，关闭其他动画，只开启点击的那个动画
        for (int i = 0; i < childAnimators.Count; i++)
        {
            if (i == idx)
            {
                childAnimators[i].enabled = true;
            }
            else
            {
                childAnimators[i].Rebind();
                childAnimators[i].enabled = false; // 关闭其他动画
            }
        }
    }
}
