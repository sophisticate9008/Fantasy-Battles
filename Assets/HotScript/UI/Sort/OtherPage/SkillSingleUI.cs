using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSingleUI : TheUIBase
{
    public string armType;
    public int level => (int)PlayerDataConfig.GetValue(ArmUtil.ArmTypeToLevelFieldName(armType));
    public ArmPropBase current;
    public ArmPropBase next;
    public int lastLevel;
    public int lastMoney;
    public Image ring;
    public Image icon;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI nameText;
    public GameObject redDot;
    public Button btn;
    public int chip => (int)PlayerDataConfig.GetValue(ArmUtil.ArmTypeToChipFieldName(armType));
    public int money => (int)PlayerDataConfig.GetValue("money");
    void FindNecessary()
    {
        ring = transform.RecursiveFind("Ring").GetComponent<Image>();
        icon = transform.RecursiveFind("Icon").GetComponent<Image>();
        levelText = transform.RecursiveFind("LevelText").GetComponent<TextMeshProUGUI>();
        nameText = transform.RecursiveFind("NameText").GetComponent<TextMeshProUGUI>();
        redDot = transform.RecursiveFind("RedDot").gameObject;
        btn = GetComponent<Button>();
    }
    void BindButton() {
        btn.onClick.AddListener(AwakePanel);
    }
    void AwakePanel() {
        SkillInfoPanel infoPanel = transform.parent.parent.RecursiveFind("InfoPanel").GetComponent<SkillInfoPanel>();
        infoPanel.currentArmProp = current;
        infoPanel.AwakePanel();
    }
    void UpdateSome() {
        nameText.text = current.armName;
        levelText.text = level.ToString();
        ring.fillAmount = Mathf.Min(1, (float)chip / current.chipNeed);
        icon.sprite = CommonUtil.GetAssetByName<Sprite>(current.resName);
    }
    void JudjeSatisfyUpgrade()
    {
        int needChipCount = current.chipNeed;
        lastMoney = money;
        int needMoneyCount = current.moneyNeed;
        if (chip >= needChipCount && needMoneyCount >= money)
        {
            redDot.SetActive(true);
        }
        else
        {
            redDot.SetActive(false);
        }
    }
    private void Start()
    {
        FindNecessary();
        BindButton();
    }
    public void GetProps()
    {
        lastLevel = level;
        current = new ArmPropBase(level, armType);
        next = new ArmPropBase(level + 1, armType);
    }
    
    public override void Init()
    {
        GetProps();
        JudjeSatisfyUpgrade();
        UpdateSome();
    }

    private void Update()
    {
        //监听升级
        if (level != lastLevel || money != lastMoney)
        {
            Init();
        }
    }
}