
using UnityEngine;

public class HoldComponent : ComponentBase
{
    public HoldComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
        Prefab = (Config as IHoldable).HoldChildConfig.Prefab;
    }
    public override void Exec(GameObject enemyObj)
    {
        GetObjAndInitOnEnemy(enemyObj);
    }
    
}