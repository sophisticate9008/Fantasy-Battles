
using UnityEngine;

public class DizzyComponent : ComponentBase
{
    public DizzyComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
    }

    public override void Exec(GameObject enemyObj)
    {
        EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
        enemyBase.AddBuff("眩晕", SelfObj, Config.PalsyTime);
    }
}