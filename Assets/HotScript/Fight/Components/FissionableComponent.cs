

using UnityEngine;

public class FissionableComponent : ComponentBase
{

    readonly string findType;
    public FissionableComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
        IFissionable FissionableConfig = Config as IFissionable;
        Prefab = FissionableConfig.FissionableChildConfig.Prefab;
        findType = FissionableConfig.FindType;
    }

    public override void Exec(GameObject enemyObj)
    {
        GameObject targetEnemy;
        ArmChildBase armChildPrefab = Prefab.GetComponent<ArmChildBase>();
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
        var objs = IMultipleable.MutiInstantiate(Prefab, detectionCenter, baseDirection);

        foreach (var obj in objs)
        {
            obj.GetComponent<ArmChildBase>().FirstExceptQueue.Enqueue(enemyObj);
        }

        IMultipleable.InitObjs(objs);
    }
}
