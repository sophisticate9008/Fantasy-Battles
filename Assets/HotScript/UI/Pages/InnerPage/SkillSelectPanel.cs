using System.Collections.Generic;
using MyEnums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class SkillSelectPanel : TheUIBase
{
    List<Button> Buttons => transform.GetChild(0).GetComponentsInDirectChildren<Button>();
    public List<SkillNode> skills;
    public TextMeshProUGUI textMeshProUGUI;
    private string title;
    public SkillPanelMode mode;
    public Button confirmBtn;
    public override void Init()
    {

        if (mode == SkillPanelMode.Select)
        {
            title = "选择一个技能";
            for (int i = 0; i < skills.Count; i++)
            {
                int idx = i;
                Buttons[idx].onClick.AddListener(() => SelectSkill(skills[idx]));
            }
        }
        else
        {
            title = "获得三个技能";
            confirmBtn.gameObject.SetActive(true);
            confirmBtn.onClick.AddListener(() =>
            {
                FighteManager.Instance.ControlGame(true);
                Destroy(gameObject);
            });
        }

        textMeshProUGUI.text = title;
        ChangeDetail();
    }
    private void SelectSkill(SkillNode skill)
    {
        SkillManager.Instance.SelectSkill(skill);
        FighteManager.Instance.ControlGame(true);
        Destroy(gameObject);
    }
    void ChangeDetail()
    {
        int idx = 0;
        foreach (var b in Buttons)
        {
            SkillNode skill = skills[idx++];
            Text skillName = b.transform.RecursiveFind("SkillName").GetComponent<Text>();
            Text desc = b.transform.RecursiveFind("Desc").GetComponent<Text>();
            Image icon = b.transform.RecursiveFind("Icon").GetComponent<Image>();
            Sprite iconSprite = CommonUtil.GetAssetByName<Sprite>(skill.resName);
            skillName.text = skill.skillName;
            desc.text = skill.desc;
            icon.sprite = iconSprite;
        }
    }
}