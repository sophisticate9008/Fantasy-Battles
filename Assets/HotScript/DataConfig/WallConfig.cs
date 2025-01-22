using UnityEngine;

[System.Serializable]
public class WallConfig : ConfigBase
{
    public override bool IsCreatePool { get; set; } = false;
    [SerializeField] private int lifeMax = 6000;
    [SerializeField] private int immunityCount = 10;
    [SerializeField] private int damageReduction = 0;

    public int LifeMax { get { return lifeMax; } set { lifeMax = value; } }
    public int ImmunityCount { get { return immunityCount; } set { immunityCount = value; } }
    public int DamageReduction { get { return damageReduction; } set { damageReduction = value; } }
    public int CurrentLife { get; set; } = 0;

}