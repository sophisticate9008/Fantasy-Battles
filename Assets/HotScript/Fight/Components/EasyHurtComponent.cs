using UnityEngine;

public class EasyHurtComponent : ComponentBase
{
    public EasyHurtComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
    }

    public override void Exec(GameObject enemyObj)
    {
        EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
        enemyBase.AddBuff(Config.ChineseOwner + "易伤", SelfObj, Config.EasyHurtTime, Config.EasyHurtDegree);
    }
}