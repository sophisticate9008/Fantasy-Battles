
using UnityEngine;

public class FireComponent : ComponentBase
{
    public FireComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
        
    }

    public override void Exec(GameObject enemyObj)
    {
        EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
        enemyBase.AddBuff(Config.ChineseOwner + "点燃", SelfObj, Config.FireTime, Config.fireTlc);
    }
}