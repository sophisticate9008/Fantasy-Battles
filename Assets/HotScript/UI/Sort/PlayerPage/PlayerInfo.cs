using TMPro;
using UnityEngine.UI;

public class PlayerInfo: TheUIBase {
    public Image innerBar;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI attackText;
    public Button role;
    public Button sward;
    public Button prop;

    private void Start() {
        AutoInjectFields();
        BindButton();
        UpdateBar("exp");
        PlayerDataConfig.OnDataChanged += UpdateBar;
    }

    void UpdateBar(string fieldName) {
        if(fieldName == "exp" || fieldName == "jewelChange") {
            progressText.text = PlayerDataConfig.ExpCurrent + "/" + PlayerDataConfig.ExpNeed;
            levelText.text = PlayerDataConfig.PlayerLevel.ToString();
            GlobalConfig.Init();
            GlobalConfig.LoadJewel();
            attackText.text = "攻击力:" + GlobalConfig.AttackValue;
        }
    }
    
    void BindButton() {
        prop.onClick.AddListener(ShowProp);
        sward.onClick.AddListener(UpgradeEuqipmentPanel);
        
    }
    void UpgradeEuqipmentPanel() {

    }
    void ShowProp() {
        UIManager.Instance.OnCommonUI("宝石总览", GlobalConfig.MergeJewelDes());
    }

}