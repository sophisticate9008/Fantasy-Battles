using UnityEngine;

public class GBHComponent : ComponentBase
{
    public GBHComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
    }

    public override void Exec(GameObject enemyObj)
    {
        EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
        enemyBase.AddBuff(Config.ChineseOwner + "重伤", SelfObj, 3, Config.GBHRate);
    }
}