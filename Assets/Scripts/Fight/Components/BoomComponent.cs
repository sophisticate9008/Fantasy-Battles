using FightBases;
using UnityEngine;

public class BoomComponent : ComponentBase
{
    public BoomComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
        Prefab = (Config as IBoomable).BoomChildConfig.Prefab;

    }

    public override void GetObjAndInit()
    {
        ArmChildBase selfArmchild = SelfObj.GetComponent<ArmChildBase>();
        ArmChildBase obj = Prefab.GetComponent<ArmChildBase>().GetOneFromPool();
        obj.transform.position = SelfObj.transform.position - 0.2f * selfArmchild.Direction;
        obj.Init();

    }
    public override void Exec(GameObject enemyObj)
    {
        GetObjAndInit();
    }
}