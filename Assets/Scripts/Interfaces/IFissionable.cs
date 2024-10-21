using MyBase;

public interface IFissionable {
    public ArmConfigBase ChildConfig { get; }
    public string FindType{get;set;}
}