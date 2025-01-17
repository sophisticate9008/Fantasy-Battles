using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


using TMPro;
using UnityEngine;
using YooAsset;
//管理战斗逻辑，伤害等
public class FighteManager : ManagerBase<FighteManager>
{

    public PlayerDataConfig PlayerDataConfig => ConfigManager.Instance.GetConfigByClassName("PlayerData") as PlayerDataConfig;
    public WallConfig WallConfig => ConfigManager.Instance.GetConfigByClassName("Wall") as WallConfig;
    public AnimationCurve spawnRateCurve;
    public MissionRecord mr => MissionManager.Instance.mr;
    public float radius = 25f;
    private readonly GameObject damageTextPrefab;
    public Dictionary<string, float> cdDict = new();
    public MissionBase currentMission => MissionFactory.Create(mr.missionId);
    public int exp = 0;
    public int level = 1;
    public int CurrentNeedExp => currentMission.A1_D * level;
    public Queue<string> bloodMsgs = new();

    GameObject DamageTextPrefab
    {
        get
        {
            if (damageTextPrefab == null)
            {
                return CommonUtil.GetAssetByName<GameObject>("DamageText");
            }
            else
            {
                return damageTextPrefab;
            }
        }
    }
    public GlobalConfig GlobalConfig => ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
    private static readonly Dictionary<string, string> colorDict = new() {
        {"ice", "blue"},
        {"fire", "red"},
        {"ad", "white"},
        {"energy", "#B0D3B5"}
    };
    public Dictionary<string, float> statistics = new();

    private void Start()
    {

        ObjectPoolManager.Instance.CreatePool("DamageTextUIPool", DamageTextPrefab, 20, 500);
        LoadJewel();
        InitWallBlood();
        EndGame(true);
        EnemyManager.Instance.Init();

    }
    public void InitWallBlood()
    {
        WallConfig.CurrentLife = WallConfig.LifeMax;
    }
    private void LoadJewel()
    {
        for (int i = 1; i <= 6; i++)
        {

            var jewels = PlayerDataConfig.GetValue("place" + i) as List<JewelBase>;
            foreach (var jewel in jewels)
            {
                ItemFactory.CreateJewelAction(jewel.id, jewel.level).Invoke();
            }
        }
    }
    #region 显示伤害
    public void CreateDamageText(GameObject enemyObj, float damage, string type, bool isCritical)
    {
        GameObject textClone = ObjectPoolManager.Instance.GetFromPool("DamageTextUIPool", DamageTextPrefab);

        // 获取主相机
        Camera mainCamera = Camera.main;

        // 将敌人的世界坐标转换为屏幕空间坐标
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(enemyObj.transform.position);

        // 设置圆周波动范围
        float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI); // 随机角度

        // 计算圆周上的偏移量
        float offsetX = Mathf.Cos(randomAngle) * radius;
        float offsetY = Mathf.Sin(randomAngle) * radius;

        // 将偏移量应用到 UI 元素的位置
        Vector3 offsetPosition = new Vector3(screenPosition.x + offsetX, screenPosition.y + offsetY, screenPosition.z);

        // 设置 UI 元素的位置为圆周范围内的随机位置
        textClone.transform.position = offsetPosition;

        // 伤害值格式转换
        string formattedDamage = FormatDamage(damage);

        // 生成彩色字符串
        string color = colorDict.ContainsKey(type) ? colorDict[type] : "white";
        string damageText = isCritical ? $"<color=#ED1523>{formattedDamage}</color>" : $"<color={color}>{formattedDamage}</color>";
        Transform critText = textClone.transform.Find("Critical");
        Transform normalText = textClone.transform.Find("Normal");

        // 设置 TextMeshPro 组件的文本
        Transform[] textTransforms = new Transform[] { critText, normalText };
        int activeIndex = isCritical ? 0 : 1;
        int inactiveIndex = isCritical ? 1 : 0;

        // 启用其中一个，禁用另一个
        textTransforms[activeIndex].gameObject.SetActive(true);
        textTransforms[inactiveIndex].gameObject.SetActive(false);

        // 更新启用的文本内容
        if (textTransforms[activeIndex].TryGetComponent<TextMeshProUGUI>(out var activeText))
        {
            activeText.text = damageText;
        }

        // 启动协程，0.3秒后禁用
        StartCoroutine(DisableDamageText(textClone, 0.3f));
    }
    #endregion

    #region  格式化伤害
    // 格式化伤害值方法
    private string FormatDamage(float damage)
    {
        if (damage < 0)
        {
            return "Miss";
        }
        else if (damage == 0)
        {
            return "Immunity";
        }
        else if (damage >= 1000000)
        {
            return (damage / 1000000f).ToString("0.0") + "M";
        }
        else if (damage >= 1000)
        {
            return (damage / 1000f).ToString("0.0") + "K";
        }
        else
        {
            return damage.ToString("0");
        }
    }
    #endregion
    private IEnumerator DisableDamageText(GameObject textObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 重新返回对象到对象池
        ObjectPoolManager.Instance.ReturnToPool("DamageTextUIPool", textObject);
    }


    #region 伤害过滤
    public void SelfDamageFilter(GameObject enemyObj, GameObject selfObj, bool isBuffDamage = false, float percentage = 0, float tlc = 0, string damageType = "fire")
    {


        EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
        EnemyConfigBase enemyConfig = enemyBase.Config;
        ArmChildBase armChildBase = selfObj.GetComponent<ArmChildBase>();
        ArmConfigBase armConfig = armChildBase.Config;
        string owner = armConfig.Owner;
        if (!isBuffDamage)
        {
            damageType = armConfig.DamageType;
        }
        //首先过滤位置不匹配
        if (armConfig.DamagePos != "all" && enemyConfig.ActionType != "land")
        {
            CreateDamageText(enemyObj, -1, "", false);
            return;
        }

        //过滤掉元素免疫
        if (enemyConfig.DamageTypeImmunityList.IndexOf(damageType) != -1)
        {
            CreateDamageText(enemyObj, 0, "", false);
            return;
        }
        ;

        //消耗怪物免疫次数
        if (enemyBase.ImmunityCount > 0)
        {
            CreateDamageText(enemyObj, 0, "", false);
            enemyBase.ImmunityCount--;
            return;
        }
        //计算基础伤害
        float baseDamage;
        if (isBuffDamage)
        {

            baseDamage = GlobalConfig.AttackValue * armConfig.BuffDamageTlc;
        }
        else
        {
            if (tlc == 0)
            {
                baseDamage = GlobalConfig.AttackValue * armConfig.Tlc;
            }
            else
            {
                baseDamage = GlobalConfig.AttackValue * tlc;
            }

        }
        //基础伤害通过伤害加成
        float addtion = GlobalConfig.AllAddition;
        Dictionary<string, float> damageAddition = GlobalConfig.GetDamageAddition();
        foreach (var item in damageAddition)
        {
            if (item.Key == damageType || item.Key == armConfig.DamageExtraType)
            {
                addtion += item.Value;
            }
        }
        //随机浮动宝石
        float randomAdditon = UnityEngine.Random.Range(GlobalConfig.RandomAdditonMin, GlobalConfig.RandomAdditonMax);
        addtion += randomAdditon;
        //对精英boss加成
        addtion += GlobalConfig.AdditionToEliteOrBoss;
        //待补充 血量高于 血量低于 自身血量低于 防线生命低于

        //基础伤害乘倍率
        baseDamage *= 1 + addtion;

        //怪物伤害减免(可为负)
        Dictionary<string, float> damageReduction = enemyConfig.GetDamageReduction();
        float reduction = 0;
        foreach (var item in damageReduction)
        {
            if (item.Key == damageType || item.Key == armConfig.DamageExtraType)
            {
                reduction += item.Value;
            }
        }
        baseDamage *= 1 + reduction;

        //暴击
        float critRate = GlobalConfig.CritRate + armConfig.CritRate;
        float critDamage = GlobalConfig.CritDamage;
        float[] args;
        bool isCritical = false;
        if (UnityEngine.Random.Range(1, 101) < critRate * 100)
        {
            isCritical = true;
            baseDamage *= 2 + critDamage;
            //百爆
            args = GlobalConfig.CritWithPercentageAndMax;
            baseDamage += Math.Max(enemyBase.MaxLife * args[0], GlobalConfig.AttackValue * args[1]);
        }
        //一般百
        args = GlobalConfig.DamageWithPercentageAndMax;
        baseDamage += Math.Max(enemyBase.MaxLife * args[0], GlobalConfig.AttackValue * args[1]);
        //加上计算好的百分比
        baseDamage += enemyBase.MaxLife * percentage;
        //易伤
        baseDamage *= 1 + enemyBase.EasyHurt;
        CreateDamageText(enemyObj, baseDamage, damageType, isCritical);
        RecordDamage((int)baseDamage, owner);
        //上海结算并判断谁杀死的
        enemyBase.CalLife((int)baseDamage, owner);
    }
    public void EnemyDamegeFilter(int harm, int count = 1)
    {
        int indeedHarm = harm - WallConfig.DamageReduction;
        if (indeedHarm <= 0)
        {
            for (int i = 0; i < count; i++)
            {
                bloodMsgs.Enqueue("格挡");
            }
            return;
        }

        for (int i = 0; i < count; i++)
        {
            if (WallConfig.ImmunityCount > 0)
            {
                bloodMsgs.Enqueue("免疫");
                WallConfig.ImmunityCount--;
                continue;
            }
            WallConfig.CurrentLife -= indeedHarm;
            WallConfig.CurrentLife = Mathf.Clamp(WallConfig.CurrentLife, 0, WallConfig.LifeMax);
            bloodMsgs.Enqueue($"<color=#D72D2D> -{indeedHarm} </color>");
        }
    }
    #endregion
    //记录伤害
    public void RecordDamage(int damage, string owner)
    {
        if (statistics.ContainsKey(owner))
        {
            statistics[owner] += damage;
        }
        else
        {
            statistics[owner] = damage;
        }
    }
    public void AddExp(int val)
    {
        exp += val;
        if(exp >= 2) {
            ControlGame(false);

            EndGame(true);
        }
        if (exp / CurrentNeedExp > 0)
        {
            exp %= CurrentNeedExp;
            level++;
            AwakeSelectPanel();
        }
    }
    #region  技能选择
    public void AwakeSelectPanel()
    {
        GameObject canvas = GameObject.Find("UICanvas");
        GameObject panel = canvas.transform.RecursiveFind("SelectPanelThree").gameObject;
        GameObject panelBackup = Instantiate(panel, panel.transform.parent);
        panelBackup.SetActive(true);
        SkillSelectPanel panelBackupUI = panelBackup.GetComponent<SkillSelectPanel>();
        List<SkillNode> availableSkills = SkillManager.Instance.GetAvailableSkills();
        panelBackupUI.skills = availableSkills.RandomChoices(3);
        Debug.Log("可选技能数量" + availableSkills.Count);
        panelBackupUI.Init();
        ControlGame(false);
    }

    public void ControlGame(bool isContinue)
    {
        if (isContinue)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
    #endregion


    public void AddBlood(int val)
    {
        WallConfig.CurrentLife += val;
        WallConfig.CurrentLife = Mathf.Clamp(WallConfig.CurrentLife, 0, WallConfig.LifeMax);
        bloodMsgs.Enqueue($"<color=#27DE1F> +{val} </color>");
    }

    #region  对局结束
    public void EndGame(bool isSuccess)
    {
        Debug.Log("对局结束");

        if (isSuccess)
        {
            mr.successPercent = WallConfig.CurrentLife / (float)WallConfig.LifeMax;
            mr.Save();
        }
        var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
        YooAssets.LoadSceneSync("Main", sceneMode);
        
    }
    #endregion
}
