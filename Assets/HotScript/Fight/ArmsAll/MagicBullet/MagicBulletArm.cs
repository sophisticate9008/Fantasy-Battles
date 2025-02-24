using System.Collections;



using UnityEngine;
using YooAsset;

public class MagicBulletArm : ArmBase, IRepeatable
{


    public MagicBulletConfig ConcreteConfig => Config as MagicBulletConfig;
    //锁定的敌人

    public int RepeatLevel { get { return ConcreteConfig.RepeatLevel; } set { } }

    protected override void Start()
    {
        base.Start();
    }

    // [Inject]
    // public void Inject(MagicBullet prefab)
    // {
    //     Debug.Log("Inject MagicBullet");
    //     this.prefab = prefab;
    // }

    public override void FisrtFindTarget()
    {
        FindTargetNearestOrElite();
    }
    public override void OtherFindTarget()
    {
        FindTargetNearestOrElite();

    }
    public override void Attack()
    {
        StartCoroutine(ShootAtTarget());
    }
    // 发射子弹
    private IEnumerator ShootAtTarget()
    {
        for (int i = 0; i < RepeatLevel; i++) // 连发逻辑
        {
            AttackMultipleOnce(); // 多条弹道发射
            yield return new WaitForSeconds(ConcreteConfig.RepeatCd); // 每次连发之间的间隔
        }
    }


}
