
using UnityEngine;


public class FreezeComponent : ComponentBase
{

    public FreezeComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
    }

    public override void Exec(GameObject enemyObj)
    {
        EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
        enemyBase.AddBuff("冰冻", SelfObj, Config.FreezeTime);
    }
}

