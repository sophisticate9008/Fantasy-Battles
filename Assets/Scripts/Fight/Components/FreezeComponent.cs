using Factorys;
using MyBase;
using UnityEngine;

namespace MyComponents
{
    public class FreezeComponent : ComponentBase
    {
        public float Duration{get;set;} = 2f;
        public FreezeComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
        {
        }

        public override void Exec(GameObject enemyObj)
        {
            EnemyBase enemyBase = enemyObj.GetComponent<EnemyBase>();
            enemyBase.AddBuff("冰冻", SelfObj, Duration);
        }
    }
}
