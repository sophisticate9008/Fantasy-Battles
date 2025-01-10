
using UnityEngine;
using YooAsset;

public static class EnemyPrefabFactory
{
    private static GameObject normalPrefab;
    private static GameObject elitePrefab;
    public static GameObject NormalPrefab
    {
        get
        {
            if (normalPrefab == null)
            {
                normalPrefab = CommonUtil.GetAssetByName<GameObject>("NormalEnemyPrefab");
                Debug.Log("Loaded " + normalPrefab.name);
                return normalPrefab;
            }
            else
            {
                Debug.Log("Loaded " + normalPrefab.name);
                return normalPrefab;
            }
        }
    }
    public static GameObject ElitePrefab
    {
        get
        {
            if (elitePrefab == null)
            {
                elitePrefab = CommonUtil.GetAssetByName<GameObject>("EliteEnemyPrefab");
                return elitePrefab;
            }
            else
            {
                return elitePrefab;
            }
        }
    }
    public static GameObject Create(string enemyName, string enemyType)
    {

        GameObject prefab = enemyType == "normal" ? NormalPrefab : ElitePrefab;
        EnemyBase enemyBase = prefab.AddComponent(CommonUtil.GetTypeByName(enemyName)) as EnemyBase;
        string controllerName = enemyName + "_Controller";
        RuntimeAnimatorController controller = YooAssets.LoadAssetSync<RuntimeAnimatorController>(controllerName).AssetObject
            as RuntimeAnimatorController;
        enemyBase.animator.runtimeAnimatorController = controller;
        return prefab;
    }

}
