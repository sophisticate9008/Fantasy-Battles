using System.Collections.Generic;
using UnityEngine;

public class SkillPage: TheUIBase
{
    private GameObject skillPrefab;
    private Transform parent;
    private List<string> allArmTypes;
    
    private void Start() {
        skillPrefab = CommonUtil.GetAssetByName<GameObject>("SkillSingle");
        allArmTypes = ArmUtil.AllArmTypes;
        Debug.Log("armtypes");
        
        parent = transform.RecursiveFind("技能");
        foreach (var armType in allArmTypes) {
            GameObject clone = Instantiate(skillPrefab,parent);
            SkillSingleUI skillSingleUI = clone.AddComponent<SkillSingleUI>();
            skillSingleUI.armType = armType;
            clone.SetActive(true);
        }
    }


}