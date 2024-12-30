using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class SkillPanel :TheUIBase {
    List<string> SelectedArmType => SkillManager.Instance.SelectedArmTypes;
    List<Image> skills;
    Image MagicBullet;
    private void Start() {
        skills = GameObject.Find("Skills").transform.GetComponentsInDirectChildren<Image>();
        MagicBullet = skills[^1];
        
    }
    private void Update() {
        int idx = -1;
        foreach(var item in SelectedArmType) {
            idx++;
            if(idx == 0) {
                UpdateMagicBullet();
                //是枪，跳过处理
                continue;
            }
            var skill = skills[idx - 1];
            skill.gameObject.SetActive(true);
            skill.sprite = CommonUtil.GetAssetByName<Sprite>(SkillUtil.ArmTypeToResName(item));
            ArmConfigBase armConfigBase = ConfigManager.Instance.GetConfigByClassName(item) as ArmConfigBase;
            skill.transform.RecursiveFind("Mask").GetComponent<Image>().fillAmount = armConfigBase.CurrentCd / armConfigBase.Cd;
            skill.transform.RecursiveFind("Ring").GetComponent<Image>().fillAmount = armConfigBase.RestDuration / armConfigBase.Duration;
        }
    }
    private void UpdateMagicBullet() {
        ArmConfigBase armConfigBase = ConfigManager.Instance.GetConfigByClassName("MagicBullet") as ArmConfigBase;
        MagicBullet.transform.RecursiveFind("Mask").GetComponent<Image>().fillAmount = armConfigBase.CurrentCd / armConfigBase.Cd;
        MagicBullet.transform.RecursiveFind("Num").GetComponent<Text>().text = armConfigBase.AttackCount - armConfigBase.CurrentAttackedNum + "/" + armConfigBase.AttackCount;
    }




}