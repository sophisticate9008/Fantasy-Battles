using UnityEngine;

public class ManagerBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void AwakeCallBack() {

    }
    protected virtual void Awake()
    {
        // 检查是否已经有实例存在
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 防止创建多个实例
        }
        else
        {
            Instance = this as T;
            AwakeCallBack();
        }
        
    }
}
