using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class SkillPanel : TheUIBase
{
    List<string> SelectedArmType => SkillManager.Instance.SelectedArmTypes;
    List<Image> skills;
    Image MagicBullet;
    private HashSet<Button> _registeredButtons = new HashSet<Button>();
    public RangeFireShow rangeFireShow;

    // 缓存组件的字典
    private Dictionary<Image, (Image maskImage, Image ringImage)> skillComponents = new Dictionary<Image, (Image, Image)>();
    private Image _magicBulletMaskImage;
    private Text _magicBulletNumText;

    private void Start()
    {
        skills = GameObject.Find("Skills").transform.GetComponentsInDirectChildren<Image>();
        MagicBullet = skills[^1];

        // 缓存MagicBullet相关组件
        _magicBulletMaskImage = MagicBullet.transform.RecursiveFind("Mask").GetComponent<Image>();
        _magicBulletNumText = MagicBullet.transform.RecursiveFind("Num").GetComponent<Text>();

        // 缓存所有技能的 Mask 和 Ring 组件
        foreach (var skill in skills)
        {
            var maskImage = skill.transform.RecursiveFind("Mask").GetComponent<Image>();
            var ringImage = skill.transform.RecursiveFind("Ring").GetComponent<Image>();
            skillComponents[skill] = (maskImage, ringImage);
        }
    }

    private void Update()
    {
        int idx = -1;
        foreach (var item in SelectedArmType)
        {
            idx++;
            if (idx == 0)
            {
                UpdateMagicBullet();
                continue; // 是枪，跳过处理
            }

            var skill = skills[idx - 1];
            skill.gameObject.SetActive(true);
            skill.sprite = CommonUtil.GetAssetByName<Sprite>(SkillUtil.ArmTypeToResName(item));

            ArmConfigBase armConfigBase = ConfigManager.Instance.GetConfigByClassName(item) as ArmConfigBase;

            // 获取已经缓存的 Mask 和 Ring 组件
            var (maskImage, ringImage) = skillComponents[skill];

            // 更新进度条
            maskImage.fillAmount = armConfigBase.CurrentCd / armConfigBase.Cd;
            ringImage.fillAmount = armConfigBase.RestDuration / armConfigBase.Duration;

            Button btn = skill.transform.GetComponent<Button>();
            if (!_registeredButtons.Contains(btn))
            {
                // 移除旧的监听器（如果有）
                btn.onClick.RemoveAllListeners();
                // 添加新的监听器
                btn.onClick.AddListener(() =>
                {
                    ShowRangeFire(armConfigBase.RangeFire);
                });
                // 记录该按钮已经注册过监听器
                _registeredButtons.Add(btn);
            }
        }
    }

    private void UpdateMagicBullet()
    {
        ArmConfigBase armConfigBase = ConfigManager.Instance.GetConfigByClassName("MagicBullet") as ArmConfigBase;
        
        // 更新MagicBullet相关的进度条和显示数值
        _magicBulletMaskImage.fillAmount = armConfigBase.CurrentCd / armConfigBase.Cd;
        _magicBulletNumText.text = $"{armConfigBase.AttackCount - armConfigBase.CurrentAttackedNum}/{armConfigBase.AttackCount}";

        Button btn = MagicBullet.transform.GetComponent<Button>();
        if (!_registeredButtons.Contains(btn))
        {
            // 移除旧的监听器（如果有）
            btn.onClick.RemoveAllListeners();
            // 添加新的监听器
            btn.onClick.AddListener(() =>
            {
                ShowRangeFire(armConfigBase.RangeFire);
            });
            // 记录该按钮已经注册过监听器
            _registeredButtons.Add(btn);
        }
    }

    void ShowRangeFire(float rangeFire)
    {
        rangeFireShow.Show(rangeFire);
    }
}
