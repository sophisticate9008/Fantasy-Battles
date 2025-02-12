using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPanel : ConsumeBase
{
    public ArmPropBase lastArmProp;
    public ArmPropBase currentArmProp;
    public ArmPropBase nextArmProp;

    public List<string> propsFieldName = new() {
        "damageType", "attackCd", "duration", "rangeFire", "tlc", "cd", "penetration", "damagePos"
    };
    public Dictionary<string, string> propsName = new() {
        {"damageType", "类型"},
        {"attackCd", "间隔"},
        {"duration", "持续"},
        {"rangeFire", "范围"},
        {"tlc", "倍率"},
        {"cd", "冷却"},
        {"penetration", "穿透"},
        {"damagePos", "索敌"},
    };

    public Dictionary<string, string> typeName = new() {
        {"fire", "火"},
        {"ice", "冰"},
        {"elec", "电"},
        {"ad", "物理"},
        {"wind", "风"},
        {"energy", "能量"},
    };
    public List<TextMeshProUGUI> propTexts;
    public TextMeshProUGUI nameText;
    public Image icon;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI desText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI chipText;
    public Button confirmBtn;
    public Button closeBtn;
    public Image chipImg;
    void FindNecessary()
    {
        foreach (var prop in propsFieldName)
        {
            propTexts.Add(transform.RecursiveFind(prop).GetComponent<TextMeshProUGUI>());
        }
        AutoInjectFields();

    }
    public override void BindButton()
    {
        confirmBtn.onClick.AddListener(PreConsume);
        closeBtn.onClick.AddListener(Close);
    }
    public override bool PostConsume()
    {
        PlayerDataConfig.UpdateValueAdd(currentArmProp.levelFieldName, 1);
        currentArmProp = new(currentArmProp.level + 1, currentArmProp.armtype);
        return base.PostConsume();
    }
    public override void Start()
    {
        FindNecessary();
        BindButton();
    }
    public override void Init()
    {
        lastArmProp = currentArmProp;
        nextArmProp = new ArmPropBase(currentArmProp.level + 1, currentArmProp.armtype);
        ConsumeItemsData.Clear();
        ConsumeItemsData.Add(("money", currentArmProp.moneyNeed));
        ConsumeItemsData.Add((currentArmProp.chipFieldName, currentArmProp.chipNeed));
        InitProps();
        InitOther();
    }
    void InitOther()
    {
        int moneyNeed = currentArmProp.moneyNeed;
        moneyText.text = ToolManager.Instance.GenerateNeedCountText("money", moneyNeed);
        chipText.text = ToolManager.Instance.GenerateNeedCountText(currentArmProp.chipFieldName, currentArmProp.chipNeed);
        chipImg.sprite = CommonUtil.GetAssetByName<Sprite>(currentArmProp.chipResName);
        icon.sprite = CommonUtil.GetAssetByName<Sprite>(currentArmProp.resName);
        levelText.text = currentArmProp.level.ToString();
        desText.text = currentArmProp.des;
        nameText.text = currentArmProp.armName;
    }
    void InitProps()
    {
        for (int i = 0; i < propsFieldName.Count; i++)
        {
            string fieldName = propsFieldName[i];
            string otherStr = fieldName == "tlc" || fieldName == "cd"
                ? $"-><color=#259645>{nextArmProp.GetFieldValue(fieldName)}</color>" : "";
            propTexts[i].text = propsName[fieldName] + ":"
                + ReplaceText(currentArmProp.GetFieldValue(fieldName).ToString()) + otherStr;

        }
    }
    void Close()
    {
        CloseToDirection("down", 0.3f);
    }
    public void AwakePanel()
    {
        OpenFromDirection("down", 0.3f);
    }
    string ReplaceText(string text)
    {
        if (typeName.ContainsKey(text))
        {
            return typeName[text];
        }
        else
        {
            return text;
        }
    }
    private void Update()
    {
        if (lastArmProp != currentArmProp)
        {
            Init();
        }
    }

}