using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace MyBase
{
    public abstract class BuffBase : IBuff
    {
        public EnemyBase EnemyBase => EnemyObj.GetComponent<EnemyBase>();
        public ArmChildBase ArmChildBase => SelfObj.GetComponent<ArmChildBase>();

        public GameObject EnemyObj { get; set; }

        public float Duration { get; set; } = 5f;

        public string BuffName { get; set; }

        public GameObject SelfObj { get ; set ; }

        public BuffBase(string buffName, float duration,GameObject selfObj, GameObject enemyObj)
        {
            BuffName = buffName;
            Duration = duration;
            EnemyObj = enemyObj;
        }
        public abstract void Effect();  // 留给子类实现具体效果
        public abstract void Remove();  // 留给子类实现具体移除逻辑
        public void EffectAndAutoRemove()
        {
            Effect();
            UpdateEndtimes();
            EnemyObj.GetComponent<MonoBehaviour>().StartCoroutine(AutoRemove());
        }
        private void UpdateEndtimes()
        {
            float buffEndTime = Time.time + Duration;
            EnemyBase.BuffEndTimes[BuffName] = buffEndTime;
        }
        public void EffectControl()
        {
            EnemyBase.CanAction = false;
            float now = Time.time;
            EnemyBase.HardControlEndTime = Mathf.Max(now + Duration, EnemyBase.HardControlEndTime);


        }
        private IEnumerator AutoRemove()
        {
            while (true)
            {
                float now = Time.time;
                yield return new WaitForSeconds(0.1f);
                
                if (now >= EnemyBase.BuffEndTimes[BuffName])
                {
                    Remove();
                    break;
                }
            }
        }
        public virtual void RemoveControl()
        {
            float now = Time.time;
            if(now >= EnemyBase.HardControlEndTime) {
                EnemyBase.CanAction = true;
            }
            

        }
    }
}