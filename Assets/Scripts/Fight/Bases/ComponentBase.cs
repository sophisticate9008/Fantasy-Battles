using System.Collections.Generic;
using UnityEngine;

namespace MyBase
{


    public abstract class ComponentBase : IComponent
    {
        public ArmConfigBase Config { get; set; }

        public ComponentBase(string componentName, string type, GameObject selfObj)
        {
            Type = type.Split('|');
            ComponentName = componentName;
            SelfObj = selfObj;

        }

        public GameObject SelfObj { get; set; }
        public string ComponentName { get; set; }
        public string[] Type { get; set; }

        public virtual void Init()
        {

        }

        public abstract void TriggerExec(GameObject enemyObj);

    }
}