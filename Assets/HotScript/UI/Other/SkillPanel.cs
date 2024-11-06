using System.Collections.Generic;
using FightBases;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class SkillPanel :TheUIBase {
    List<string> SelectedArmType => SkillManager.Instance.SelectedArmTypes;
    List<Image> skills;
    Image bullet;
    private void Start() {
        skills = GameObject.Find("Skills").transform.GetComponentsInDirectChildren<Image>();
        bullet = skills[^1];
        
    }
    private void Update() {
        int idx = -1;
        foreach(var item in SelectedArmType) {
            idx++;
            if(idx == 0) {
                UpdateBullet();
                //是枪，跳过处理
                continue;
            }
            var skill = skills[idx - 1];
            skill.gameObject.SetActive(true);
            skill.sprite = YooAssets.LoadAssetSync<Sprite>(SkillUtil.ArmTypeToResName(item)).AssetObject as Sprite;
            ArmConfigBase armConfigBase = ConfigManager.Instance.GetConfigByClassName(item) as ArmConfigBase;
            skill.transform.RecursiveFind("Mask").GetComponent<Image>().fillAmount = armConfigBase.CurrentCd / armConfigBase.Cd;
            skill.transform.RecursiveFind("Ring").GetComponent<Image>().fillAmount = armConfigBase.RestDuration / armConfigBase.Duration;
        }
    }
    private void UpdateBullet() {
        ArmConfigBase armConfigBase = ConfigManager.Instance.GetConfigByClassName("Bullet") as ArmConfigBase;
        bullet.transform.RecursiveFind("Mask").GetComponent<Image>().fillAmount = armConfigBase.CurrentCd / armConfigBase.Cd;
        bullet.transform.RecursiveFind("Num").GetComponent<Text>().text = armConfigBase.AttackCount - armConfigBase.CurrentAttackedNum + "/" + armConfigBase.AttackCount;
    }




}