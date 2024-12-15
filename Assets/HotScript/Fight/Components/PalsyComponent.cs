
using UnityEngine;


public class PalsyComponent : ComponentBase
{

    public PalsyComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
    }

    public override void Exec(GameObject enemyObj)
    {
        EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
        enemyBase.AddBuff("麻痹", SelfObj, Config.PalsyTime);
    }
}

