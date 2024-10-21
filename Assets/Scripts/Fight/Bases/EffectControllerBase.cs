using System.Collections.Generic;
using UnityEngine;

namespace MyBase
{
    public class EffectControllerBase : MonoBehaviour, IEffectController
    {
        public string EffectName { get; set; }
        public bool IsPlaying { get; set; } = false;
        public GameObject Enemy { get; set; }

        public virtual void Init()
        {
            ClearSameEffect();
        }

        public virtual void Play()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Stop()
        {
            throw new System.NotImplementedException();
        }
        private void ClearSameEffect()
        {
            List<Transform> toRemove = new();

            foreach (Transform controller in Enemy.transform)
            {
                var effectController = controller.GetComponent<IEffectController>();
                if (effectController != null && effectController.EffectName == EffectName)
                {
                    toRemove.Add(controller);
                }
            }

            // 遍历完成后，再执行移除操作
            foreach (var controller in toRemove)
            {
                ObjectPoolManager.Instance.ReturnToPool(EffectName + "Pool", controller.gameObject);
            }
        }
    }
}
