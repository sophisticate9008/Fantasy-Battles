using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel: TheUIBase {
    private Button exitBtn;
    private Button continuebtn;
    private SortedDictionary<string,float> hs => FighteManager.Instance.harmStatistics;
    private SortedDictionary<string,int> ks => FighteManager.Instance.killStatistics;
    private Transform content;

    private void Start() {
        FindNecessary();
        Bind();
        ShowStatistics();
    }
    void FindNecessary() {
        exitBtn = transform.RecursiveFind("Exit").GetComponent<Button>();
        continuebtn = transform.RecursiveFind("Continue").GetComponent<Button> ();
        content = transform.RecursiveFind("Content");
    }

    void Bind() {
        continuebtn.onClick.AddListener(ContinueGame);
        exitBtn.onClick.AddListener(ExitGame);
    }
    void ContinueGame() {
        FighteManager.Instance.ControlGame(true);
        gameObject.SetActive(false);
    }
    void ExitGame() {
        FighteManager.Instance.EndGame(false);
    }
    void ShowStatistics() {
        int count = hs.Count;
        for(int i = 0; i< 5; i++) {
            if(i >= count) {
                content.GetChild(i).gameObject.SetActive(false);
            }else {
                ShowSingle(i);
            }
        }
    }

    void ShowSingle(int idx) {
        Transform bar = content.GetChild(idx);
        var dictItem = hs.ElementAt(idx);
        Debug.Log(dictItem);
        string resName = SkillUtil.ArmTypeToResName(dictItem.Key);
        bar.RecursiveFind("Icon").GetComponent<Image>().sprite = CommonUtil.GetAssetByName<Sprite>(resName);
        bar.RecursiveFind("Harm").GetComponent<TextMeshProUGUI>().text = dictItem.Value.ToString();
        int killCount = ks.TryGetValue(dictItem.Key, out int value) ? value : 0;
        bar.RecursiveFind("Kill").GetComponent<TextMeshProUGUI>().text = killCount.ToString();
        bar.gameObject.SetActive(true); 
    }
    
    private void OnEnable() {
        ShowStatistics();
    }
}