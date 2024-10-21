using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class EffectManager : MonoBehaviour
{
    // Start is called before the first frame update
    //防止重新创建特效池子
    private HashSet<string> effectsName = new();
    public static EffectManager Instance
    { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

    }
    
    public GameObject GetoneFromPool(string effectName) {
        GameObject prefab = YooAssets.LoadAssetSync(effectName).AssetObject as GameObject;
        if(!effectsName.Contains(effectName)) {
            ObjectPoolManager.Instance.CreatePool(effectName + "Pool", prefab, 1, 500);
            effectsName.Add(effectName);
            return  ObjectPoolManager.Instance.GetFromPool(effectName + "Pool", prefab);
        }else {
            return  ObjectPoolManager.Instance.GetFromPool(effectName + "Pool", prefab);
        }
    }
    public IEffectController Play(GameObject enemy, string effectName) {
        IEffectController controller = GetoneFromPool(effectName).GetComponent<IEffectController>();
        controller.Enemy = enemy;
        controller.EffectName = effectName;
        controller.Init();
        controller.Play();
        return controller;
    }
    public void Stop(IEffectController controller) {
        controller.Stop();
        ObjectPoolManager.Instance.ReturnToPool(controller.EffectName + "Pool",(controller as MonoBehaviour).gameObject);
        // StartCoroutine(WaitAnimStop(controller));
    }
    //节省资源 特效动画简化
    // private IEnumerator WaitAnimStop(IEffectController controller) {
    //     if(controller.IsPlaying) {
    //         yield return null;
    //     }else {
    //         ObjectPoolManager.Instance.ReturnToPool(controller.EffectName + "Pool",(controller as MonoBehaviour).gameObject);
    //     }
    // }

}
