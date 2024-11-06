using FightBases;

public interface IFissionable {
    public ArmConfigBase FissionableChildConfig { get; }
    public string FindType{get;set;}
}