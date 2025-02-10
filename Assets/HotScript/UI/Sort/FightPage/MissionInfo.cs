
using UnityEngine.UI;

public class MissionInfo:TheUIBase {
    public Button monsterInfo;
    public Button rewards;
    private void Start() {
        AutoInjectFields();
        BindButton();
    }
    void BindButton() {
        monsterInfo.onClick.AddListener(ShowMonsterInfo);
        rewards.onClick.AddListener(ShowJewelProb);
    }

    void ShowMonsterInfo() {
        var types = MissionManager.Instance.mb.enemyTypes;
        string text = "";
        foreach (var type in types) {
            text += EnemyUtil.EnemyTypeToName(type) + ":\n";
            text += EnemyUtil.EnemyTypeToDes(type) + "\n";
            text += "\n";
        }
        UIManager.Instance.OnCommonUI("怪物信息", text);
    }
    void ShowJewelProb() {
        string text = "局内5级之后可获得1-5个宝石,其他奖励随等级提升,宝石概率如下:\n\n";
        text += ItemUtil.ProbDictToString(MissionManager.Instance.mb.JewelProbDict);
        UIManager.Instance.OnCommonUI("奖励信息", text);
    }
}