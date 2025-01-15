using System.Collections.Generic;
using UnityEngine;




public abstract class ComponentBase : IComponent
{
    public GameObject Prefab { get; set; }
    public ArmConfigBase Config => SelfObj.GetComponent<ArmChildBase>().Config;

    public ComponentBase(string componentName, string type, GameObject selfObj)
    {
        Types = type.Split('|');
        ComponentName = componentName;
        SelfObj = selfObj;

    }

    public GameObject SelfObj { get; set; }
    public string ComponentName { get; set; }
    public string[] Types { get; set; }

    public virtual void Init()
    {

    }

    public abstract void Exec(GameObject enemyObj);
    public virtual void GetObjAndInitOnSelf()
    {
        ArmChildBase obj = Prefab.GetComponent<ArmChildBase>().GetOneFromPool();
        obj.transform.position = SelfObj.transform.position;
        obj.Init();
    }
    public virtual void GetObjAndInitOnEnemy(GameObject enemyObj)
    {
        ArmChildBase obj = Prefab.GetComponent<ArmChildBase>().GetOneFromPool();
        obj.transform.position = enemyObj.transform.position + 0.2f * Vector3.down;
        obj.Init();
    }
}
