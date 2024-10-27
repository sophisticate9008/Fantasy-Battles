using Factorys;
using FightBases;
using UnityEngine;

namespace MyComponents
{
    public class SlowComponent : ComponentBase
    {

        public SlowComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
        {
        }

        public override void Exec(GameObject enemyObj)
        {
            EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
            enemyBase.AddBuff("减速", SelfObj, Config.SlowTime,Config.SlowDegree);
        }
    }
}
