using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillPage : TheUIBase
{
    private GameObject skillPrefab;
    private Transform parent;
    private List<string> allArmTypes;
    public TextMeshProUGUI critDamage;

    private void Start()
    {
        PlayerDataConfig.OnDataChanged += UpdateCritDamage;
        AutoInjectFields();
        skillPrefab = CommonUtil.GetAssetByName<GameObject>("SkillSingle");
        allArmTypes = ArmUtil.AllArmTypes;
        Debug.Log("armtypes");
        parent = transform.RecursiveFind("技能列表");
        InitSkillUI();
        UpdateCritDamage("levelArm");
    }
    void InitSkillUI()
    {
        foreach (var armType in allArmTypes)
        {

            GameObject clone = Instantiate(skillPrefab, parent);
            SkillSingleUI skillSingleUI = clone.AddComponent<SkillSingleUI>();
            skillSingleUI.armType = armType;
            clone.SetActive(true);
        }
    }
    void UpdateCritDamage(string fieldName)
    {
        if (fieldName.Contains("levelArm"))
        {
            critDamage.text = $"每升一级获得2%爆伤\n当前总爆伤{PlayerDataConfig.AllLevelArm * 2}%";
        }

    }

}