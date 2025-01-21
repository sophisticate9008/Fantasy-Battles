using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitcherColor : PageSwitcher
{
    public override void ClickCallBack(int idx)
    {

        foreach (Button button in btns)
        {
            Color color = button.GetComponent<Image>().color;
            button.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        }
        Color selectedColor = btns[idx].GetComponent<Image>().color;
        btns[idx].GetComponent<Image>().color = new Color(selectedColor.r, selectedColor.g, selectedColor.b, 1);
    }

}