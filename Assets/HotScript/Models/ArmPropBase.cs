public class ArmPropBase : ReflectiveBase
{
    public int level;
    public float cd => cdAP.a1 - cdAP.d * level;
    public float rangeFire => ArmUtil.ArmTypeToRangeFire(armtype);
    public float tlc => tlcAP.a1 + tlcAP.d * level;
    public string damagePos => ArmUtil.ArmTypeToDamagePos(armtype);
    public string damageType => ArmUtil.ArmTypeToDamageType(armtype);

    public string armtype;
    public float duration => ArmUtil.ArmTypeToDuration(armtype);
    public (float a1, float d) tlcAP => ArmUtil.ArmTypeToTlcAP(armtype);
    public (float a1, float d) cdAP => ArmUtil.ArmTypeToCdAP(armtype);
    public string resName => SkillUtil.ArmTypeToResName(armtype);
    public string armName => ArmUtil.ArmTypeToArmName(armtype);
    public int moneyNeed => Constant.upgradeMoneyNeed.a1 + Constant.upgradeMoneyNeed.d * level;
    public int chipNeed => Constant.upgradeChipNeed.a1 + Constant.upgradeChipNeed.d * level;
    public string chipResName => ArmUtil.ArmTypeToChipResName(armtype);
    public int penetration => ArmUtil.ArmTypeToPenetration(armtype);
    public float attackCd => ArmUtil.ArmTypeToAttackCd(armtype);
    public string des => ArmUtil.ArmTypeToDes(armtype);
    public string chipFieldName => "armChip" + ArmUtil.ArmTypeToId(armtype);
    public string levelFieldName => "levelArm" + ArmUtil.ArmTypeToId(armtype);
    public ArmPropBase(int level, string armtype)
    {
        this.level = level;
        this.armtype = armtype;
    }

}