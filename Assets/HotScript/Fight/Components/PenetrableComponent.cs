
using UnityEngine;

public class PenetrableComponent : ComponentBase, IPenetrable
{

    private int _penetrationLevel = 0;
    public PenetrableComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {

    }
    public override void Init()
    {
        base.Init();
        PenetrationLevel = (ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig).AllPenetrationLevel;
        PenetrationLevel += (SelfObj.GetComponent<ArmChildBase>().Config as IPenetrable).PenetrationLevel;
    }
    public int PenetrationLevel
    {
        get => _penetrationLevel;
        set
        {
            _penetrationLevel = value;
        }
    }
    public void HandleDestruction()
    {
        SelfObj.GetComponent<ArmChildBase>().ReturnToPool();
    }

    public override void Exec(GameObject enemyObj)
    {
        PenetrationLevel -= enemyObj.GetComponent<EnemyBase>().Config.Blocks;
        if (PenetrationLevel <= 0)
        {
            HandleDestruction();
        }
    }
}


