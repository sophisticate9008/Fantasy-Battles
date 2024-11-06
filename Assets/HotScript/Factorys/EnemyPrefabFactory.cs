using FightBases;
using UnityEngine;
using YooAsset;

namespace Factorys
{
    public class EnemyPrefabFactory
    {
        private static GameObject normalPrefab;
        private static GameObject elitePrefab;
        public static GameObject NormalPrefab
        {
            get
            {
                if (normalPrefab == null)
                {
                    normalPrefab = Resources.Load<GameObject>(Constant.EnemyPrefabResPath + "NormalZombie");
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
                    elitePrefab = Resources.Load<GameObject>(Constant.EnemyPrefabResPath + "EliteZombie");
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
            GameObject prefabClone = GameObject.Instantiate(prefab);
            EnemyBase enemyBase = prefabClone.AddComponent(CommonUtil.GetTypeByName(enemyName)) as EnemyBase;
            string controllerName = enemyName.ToLower() + "_Controller";
            RuntimeAnimatorController controller = YooAssets.LoadAssetSync<RuntimeAnimatorController>(controllerName).AssetObject 
                as RuntimeAnimatorController;
            enemyBase.animator.runtimeAnimatorController = controller;
            return prefabClone;
        }

    }
}