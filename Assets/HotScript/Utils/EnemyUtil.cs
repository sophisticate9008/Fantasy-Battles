using System.Collections.Generic;
using UnityEngine;

public static class EnemyUtil {
    public static readonly int MonsterMaxId = 13;
    public static readonly int BossMaxId = 6;
    public static readonly Dictionary<string, string> nameDict = new() {
        {"Monster1", "肥胖兽"},
        {"Monster2", "草兽"},
        {"Monster3", "噬魇球"},
        {"Monster4", "影翼射手"},
        {"Monster5", "法师"},
        {"Monster6", "烈焰魔灵"},
        {"Monster7", "疾风鸟"},
        {"Monster8", "冰灵"},
        {"Monster9", "冰刺魔怪"},
        {"Monster10", "狼魂"},
        {"Monster11", "盾兽"},
        {"Monster12", "双刃鬼影"},
        {"Monster13", "雷霆魔"},
        {"Boss1", "绿魔领主"},
        {"Boss2", "海蛟领主"},
        {"Boss3", "钢铁领主"},
        {"Boss4", "冰火领主"},
        {"Boss5", "蝙蝠领主 "},
        {"Boss6", "神秘领主"}
    };
        public static readonly Dictionary<string, string> desDict = new() {
        {"Monster1", "较难击退,减少50%风系伤害,占用两个穿透"},
        {"Monster2", "燃烧状态延长100%,未处于燃烧状态时会回血,燃烧时死亡会点燃周围其他怪物3s(固定为攻击力1%的灼烧buff),非燃烧时会给周围怪物回复5%最大生命值"},
        {"Monster3", "远程攻击,射程较远,受到额外50%火系伤害和电系伤害"},
        {"Monster4", "飞行远程,受到额外50%风系伤害和能量伤害"},
        {"Monster5", "远程攻击,并且能歪曲弹道,受到额外50%物理伤害"},
        {"Monster6", "减少50%火系伤害.受到额外50%电系伤害,免疫燃烧,冰冻"},
        {"Monster7", "飞行近战,受到额外50%风系伤害和能量伤害,有独特的飞行轨迹"},
        {"Monster8", "免疫冰冻和点燃,减少50%冰系伤害,受到额外50%火系伤害"},
        {"Monster9", "免疫麻痹,冰冻,阻挡弹道,难以击退,额外受到100%火焰伤害"},
        {"Monster10", "免疫减速,受到额外50%火系伤害,死亡后加速身边怪物50%,持续2s,攻击力较高"},
        {"Monster11", "免疫眩晕,减少50%弹道伤害,较难击退"},
        {"Monster12", "首次到达50%血量时,获得无敌冲刺2s(加速200%,免疫不良状态和伤害),接近防线时无法触发,攻击两次,受到额外50%元素伤害"},
        {"Monster13", "免疫麻痹,攻击两次,额外受到50%火系伤害,减少50%电系伤害"},
        {"Boss1", "间歇性回血,点燃状态会抑制50%,较难击退,受到额外50%火系伤害"},
        {"Boss2", "被冰冻后持续回血,受到额外50%电系伤害,减少50%能量伤害"},
        {"Boss3", "免疫物理伤害和眩晕,受到额外50%元素伤害"},
        {"Boss4", "免疫火系和冰系伤害,免疫燃烧和冻结"},
        {"Boss5", "有20%几率免疫弹道,受到额外50%风系伤害"},
        {"Boss6", "免疫元素伤害,免疫冰冻,麻痹,点燃,受到50%物理伤害"}
    };
    public static int EnemyTypeToId(string enemyType) {
        if(enemyType.Contains("Monster")) {
            return int.Parse(enemyType.Replace("Monster", ""));
        }
        else {
            return int.Parse(enemyType.Replace("Boss", "")) + MonsterMaxId;
        }
    }
    public static string IdToEnemyType(int id) {
        if (id <= MonsterMaxId) {
            return "Monster" + id;
        }else {
            return "Boss" + (id - MonsterMaxId);
        }
    }

    public static string IdToName(int id) {
        string enemyType = IdToEnemyType(id);
        return EnemyTypeToName(enemyType);
    }
    public static string EnemyTypeToName(string enemyType) {
        return nameDict[enemyType];
    }
    public static string EnemyTypeToDes(string enemyType) {
        return desDict[enemyType];
    }
    public static RuntimeAnimatorController EnemyTypeToController(string enemyType) {
        string controllerName = enemyType + "_Controller";
        return CommonUtil.GetAssetByName<RuntimeAnimatorController>(controllerName);
    }   
}