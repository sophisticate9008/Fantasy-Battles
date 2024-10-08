using System.Collections.Generic;
using ArmConfigs;
using Factorys;
using MyBase;
using UnityEngine;

public class FissionableComponent : ComponentBase
{

    readonly GameObject prefab;
    readonly string findType;
    public FissionableComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
        Config = selfObj.GetComponent<ArmChildBase>().Config;
        IFissionable FissionableConfig = Config as IFissionable;
        prefab = FissionableConfig.ChildConfig.Prefab;
        findType = FissionableConfig.FindType;
    }

    public override void TriggerExec(GameObject enemyObj)
    {
        GameObject targetEnemy;
        ArmChildBase armChildPrefab = prefab.GetComponent<ArmChildBase>();
        Collider2D collider = SelfObj.GetComponent<Collider2D>();
        Vector3 detectionCenter = collider.bounds.center;
        if(findType == "scope") {
            armChildPrefab.FindTargetInScope(1,enemyObj);
        }else {
            armChildPrefab.FindTargetRandom(enemyObj);
        }
        
        if (armChildPrefab.TargetEnemy != null)
        {
            targetEnemy = armChildPrefab.TargetEnemy;
        }
        else
        {
            return;
        }

        Vector3 baseDirection = (targetEnemy.transform.position - SelfObj.transform.position).normalized;
        var objs = IMultipleable.MutiInstantiate(prefab, detectionCenter, baseDirection);

        foreach (var obj in objs)
        {
            obj.GetComponent<ArmChildBase>().FirstExceptQueue.Enqueue(enemyObj);
        }

        IMultipleable.InitObjs(objs);
    }
}
