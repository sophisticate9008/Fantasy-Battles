using UnityEngine;
using System.Collections.Generic;
public class ObjectPoolManager : ManagerBase<ObjectPoolManager>
{


    // 字典存储多个对象池，key为池的名字，值为对象池队列
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // 字典存储每个池的最大长度，key为池的名字，值为池的最大长度
    private Dictionary<string, int> poolMaxSizeDictionary;

    // 字典存储每个对象池的父对象
    private Dictionary<string, Transform> poolParentDictionary;
    protected override void AwakeCallBack()
    {
        base.AwakeCallBack();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        poolMaxSizeDictionary = new Dictionary<string, int>();
        poolParentDictionary = new Dictionary<string, Transform>();

    }

    // 创建对象池，并设置池的最大长度和父对象
    public void CreatePool(string poolName, GameObject prefab, int initialPoolSize, int maxPoolSize)
    {
        if (poolDictionary.ContainsKey(poolName))
        {
            //不重复创建
            return;
        }
        GameObject grandParent;
        if (poolName.Contains("UI"))
        {
            grandParent = GameObject.Find("UIPools");
        }
        else
        {
            grandParent = GameObject.Find("ObjectPools");
        }
        GameObject poolObject = new GameObject(poolName);
        poolObject.transform.parent = grandParent.transform;
        Queue<GameObject> objectPool = new Queue<GameObject>();

        // 初始化时创建指定数量的对象，并加入池中
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab, poolObject.transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(poolName, objectPool);
        poolMaxSizeDictionary.Add(poolName, maxPoolSize); // 保存每个池的最大长度
        poolParentDictionary.Add(poolName, poolObject.transform);       // 保存父对象
        Debug.Log($"创建对象池 {poolName}");

    }

    // 从对象池中获取对象，如果没有可用对象则创建一个新的
    public GameObject GetFromPool(string poolName, GameObject prefab)
    {

        if (poolDictionary.ContainsKey(poolName))
        {
            // 如果对象池中有对象，取出一个对象
            if (poolDictionary[poolName].Count > 0)
            {
                GameObject obj = poolDictionary[poolName].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                // 如果池子空了，就创建一个新的对象，并设置父对象
                Transform parent = poolParentDictionary[poolName];
                GameObject newObj = Instantiate(prefab, parent);
                newObj.SetActive(true);
                return newObj;
            }
        }
        else
        {
            Debug.LogError($"Pool {poolName} doesn't exist!");
            return null;
        }
    }

    // 将对象返回到池子里
    public void ReturnToPool(string poolName, GameObject obj)
    {
        if(obj == null) {
            return;
        }
        if (poolDictionary.ContainsKey(poolName))
        {
            // 如果池子已满，直接销毁对象
            if (poolDictionary[poolName].Count >= poolMaxSizeDictionary[poolName])
            {
                Destroy(obj);
            }
            else
            {
                obj.SetActive(false);
                obj.transform.SetParent(poolParentDictionary[poolName]); // 设置父对象，防止对象散开
                poolDictionary[poolName].Enqueue(obj);
            }
        }
        else
        {
            Debug.LogError($"Pool {poolName} doesn't exist!");
            Destroy(obj); // 如果池不存在，销毁对象
        }
    }
    public void ClearPool(string poolName) {
        if(poolDictionary.ContainsKey(poolName)) {
            poolDictionary[poolName].Clear();
        }
        
    }
}
