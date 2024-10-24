using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : ManagerBase<SkillManager>
{
    public SkillConfig SkillConfig => ConfigManager.Instance.GetConfigByClassName("Skill") as SkillConfig;
    private List<SkillNode> selectedSkills = new();
    private readonly List<int> initialSkillIds = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
    public List<SkillNode> Skills => SkillConfig.skills;
    private int initialSkillCount = 0;
    public void UnlockSkill(int id)
    {
        SkillNode skill = Skills.Find(s => s.id == id);
        if (skill != null)
        {
            skill.isUnlocked = true; // 解锁技能界面
        }
    }

    //更新前置解锁情况
    void UpdateSatisfiedStatus()
    {
        foreach (var skill in Skills)
        {
            bool pre = skill.isSatisfied;
            skill.isSatisfied = Check(skill);
            if (!pre)
            {
                if (skill.isSatisfied)
                {
                    Debug.Log(skill.skillName + "已解锁");
                }
            }
        }
    }
    public void SelectSkill(SkillNode skill)
    {
        int id = skill.id;
        if (skill != null)
        {
            if (initialSkillIds.Contains(id))
            {
                initialSkillCount++;
            }
            selectedSkills.Add(skill);

            int skillCount = selectedSkills.Count(s => s.id == id);
            if (skillCount >= skill.maxSelCount) // Check against individual max
            {
                skill.isUnlocked = false;
            }
        }
    }

    private bool Check(SkillNode skill)
    {
        //检查前置
        foreach (int prereqId in skill.prerequisiteIds)
        {
            if (!selectedSkills.Exists(s => s.id == prereqId))
            {
                return false; // 前置技能未解锁
            }
        }

        foreach (var _ in selectedSkills)
        {
            if (_ != skill && _.conflictIds.Contains(skill.id))
            {
                return false; // 冲突列表里有该技能
            }
        }


        //检查初始技能
        if (initialSkillCount >= 4)
        {
            if (initialSkillIds.Contains(skill.id))
            {
                initialSkillCount++;
                return false;
            }
        }

        //检查是否达到最大数
        return true;
    }
    public List<SkillNode> GetAvailableSkills()
    {
        UpdateSatisfiedStatus();
        return Skills.FindAll(s => s.isSatisfied && s.isUnlocked);
    }
}

