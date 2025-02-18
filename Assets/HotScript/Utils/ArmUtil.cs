using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class ArmUtil
{
    public static List<string> AllArmTypes => SkillUtil.armTypeDict.Values.ToList();
    public static string ArmTypeToDamagePos(string ArmType)
    {
        return ArmType switch
        {
            "GroundStab" => "land",
            "FlameOrb" => "land",
            "IceBloom" => "land",
            _ => "all",
        };
    }
    #region  伤害类型

    public static string ArmTypeToDamageType(string ArmType)
    {
        return ArmType switch
        {
            "BoomFireBall" => "fire",//0
            "IceBall" => "ice",//1
            "ElectroHit" => "elec",//2
            "GroundStab" => "ad",//3
            "EnergyRay" => "energy",//4
            "Laser" => "energy",//5
            "IceBloom" => "ice",//6
            "JumpElectro" => "elec",//7
            "Tornado" => "wind",//8
            "DragonLaunch" => "ad",//9
            "PressureCutter" => "wind",//10
            "FlameOrb" => "fire",//11
            "WhirlingBlade" => "ad",//12
            "MagicBullet" => "ad",//13
            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion
    #region 倍率等差数列
    public static (float a1, float d) ArmTypeToTlcAP(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => (3, 0.5f),
            1 => (2, 0.4f),
            2 => (4, 0.6f),
            3 => (0.3f, 0.1f),
            4 => (2, 0.2f),
            5 => (0.6f, 0.1f),
            6 => (0.4f, 0.2f),
            7 => (1f, 0.1f),
            8 => (0.7f, 0.1f),
            9 => (5, 0.7f),
            10 => (3f, 0.3f),
            11 => (6, 0.2f),
            12 => (1.5f, 0.2f),
            13 => (1, 0.1f),

            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion
    #region  攻击范围
    public static float ArmTypeToRangeFire(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => 6f,
            1 => 6f,
            2 => 6f,
            3 => 6f,
            4 => 6f,
            5 => 6f,
            6 => 6f,
            7 => 6f,
            8 => 6f,
            9 => 6f,
            10 => 6f,
            11 => 6f,
            12 => 6f,
            13 => 10f,
            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion
    #region  cd等差数列

    public static (float a1, float d) ArmTypeToCdAP(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => (5, 0.05f),
            1 => (5, 0.05f),
            2 => (4, 0.05f),
            3 => (10, 0.05f),
            4 => (10, 0.1f),
            5 => (8f, 0.1f),
            6 => (6f, 0.1f),
            7 => (6f, 0.05f),
            8 => (10f, 0.2f),
            9 => (6f, 0.05f),
            10 => (4, 0.05f),
            11 => (4, 0.01f),
            12 => (10f, 0.05f),
            13 => (2, 0.02f),
            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion
    #region 持续时间等差数列
    #endregion
    public static string ArmTypeToLevelFieldName(string armType)
    {
        return "levelArm" + ArmTypeToId(armType);
    }
    public static string ArmTypeToChipFieldName(string armType)
    {
        return "armChip" + ArmTypeToId(armType);
    }

    public static int ArmTypeToId(string armType)
    {
        foreach (var item in SkillUtil.armTypeDict)
        {
            if (item.Value == armType)
            {
                return item.Key;
            }
        }
        return -1;
    }
    public static string IdToArmType(int id)
    {
        foreach (var item in SkillUtil.armTypeDict)
        {
            if (item.Key == id)
            {
                return item.Value;
            }
        }
        return "";
    }
    #region 武器名字
    public static string ArmTypeToArmName(string armType)
    {
        int id = ArmTypeToId(armType);
        if (id < 13)
        {
            return SkillUtil.IdToName(id);
        }
        return "魔法弹";
    }
    #endregion

    #region  碎片相关
    public static string ArmTypeToChipResName(string armType)
    {
        return "chip_" + armType;
    }
    #endregion
    public static float ArmTypeToDuration(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            4 => 3,
            5 => 4,
            6 => 3,
            8 => 15,
            11 => 3,
            12 => 10,
            _ => 20,
        };
    }
    #region  武器穿透
    public static int ArmTypeToPenetration(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => 1,
            1 => 3,
            10 => 5,
            13 => 1,
            _ => 999,
        };
    }
    #endregion
    #region  武器介绍
    public static string ArmTypeToDes(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            0 => "每穿透一个敌人时爆炸并造成击退",
            1 => "造成冰冻伤害并轻微击退",
            2 => "小范围伤害,并造成麻痹伤害,同时也是解锁电系魔法弹的前置技能",
            3 => "从底端到顶端,造成减速和小击退",
            4 => "穿透所有敌人",
            5 => "锁定一个目标持续到结束或对方死亡,并且对路径造成伤害",
            6 => "持续对敌人造成冰冻",
            7 => "在敌人之间进行跳跃,造成单体伤害,同时造成麻痹",
            8 => "持续牵引敌人",
            9 => "选定一个位置投下石块,造成范围物理伤害和击退",
            10 => "轻微击退",
            11 => "选定一个位置生成滞留的火焰区域,造成较高伤害",
            12 => "生成旋转的利刃持续造成物理伤害",
            13 => "魔法弹,最基础的技能",
            _ => throw new System.NotImplementedException(),
        };
    }
    #endregion 
    #region 攻击冷却
    public static float ArmTypeToAttackCd(string armType)
    {
        int id = ArmTypeToId(armType);
        return id switch
        {
            4 => 0.2f,
            5 => 0.2f,
            6 => 0.5f,
            8 => 0.2f,
            11 => 0.5f,
            12 => 0.2f,
            13 => 1f,
            _ => 0.5f,
        };
    }


    #endregion
    #region 技能养成解锁技能
    public static Dictionary<string, List<string>> armSkillsDes = new()
    {
        {"BoomFireBall", new() {
            "击退力度增加",
            "反弹次数加1",
            "穿透+1",
            "解锁特定技能,效果：[穿透增加,爆炸范围加大]",
            "点燃怪物两秒",
            "穿透+1",
            "必定暴击",
            "每命中30次怪物,额外释放一个"
        }},
        {"IceBall", new() {
            "解锁多发技能",
            "解锁连发技能",
            "穿透+1",
            "解锁特定技能，效果：[每命中一次怪物分裂出一个小号寒冰弹]",
            "有30%概率冰冻敌人2s",
            "反弹次数+1",
            "穿透+1",
            "叠加冻伤,持续5s,上限十层,冰系技能共享层数"
        }},
        {"ElectroHit", new () {
            "伤害倍率增加0.5",
            "麻痹时间增加1s",
            "击杀怪物后减少0.2sCd",
            "解锁技能，效果：[伤害变为200%]",
            "每次释放造成两次伤害",
            "解锁技能，效果：[击杀后范围内触发一次追击,不触发附加效果]",
            "命中后，触发一次弹射三次的跳跃电子，不继承强化和效果",
            "命中后，10s内受到的所有伤害提高10%",
        }},
        {"GroundStab", new() {
            "减速效果提高10%",
            "减速效果提高10%",
            "解锁技能：效果:[附加火焰]",
            "附加的火焰造成最大生命值2%的伤害",
            "使敌人受到的伤害提高10%",
            "有1%概率秒杀普通敌人",
            "减速效果持续时间增加2s",
            "击退力度增加100%"
        }},
        {"EnergyRay", new() {
            "增加1s持续时间",
            "增加30%的减速效果",
            "减少1s cd",
            "解锁技能：效果：[重伤50% 持续1s]",
            "增加1s持续时间",
            "有10%概率眩晕敌人1s",
            "使敌人受到伤害提高10%",
            "重伤效果变为100%"
        }},
        {"Laser", new() {
            "伤害倍率增加0.5",
            "cd - 2",
            "提高2射程",
            "解锁技能:效果:[持续降下无强化的电流直击]",
            "点燃敌人2s",
            "增加击退效果",
            "使敌人受到伤害提高10%",
            "击杀敌人后增加0.2s持续时间"
        }},
        {"IceBloom", new() {
            "增加30%的减速效果",
            "cd - 2s",
            "持续时间+1s",
            "解锁技能：效果：[5%概率触发深度冻结,无视冰冻抗性]",
            "减速效果翻倍",
            "结束时释放冰片+6",
            "结束时释放冰片+6",
            "结束时释放冰片+6"
        }},
        {"JumpElectro", new() {
            "弹射次数+1",
            "麻痹时间+0.5s",
            "cd - 1s",
            "命中敌人5%概率再次释放一次",
            "伤害倍率增加0.5",
            "点燃敌人3s",
            "每次命中5%减少0.5sCd",
            "cd - 2s"

        }},
        {"Tornado", new() {
            "伤害倍率增加0.2",
            "持续时间+2s",
            "范围增加0.2",
            "解锁技能：效果：[变为冰龙卷]",
            "解锁技能：效果：[变为雷龙卷]",
            "牵引力增加",
            "cd - 3",
            "每牵引30个敌人，释放一次无强化压缩气刃"
        }},
        {"DragonLaunch", new() {
            "cd - 1s",
            "下落速度加快",
            "必定眩晕敌人2秒",
            "范围加大",
            "击退力度增加",
            "使敌人受到的伤害增加10%",
            "cd - 2",
            "伤害增加100%"
        }},
        {"PressureCutter", new() {
            "cd - 1",
            "击退力度增加",
            "射程增大2",
            "击退力度增大",
            "穿透+2",
            "初始多发+1",
            "cd - 2",
            "穿透 + 3"
        }},
        {"FlameOrb", new() {
            "cd - 0.5s",
            "射程增加1",
            "持续时间+1s",
            "解锁技能：效果：[伤害翻倍]",
            "cd - 0.6s",
            "增加50%的减速效果",
            "灼烧时造成最大生命值1%的伤害",
            "点燃时造成最大生命值1%的伤害"
        }},
        {"WhirlingBlade", new() {
            "持续时间+2s",
            "初始体积增大20%",
            "cd - 2s",
            "解锁技能:效果：[每命中30次敌人，释放一个无强化的小利刃]",
            "运行速度加快",
            "1%概率秒杀敌人",
            "火焰点燃附加1%最大生命值1%",
            "初始变为两个利刃运行"
        }},
        {"MagicBullet", new() {
            "初始暴击率 + 3%",
            "穿透+1",
            "火系,电系魔法弹取消前置",
            "解锁两个技能:效果[碰到墙壁反弹]，效果[攻速增加10%]",
            "初始暴击率 + 5%",
            "冰系魔法弹取消前置",
            "魔法弹爆炸取消前置",
            "魔法弹有5%的概率造成30%的重伤效果3s"
        }}
    };
    public static List<int> armLevelPos = new() {
        3, 6, 10, 13, 18, 22, 26, 30
    };

    public static List<string> ArmTypeToSkills(string armType)
    {
        return armSkillsDes[armType];
    }
    #endregion
    #region 配置类的快速访问
    public static BoomFireBallConfig boomFireBallConfig =>
        ConfigManager.Instance.GetConfigByClassName("BoomFireBall") as BoomFireBallConfig;

    public static IceBallConfig iceBallConfig =>
        ConfigManager.Instance.GetConfigByClassName("IceBall") as IceBallConfig;

    public static ElectroHitConfig electroHitConfig =>
        ConfigManager.Instance.GetConfigByClassName("ElectroHit") as ElectroHitConfig;

    public static GroundStabConfig groundStabConfig =>
        ConfigManager.Instance.GetConfigByClassName("GroundStab") as GroundStabConfig;

    public static EnergyRayConfig energyRayConfig =>
        ConfigManager.Instance.GetConfigByClassName("EnergyRay") as EnergyRayConfig;

    public static LaserConfig laserConfig =>
        ConfigManager.Instance.GetConfigByClassName("Laser") as LaserConfig;

    public static IceBloomConfig iceBloomConfig =>
        ConfigManager.Instance.GetConfigByClassName("IceBloom") as IceBloomConfig;

    public static JumpElectroConfig jumpElectroConfig =>
        ConfigManager.Instance.GetConfigByClassName("JumpElectro") as JumpElectroConfig;

    public static TornadoConfig tornadoConfig =>
        ConfigManager.Instance.GetConfigByClassName("Tornado") as TornadoConfig;

    public static DragonLaunchConfig dragonLaunchConfig =>
        ConfigManager.Instance.GetConfigByClassName("DragonLaunch") as DragonLaunchConfig;

    public static PressureCutterConfig pressureCutterConfig =>
        ConfigManager.Instance.GetConfigByClassName("PressureCutter") as PressureCutterConfig;

    public static FlameOrbConfig flameOrbConfig =>
        ConfigManager.Instance.GetConfigByClassName("FlameOrb") as FlameOrbConfig;

    public static WhirlingBladeConfig whirlingBladeConfig =>
        ConfigManager.Instance.GetConfigByClassName("WhirlingBlade") as WhirlingBladeConfig;

    public static MagicBulletConfig magicBulletConfig =>
        ConfigManager.Instance.GetConfigByClassName("MagicBullet") as MagicBulletConfig;

    public static GlobalConfig globalConfig =>
        ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
    #endregion
    #region 技能养成的执行函数
    public static Dictionary<string, List<Action>> skillActionDict = new()     {
        {"BoomFireBall", new() {
            () => {boomFireBallConfig.BoomChildConfig.MaxForce += 30;},
            () => {boomFireBallConfig.ReboundCount += 1;},
            () => {boomFireBallConfig.PenetrationLevel += 1;},
            () => {SkillManager.Instance.UnlockSkill(37);},
            () => {boomFireBallConfig.BoomChildConfig.ComponentStrs.Add("点燃"); boomFireBallConfig.BoomChildConfig.FireTime += 2;},
            () => {boomFireBallConfig.PenetrationLevel += 1;},
            () => {boomFireBallConfig.CritRate = 1; boomFireBallConfig.BoomChildConfig.CritRate = 1;},
            () => {
                FighteManager.Instance.AddAccumulateListener(nameof(BoomFireBallBoom), 30, (selfObj) => {
                    boomFireBallConfig.TheArm.FindRandomTarget();
                    boomFireBallConfig.TheArm.Attack();
                });
            },

        }},
        {"IceBall", new() {
            () => {SkillManager.Instance.UnlockSkill(48);},
            () => {SkillManager.Instance.UnlockSkill(49);},
            () => {iceBallConfig.PenetrationLevel += 1;},
            () => {SkillManager.Instance.UnlockSkill(56);},
            () => {iceBallConfig.FreezenProb += 0.3f;},
            () => {iceBallConfig.ReboundCount += 1;},
            () => {iceBallConfig.PenetrationLevel += 1;},
            () => {globalConfig.freezenHurtMaxLevel += 5;},
        }},
        {"ElectroHit", new () {
            () => {electroHitConfig.Tlc += 0.5f;},
            () => {electroHitConfig.PalsyTime += 1;},
            () => {electroHitConfig.TheArm.killActions.Add(() => {electroHitConfig.TheArm.SubtractCd(0.2f);} );},
            () => {SkillManager.Instance.UnlockSkill(75);},
            () => {electroHitConfig.harmCount += 1;},
            () => {SkillManager.Instance.UnlockSkill(69);},
            () => {
                electroHitConfig.TheArm.Config.typeActions["enter"].Add((selfObj, enemyObj) => {
                    JumpElectroConfig initConfig = ArmConfigBase.CreateInitConfig<JumpElectroConfig>();
                    initConfig.JumpCount = 3;
                    initConfig.Owner = electroHitConfig.Owner;
                    List<GameObject> objs = electroHitConfig.TheArm.FindRandomTarget();
                    GameObject targetEnemy;
                    if(objs.Count > 0) {
                        targetEnemy = objs[0];
                    }else {
                        targetEnemy = null;
                    }
                    FighteManager.Instance.AttackWithCustomConfig(targetEnemy, initConfig,selfObj);
                });
                // jumpElectroConfig.TheArm.AttackWithCustomConfig(jumpElectroConfig.TheArm.TargetEnemy);
            },
            () => {electroHitConfig.ComponentStrs.Add("易伤");},
        }},
        {"GroundStab", new() {
            () => {groundStabConfig.SlowDegree += 0.1f;},
            () => {SkillManager.Instance.UnlockSkill(89);},
            () => {groundStabConfig.FirePercentage += 0.02f;},
            () => {groundStabConfig.EasyHurtDegree += 0.1f;},
            () => {groundStabConfig.CrushProb += 0.01f;},
            () => {groundStabConfig.SlowTime += 2;},
            () => {groundStabConfig.MaxForce *= 2;},
        }},
        {"EnergyRay", new() {
            () => {energyRayConfig.Duration += 1;},
            () => {energyRayConfig.ComponentStrs.Add("减速"); energyRayConfig.SlowDegree += 0.3f;},
            () => {energyRayConfig.Cd -= 1;},
            () => {energyRayConfig.ComponentStrs.Add("重伤"); energyRayConfig.GBHRate += 0.5f;},
            () => {energyRayConfig.Duration += 2;},
            () => {energyRayConfig.DizzyProb = 0.1f;  energyRayConfig.ComponentStrs.Add("眩晕");},
            () => {energyRayConfig.ComponentStrs.Add("易伤");},
            () => {energyRayConfig.GBHRate += 0.5f;},
        }},
        {"Laser", new() {
            () => {laserConfig.Tlc += 0.5f;},
            () => {laserConfig.Cd -= 2;},
            () => {laserConfig.RangeFire += 2;},
            () => {SkillManager.Instance.UnlockSkill(117);},
            () => {laserConfig.FireTime = 2f; laserConfig.IsFlame = true;},
            () => {laserConfig.MaxForce = 10;},
            () => {laserConfig.EasyHurtDegree = 0.1f; laserConfig.ComponentStrs.Add("易伤");},
            () => {
                laserConfig.TheArm.killActions.Add(() => {
                    laserConfig.TheArm.AddDuration(0.2f);
                });
            },
        }},
        {"IceBloom", new() {
            () => {iceBloomConfig.ComponentStrs.Add("减速"); iceBallConfig.SlowDegree = 0.3f;},
            () => {iceBloomConfig.Cd -= 2;},
            () => {iceBloomConfig.Duration += 1;},
            () => {iceBloomConfig.SlowDegree = 0.6f;},
            () => {SkillManager.Instance.UnlockSkill(131);},
            () => {iceBloomConfig.IceChipNum += 6;},
            () => {iceBloomConfig.IceChipNum += 6;},
            () => {iceBloomConfig.IceChipNum += 6;},
        }},
        {"JumpElectro", new() {
            () => {jumpElectroConfig.JumpCount += 1;},
            () => {jumpElectroConfig.PalsyTime += 0.5f;},
            () => {jumpElectroConfig.Cd -= 1;},
            () => {jumpElectroConfig.typeActions["enter"].Add ((selfObj, enemyObj) => {
                if(UnityEngine.Random.value <= 0.05f) {
                    if(jumpElectroConfig.TheArm.TargetEnemy.activeSelf) {
                        jumpElectroConfig.TheArm.Attack();
                    }
                }
            });},
            () => {jumpElectroConfig.Tlc = 0.5f;},
            () => {jumpElectroConfig.FireTime = 3f; jumpElectroConfig.ComponentStrs.Add("点燃");},
            () => {jumpElectroConfig.typeActions["enter"].Add ((selfObj, enemyObj) => {
                if(UnityEngine.Random.value <= 0.05f) {
                    electroHitConfig.TheArm.SubtractCd(0.5f);
                }
            });},
            () => {jumpElectroConfig.Cd -= 2;}
        }},
        {"Tornado", new() {
            () => {tornadoConfig.Tlc += 0.2f;},
            () => {tornadoConfig.Duration += 2f;},
            () => {tornadoConfig.SelfScale += 0.2f;},
            () => {SkillManager.Instance.UnlockSkill(166);},
            () => {SkillManager.Instance.UnlockSkill(167);},
            () => {tornadoConfig.MaxForce -= 5;},
            () => {tornadoConfig.Cd -= 3;},
            () => {
                FighteManager.Instance.AddAccumulateListener(nameof(Tornado), 30, (selfObj) => {
                    PressureCutterConfig initConfig = ArmConfigBase.CreateInitConfig<PressureCutterConfig>();
                    initConfig.Owner = nameof(Tornado);
                    List<GameObject> objs = tornadoConfig.TheArm.FindRandomTarget();
                    GameObject targetEnemy;
                    if(objs.Count > 0) {
                        targetEnemy = objs[0];
                    }else {
                        targetEnemy = null;
                    }
                    FighteManager.Instance.AttackWithCustomConfig(targetEnemy,initConfig, selfObj );

                });
            }

        }},
        {"DragonLaunch", new() {
            () => {dragonLaunchConfig.Cd -= 1;},
            () => {((DragonLaunchArm)dragonLaunchConfig.TheArm).bombSpeed -= 2;},
            () => {dragonLaunchConfig.ComponentStrs.Add("眩晕"); dragonLaunchConfig.DizzyTime = 2;},
            () => {dragonLaunchConfig.SelfScale += 0.2f;},
            () => {dragonLaunchConfig.MaxForce += 20;},
            () => {dragonLaunchConfig.ComponentStrs.Add("易伤"); dragonLaunchConfig.EasyHurtDegree = 0.1f;},
            () => {dragonLaunchConfig.Cd -= 2;},
            () => {dragonLaunchConfig.Tlc *= 2;},
        }},
        {"PressureCutter", new() {
            () => {pressureCutterConfig.Cd -= 1;},
            () => {pressureCutterConfig.MaxForce += 20;},
            () => {pressureCutterConfig.RangeFire += 2;},
            () => {pressureCutterConfig.MaxForce += 30;},
            () => {pressureCutterConfig.PenetrationLevel += 2;},
            () => {pressureCutterConfig.MultipleLevel += 1;},
            () => {pressureCutterConfig.Cd -= 2;},
            () => {pressureCutterConfig.PenetrationLevel += 3;}
        }},
        {"FlameOrb", new() {
            () => {flameOrbConfig.Cd -= 0.5f;},
            () => {flameOrbConfig.RangeFire += 1;},
            () => {flameOrbConfig.Duration += 1;},
            () => {SkillManager.Instance.UnlockSkill(216);},
            () => {flameOrbConfig.Cd -= 0.6f;},
            () => {flameOrbConfig.ComponentStrs.Add("减速"); flameOrbConfig.SlowDegree = 0.5f; flameOrbConfig.SlowTime = 0.2f;},
            () => {flameOrbConfig.percentage += 0.01f;},
            () => {flameOrbConfig.FirePercentage += 0.01f;},
        }},
        {"WhirlingBlade", new() {
            () => {whirlingBladeConfig.Duration += 2;},
            () => {whirlingBladeConfig.SelfScale += 0.2f;},
            () => {whirlingBladeConfig.Cd -= 2;},
            () => {SkillManager.Instance.UnlockSkill(232);},
            () => {whirlingBladeConfig.Speed += 0.2f;},
            () => {whirlingBladeConfig.CrushProb += 0.01f;},
            () => {whirlingBladeConfig.FirePercentage += 0.01f;},
            () => {whirlingBladeConfig.AttackCount = 2; whirlingBladeConfig.SelfScale -= 0.3f;},
        }},
        {"MagicBullet", new() {
            () => {magicBulletConfig.CritRate += 0.03f;},
            () => {magicBulletConfig.PenetrationLevel += 1;},
            () => {SkillManager.Instance.CancelPreList(17); SkillManager.Instance.CancelPreList(18);},
            () => {SkillManager.Instance.UnlockSkill(26); SkillManager.Instance.UnlockSkill(27);},
            () => {magicBulletConfig.CritRate += 0.05f;},
            () => {SkillManager.Instance.CancelPreList(19);},
            () => {SkillManager.Instance.CancelPreList(20);},
            () => {magicBulletConfig.typeActions["enter"].Add((selfObj, enemyObj) => {
                if(UnityEngine.Random.value < 0.05) {
                    enemyObj.GetComponent<EnemyBase>().AddBuff("枪重伤",selfObj, 3, 0.3f);
                }
            });}

        }}
    };
    public static List<Action> GetArmSkillAction(string armType, int level)
    {
        return null;
    }
    #endregion

}