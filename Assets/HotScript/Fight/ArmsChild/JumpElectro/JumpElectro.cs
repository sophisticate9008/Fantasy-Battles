using System.Collections;
using System.Collections.Generic;
using ArmConfigs;
using FightBases;
using UnityEngine;

namespace ArmsChild
{
    public class JumpElectro : ArmChildBase
    {
        JumpElectroConfig ConcreteConfig => Config as JumpElectroConfig;
        private float stayTime = 0.2f;
        public override void Init()
        {
            base.Init();
            StartCoroutine(StayToJump());
        }
        private IEnumerator StayToJump()
        {
            for (int i = 0; i < ConcreteConfig.JumpCount; i++)
            {
                yield return new WaitForSeconds(stayTime);
                GameObject _ = FindTargetRandom(TargetEnemy);
                if (_ == null)
                {
                    ReturnToPool();
                    yield break;
                }
                PathDamage();
                ToolManager.Instance.TransmitByStep(stayTime, TargetEnemy.transform.position, gameObject);

            }
            ReturnToPool();
        }
        public override bool BeforeTirgger(Collider2D collision)
        {
            if (collision.gameObject != TargetEnemy)
            {
                return false;
            }
            else
            {
                return base.BeforeTirgger(collision);
            }
        }

        public void PathDamage()
        {
            List<GameObject> hitEnemys = LineCastAll(transform.position, TargetEnemy.transform.position);
            foreach (var hit in hitEnemys)
            {
                //排除线段两端
                if (hit == hitEnemys[0] || hit == hitEnemys[^1])
                {
                    continue;
                }
                CreateDamage(hit, ConcreteConfig.PathDamageTlc);
                InstalledComponents["麻痹"].Exec(hit);
            }
        }

    }
}