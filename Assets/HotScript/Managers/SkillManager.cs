using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SkillManager : ManagerBase<SkillManager>
{

    private readonly List<SkillNode> selectedSkills = new();
    private readonly List<int> initialSkillIds = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
    private List<SkillNode> _cachedSkills;
    private Dictionary<int, List<int>> _cachedPreList;

    // 使用懒加载缓存 Skills
    public List<SkillNode> Skills
    {
        get
        {
            _cachedSkills ??= SkillUtil.AllSkill();  // 第一次访问时计算并缓存
            return _cachedSkills;
        }
    }

    // 使用懒加载缓存 preList
    public Dictionary<int, List<int>> PreList
    {
        get
        {
            _cachedPreList ??= SkillUtil.GetPreListDict();  // 第一次访问时计算并缓存
            return _cachedPreList;
        }
    }
    private int initialSkillCount = 0;
    #region 前置列表
    public List<int> IdToPreList(int id)
    {
        return PreList[id];
    }
    #endregion
    public void CancelPreList(int id)
    {
        PreList[id].Clear();
    }
    public readonly List<string> SelectedArmTypes = new();
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
        SkillUtil.IdToUseAction(id).Invoke();
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
        foreach (int prereqId in IdToPreList(skill.id))
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
    //给某些减cd的技能特性用

}

