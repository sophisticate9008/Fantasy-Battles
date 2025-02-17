using TMPro;
using UnityEngine.UI;

public class SkillDes: TheUIBase{
    public ArmPropBase apb;
    public string des;
    public int needLevel;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI desText;
    public Image mask;

    private void Start() {
        AutoInjectFields();
        desText.text = des;
        levelText.text = needLevel.ToString();
        if(apb.level >= needLevel) {
            mask.gameObject.SetActive(false);
        }

    }
    public void InjectData(int needLevel, ArmPropBase apb,  string des) {
        this.des = des;
        this.needLevel = needLevel;
        this.apb = apb;
        
    }
}