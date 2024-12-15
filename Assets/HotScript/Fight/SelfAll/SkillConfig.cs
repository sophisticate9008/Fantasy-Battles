using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SkillConfig : ConfigBase
{
    public List<SkillNode> skills = SkillUtil.AllSkill();
    
}