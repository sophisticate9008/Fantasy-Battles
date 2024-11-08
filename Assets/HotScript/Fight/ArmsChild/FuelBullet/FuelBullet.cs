using FightBases;
using UnityEngine;

namespace ArmsChild
{
    public class FuelBullet : ArmChildBase
    {
        private Vector3 targetPosition;
        public override void Init()
        {
            base.Init();
            targetPosition = TargetEnemy.transform.position;
            ToolManager.Instance.TransmitByStep(1, TargetEnemy.transform.position, gameObject);
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
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            
        }
        public override void OnByTypeCallBack(string type)
        {
            if(type == "enter") {
                ReturnToPool();
            }
        }
        public override void Update()
        {
            base.Update();
            if(TargetEnemy == null || !TargetEnemy.activeSelf) {
                if(transform.position == targetPosition) {
                    InstalledComponents["Hold"].Exec(gameObject);
                    ReturnToPool();
                }
            }
        }
    }
}