using System;
using System.Collections;
using System.Collections.Generic;
using MyEnums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YooAsset;
//管理战斗逻辑，伤害等
public class FighteManager : ManagerBase<FighteManager>
{
    public Vector2 leftBottomBoundary;
    public Vector2 rightTopBoundary;
    public PlayerDataConfig PlayerDataConfig => ConfigManager.Instance.GetConfigByClassName("PlayerData") as PlayerDataConfig;
    public WallConfig WallConfig => ConfigManager.Instance.GetConfigByClassName("Wall") as WallConfig;
    public AnimationCurve spawnRateCurve;
    public MissionRecord mr => MissionManager.Instance.mr;
    public float radius = 25f;
    private readonly GameObject damageTextPrefab;
    public Dictionary<string, float> cdDict = new();
    public MissionBase mb => MissionFactory.Create(mr.missionId);
    public int exp = 0;
    public int level = 1;
    public int CurrentNeedExp => mb.A1_D * level;
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
        {"fire", "orange"},
        {"ad", "white"},
        {"energy", "#B0D3B5"},
        {"wind", "#00cade"},
        {"elec", "#c358db"}
    };
    public SortedDictionary<string, float> harmStatistics = new();
    public SortedDictionary<string, int> killStatistics = new();
    #region  关卡初始化
    private void Start()
    {
        leftBottomBoundary = Camera.main.ViewportToWorldPoint(new Vector3(Constant.leftBottomViewBoundary.x, Constant.leftBottomViewBoundary.y, Camera.main.nearClipPlane));
        rightTopBoundary = Camera.main.ViewportToWorldPoint(new Vector3(Constant.rightTopViewBoundary.x, Constant.rightTopViewBoundary.y, Camera.main.nearClipPlane));
        ObjectPoolManager.Instance.CreatePool("DamageTextUIPool", DamageTextPrefab, 20, 500);
        InitGlobalConfig();
        InitWallBlood();
        InitBackground();
        EnemyManager.Instance.Init();

    }
    void InitGlobalConfig() {
        GlobalConfig.LoadJewel();
    }
    void InitBackground()
    {
        Image backGround = GameObject.Find("Background").GetComponent<Image>();
        backGround.sprite = CommonUtil.GetAssetByName<Sprite>("back_" + mb.MapIdToMapName());
    }
    //初始化城墙
    public void InitWallBlood()
    {
        WallConfig.CurrentLife = WallConfig.LifeMax;
    }
    //加载宝石
    #endregion
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
    DamageCalculator calculator = new DamageCalculator()
        .AddNode(new ElementImmunityNode())
        .AddNode(new BaseDamageNode())
        .AddNode(new CriticalHitNode())
        .AddNode(new DamageAdditionNode())
        .AddNode(new DamageAppend())
        .AddNode(new DamageReductionNode())
        .AddNode(new EasyHurtNode());
    public void SelfDamageFilter(GameObject enemyObj, GameObject selfObj, bool isBuffDamage = false,
     float percentage = 0, float tlc = default, string damageType = "")
    {
        var context = new DamageContext
        {
            Defender = enemyObj,
            Attacker = selfObj,
            IsBuffDamage = isBuffDamage,
            Percentage = percentage,
            Tlc = tlc,
            DamageType = damageType
        };

        calculator.Calculate(context);
        //易伤
        CreateDamageText(enemyObj, (int)context.FinalDamage, context.DamageType, context.IsCritical);
        RecordDamage((int)context.FinalDamage, context.AttackerConfig.Owner);
        //上海结算并判断谁杀死的
        context.DefenderComponent.CalLife((int)context.FinalDamage, context.AttackerConfig.Owner);
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
        if (harmStatistics.ContainsKey(owner))
        {
            harmStatistics[owner] += damage;
        }
        else
        {
            harmStatistics[owner] = damage;
        }
    }
    public void RecordKill(string owner)
    {
        if (killStatistics.ContainsKey(owner))
        {
            killStatistics[owner]++;
        }
        else
        {
            killStatistics[owner] = 1;
        }
    }
    public void AddExp(int val)
    {
        exp += val;
        // if(exp >= 2) {
        //     ControlGame(false);

        //     EndGame(true);
        // }
        //升级
        if (exp / CurrentNeedExp > 0)
        {
            exp %= CurrentNeedExp;
            level++;

            AwakeSkillPanel();
            if (level == 2)
            {
                RealeaseElite();
            }
        }
    }
    #region  技能选择
    public void AwakeSkillPanel(SkillPanelMode mode = SkillPanelMode.Select)
    {
        GameObject canvas = GameObject.Find("UICanvas");
        GameObject panel = canvas.transform.RecursiveFind("SelectPanelThree").gameObject;
        GameObject panelBackup = Instantiate(panel, panel.transform.parent);
        panelBackup.SetActive(true);
        SkillSelectPanel panelBackupUI = panelBackup.GetComponent<SkillSelectPanel>();
        if (mode == SkillPanelMode.Select)
        {
            List<SkillNode> availableSkills = SkillManager.Instance.GetAvailableSkills();
            Debug.Log("可选技能数量" + availableSkills.Count);
            panelBackupUI.skills = availableSkills.RandomChoices(3);
        }
        else
        {
            List<SkillNode> selectedSkills = new();
            for (int i = 0; i < 3; i++)
            {
                List<SkillNode> availableSkills = SkillManager.Instance.GetAvailableSkills();
                SkillNode the = availableSkills.RandomChoices(1)[0];
                selectedSkills.Add(the);
                SkillManager.Instance.SelectSkill(the);
            }
            panelBackupUI.skills = selectedSkills;
        }
        panelBackupUI.mode = mode;
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
            mr.successPercent = Mathf.Max(WallConfig.CurrentLife / (float)WallConfig.LifeMax, mr.successPercent);
            mr.Save();
        }
        MissionManager.Instance.OnReward(level);
        ControlGame(true);
        var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
        YooAssets.LoadSceneSync("Main", sceneMode);

    }
    #endregion





    #region  精英逻辑
    public void RealeaseElite()
    {
        Debug.Log("释放精英");
        EnemyManager.Instance.GenerateElite();
    }
    public void DefeatElite()
    {
        ControlGame(false);
        AwakeSkillPanel(SkillPanelMode.Reward);
    }

    #endregion

    public void PauseGame()
    {
        ControlGame(false);
        GameObject canvas = GameObject.Find("UICanvas");
        GameObject pausePanel = canvas.transform.RecursiveFind("PausePanel").gameObject;
        pausePanel.SetActive(true);
    }
    void Update()
    {
        // 监听键盘上的 Esc 键或手机的返回键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
}
