public class ArmPropBase {
    public int level;
    public float cd => cdAP.a1 + cdAP.d * level;
    public float rangeFire => ArmUtil.ArmTypeToRangeFire(armtype);
    public float tlc => tlcAP.a1 + tlcAP.d * level;
    public string damagePos => ArmUtil.ArmTypeToDamagePos(armtype);
    public string damageType => ArmUtil.ArmTypeToDamageType(armtype);

    public string armtype;
    public (float a1, float d) tlcAP => ArmUtil.ArmTypeToTlcAP(armtype);
    public (float a1, float d) cdAP => ArmUtil.ArmTypeToCdAP(armtype);
    public ArmPropBase(int level, string armtype) {
        this.level = level;
        this.armtype = armtype;
    }
     
}