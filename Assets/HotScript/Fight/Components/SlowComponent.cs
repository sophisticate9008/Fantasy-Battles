

using UnityEngine;



public class SlowComponent : ComponentBase
{

    public SlowComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
    }

    public override void Exec(GameObject enemyObj)
    {
        EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
        enemyBase.AddBuff(Config.Owner + "减速", SelfObj, Config.SlowTime, Config.SlowDegree);
    }
}

