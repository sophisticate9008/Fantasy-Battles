using System.Collections;
using FightBases;
using UnityEngine;

namespace TheBuffs
{
    public class Fire : BuffBase
    {

        IEffectController controller;
        public Fire(string buffName, float duration, GameObject selfObj, GameObject enemyObj) : base(buffName, duration, selfObj, enemyObj)
        {
        }

        public override void Effect()
        {
            EnemyObj.GetComponent<MonoBehaviour>().StartCoroutine(LastingFlame());
            controller = EffectManager.Instance.Play(EnemyObj, "FireEffect");
        }

        public override void Remove()
        {
            EnemyObj.GetComponent<MonoBehaviour>().StopCoroutine(LastingFlame());
            EffectManager.Instance.Stop(controller);
        }
        IEnumerator LastingFlame()
        {
            yield return new WaitForSeconds(0.49f);
            FighteManager.Instance.SelfDamageFilter(EnemyObj, SelfObj, true, ArmChildBase.Config.FirePercentage, damageType:"fire");
            
        }
    }
}