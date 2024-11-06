using Factorys;
using FightBases;
using UnityEngine;

namespace MyComponents
{
    public class DampComponent : ComponentBase
    {
        private float damp = 0.25f;
        public DampComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
        {

        }

        public override void Exec(GameObject enemyObj)
        {
            float speedChanged = Config.Speed * damp;
            Config.Speed -= speedChanged;
            ToolManager.Instance.SetTimeout(() => Config.Speed += speedChanged, 0.2f);
        }

    }
}
