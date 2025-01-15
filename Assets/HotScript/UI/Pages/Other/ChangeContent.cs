using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeContent : TheUIBase
{
    Button _button;
    public GameObject thePage; 
    private void Start() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ChangePage);

        
    }
    public void ChangePage()
    {
        List<Button> buttons = transform.parent.GetComponentsInDirectChildren<Button>();
        foreach (Button button in buttons) {
            button.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            button.enabled = true;
        }
        _button.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        foreach(Transform page in thePage.transform.parent) {
            page.gameObject.SetActive(false);
        }
        thePage.SetActive(true);
        _button.enabled = false;
    }

}