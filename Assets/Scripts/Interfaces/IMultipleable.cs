using System.Collections.Generic;
using MyBase;
using UnityEngine;

public interface IMultipleable
{
    int MultipleLevel { get; set; }
    float AngleDifference { get; set; }

    public static Vector3 CalDirectionDifference(Vector3 baseDirection, int idx, int multipleLevel, float angleDifference)
    {
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
        if (multipleLevel % 2 == 0)
        {
            baseAngle -= angleDifference / 2f; // 顺时针偏转一半的角度差
        }
        float angleOffset = (idx - (multipleLevel - 1) / 2f) * angleDifference;
        float finalAngle = baseAngle + angleOffset;

        // 计算子弹的方向向量（根据最终角度）
        return new Vector3(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad), 0);
    }
    public static List<GameObject> MutiInstantiate(GameObject prefab, Vector3 position, Vector3 baseDirection)
    {
        List<GameObject> objs = new();
        ArmConfigBase concreteConfig = prefab.GetComponent<ArmChildBase>().Config;
        IMultipleable multipleConfig = concreteConfig as IMultipleable;
        int multipleLevel = multipleConfig.MultipleLevel;
        float angleDifference = multipleConfig.AngleDifference;

        for (int i = 0; i < multipleLevel; i++)
        {
            Vector3 bulletDirection = CalDirectionDifference(baseDirection, i, multipleLevel, angleDifference);
            // 生成子弹，并根据发射方向设置旋转
            GameObject newObj = ObjectPoolManager.Instance.GetFromPool(prefab.GetComponent<ArmChildBase>().GetType().Name + "Pool", prefab);
            newObj.transform.position = position;
            ArmChildBase newArmChild = newObj.GetComponent<ArmChildBase>();
            
            // 子弹的方向和速度设置
            newArmChild.Direction = bulletDirection.normalized;
            objs.Add(newObj);
        }
        return objs;
    }
    public static void InitObjs(List<GameObject> objs) {
        foreach (GameObject obj in objs) {
            obj.GetComponent<ArmChildBase>().Init();
        }
    }
}