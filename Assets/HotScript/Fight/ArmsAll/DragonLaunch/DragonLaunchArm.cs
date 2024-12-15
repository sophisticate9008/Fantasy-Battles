
using System.Collections;
using FightBases;
using UnityEngine;


namespace Arms
{
    public class DragonLaunchArm : ArmBase
    {
        public GameObject plane;
        public GameObject bomb;
        public float bombDropHeight = 10f; // 炸弹投放高度
        public float planeSpeed = 15f; // 飞机移动速度
        public float bombSpeed = 5f; // 炸弹下落速度

        public override void Attack()
        {
            GameObject planeClone = Instantiate(plane, plane.transform.parent);
            planeClone.SetActive(true);
            Vector3 pos = transform.position;
            pos.x = TargetEnemy.transform.position.x;
            pos.y = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0, 0)).y; // 视口底部
            planeClone.transform.position = pos;

            StartCoroutine(PlaneMove(planeClone));
        }

        private IEnumerator PlaneMove(GameObject plane)
        {
            while (plane.transform.position.y >= Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y &&
                   plane.transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y)
            {
                plane.transform.Translate(0, planeSpeed * Time.deltaTime, 0);
                yield return null;
            }

            // 当飞机超出视口时销毁
            Destroy(plane);

            // 投放炸弹
            GameObject bombClone = Instantiate(bomb, bomb.transform.parent);
            bombClone.SetActive(true);
            Vector3 targetPos = TargetEnemy.transform.position;
            bombClone.transform.position = new Vector3(plane.transform.position.x, targetPos.y + bombDropHeight, 0);
            StartCoroutine(ThrowBomb(bombClone));
        }

        public IEnumerator ThrowBomb(GameObject bomb)
        {
            Vector3 targetPos = TargetEnemy.transform.position;

            while (bomb.transform.position.y > targetPos.y)
            {
                bomb.transform.Translate(0, -bombSpeed * Time.deltaTime, 0);
                yield return null;
            }

            // 到达目标位置后触发 IndeedAttack
            IndeedAttack();
            Destroy(bomb);
        }

        public void IndeedAttack()
        {
            // 执行实际攻击逻辑
            ArmChildBase obj = GetOneFromPool();
            obj.transform.position = TargetEnemy.transform.position;
            obj.TargetEnemyByArm = TargetEnemy;
            obj.Init();
            // 这里可以实现伤害处理等逻辑
        }
    }
}