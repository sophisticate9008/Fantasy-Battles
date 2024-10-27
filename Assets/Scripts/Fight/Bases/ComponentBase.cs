using System.Collections.Generic;
using UnityEngine;

namespace FightBases
{


    public abstract class ComponentBase : IComponent
    {
        public GameObject Prefab { get; set; }
        public ArmConfigBase Config => SelfObj.GetComponent<ArmChildBase>().Config;

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

        public abstract void Exec(GameObject enemyObj);
        public virtual void GetObjAndInit()
        {
            ArmChildBase obj = Prefab.GetComponent<ArmChildBase>().GetOneFromPool();
            obj.transform.position = SelfObj.transform.position;
            obj.Init();
        }
    }
}