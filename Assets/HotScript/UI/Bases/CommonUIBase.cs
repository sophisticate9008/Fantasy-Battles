

using UnityEngine;
using UnityEngine.UI;

public class CommonUIBase : TheUIBase
{
    public string title;
    public string innerText;
    private Text titleUI;
    private GameObject inner;
    private Text innerTextUI;

    private void FindNecessary() {
        titleUI = transform.RecursiveFind("Title").GetComponent<Text>();
        inner = transform.RecursiveFind("Inner").gameObject;
        innerTextUI = inner.transform.RecursiveFind("InnerText").GetComponent<Text>();
    }
    
    public void ReplaceInner(TheUIBase ui) {
        ui.transform.CopyRectTransform(inner.transform);
        ui.transform.SetParent(inner.transform.parent, false);
        Destroy(inner);
    }

    public override void Init()
    {
        FindNecessary();
        titleUI.text = title;
        innerTextUI.text = innerText;
    }
}


