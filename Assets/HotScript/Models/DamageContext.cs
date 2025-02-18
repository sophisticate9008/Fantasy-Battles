using UnityEngine;

public class DamageContext
{
    // 输入参数
    public GameObject Attacker { get; set; }
    public GameObject Defender { get; set; }
    public bool IsBuffDamage { get; set; }
    public float Percentage { get; set; }
    public float Tlc { get; set; }
    // 计算结果
    public string DamageType { get; set; }
    public float FinalDamage { get; set; } = 0;
    public bool IsCritical { get; set; }
    public ArmConfigBase AttackerConfig => AttackerComponent.Config;
    public EnemyConfigBase DefenderConfig => DefenderComponent.Config;
    // 组件缓存
    private EnemyBase _defenderCache;
    public EnemyBase DefenderComponent =>
        _defenderCache = _defenderCache != null ? _defenderCache : Defender.GetComponent<EnemyBase>();

    private ArmChildBase _attackerCache;
    public ArmChildBase AttackerComponent =>
        _attackerCache = _attackerCache != null ? _attackerCache : Attacker.GetComponent<ArmChildBase>();
    public string Owner { get { return Attacker != null ? AttackerConfig.Owner : "other"; } }
}