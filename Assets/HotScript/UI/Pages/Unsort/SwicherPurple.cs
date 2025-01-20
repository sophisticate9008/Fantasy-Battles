using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitcherPurple : PageSwitcher
{
    public override void StartCallBack()
    {
        btns[0].onClick.Invoke();
    }
    public override void ClickCallBack(int idx)
    {

        foreach (Button button in btns) {
            button.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        btns[idx].GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

}