
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyEnums;
using UnityEngine;

public class ArmBase : MonoBehaviour, IArms
{

    private float lastFireTime = -10000f;
    public GameObject TargetEnemy { get; set; }

    public ArmConfigBase Config => ConfigManager.Instance.GetConfigByClassName(GetType().Name.Replace("Arm", "")) as ArmConfigBase;

    public void FindTargetNearestOrElite()
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        enemies = enemies.Where(x => Config.DamagePos == "all" || x.Config.ActionType == "land").ToArray();
        foreach (EnemyBase enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= Config.RangeFire)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.gameObject;
            }
        }

        if (nearestEnemy != null && nearestEnemy.activeSelf)
        {
            TargetEnemy = nearestEnemy;
        }
        else
        {
            TargetEnemy = null;
        }
    }
    protected virtual void Start()
    {
        SkillManager.Instance.SelectedArmTypes.Add(GetType().Name.Replace("Arm", ""));
    }
    public virtual void Update()
    {
        AttackLogic();
    }
    public virtual void AttackLogic()
    {
        if (TargetEnemy == null || !TargetEnemy.activeSelf)
        {
            FindTargetNearestOrElite();
        }

        if (Time.time < lastFireTime)
        {
            Config.CurrentCd = 0;
        }
        else
        {
            Config.CurrentCd = Mathf.Max(0, Config.Cd - (Time.time - lastFireTime));
        }

        if (TargetEnemy != null && TargetEnemy.activeSelf && Time.time - lastFireTime > Config.Cd)
        {
            lastFireTime = Time.time + 10000000000;//设为较大值，避免再次进入
            StartCoroutine(AttackSequence()); // 发射
        }
    }

    public virtual IEnumerator AttackSequence()
    {
        Config.CurrentAttackedNum = 0;
        while (Config.CurrentAttackedNum < Config.AttackCount)
        {

            if (Config.CurrentAttackedNum == 0)
            {
                FisrtFindTarget();
            }
            else
            {
                OtherFindTarget();
            }
            if (TargetEnemy != null && TargetEnemy.activeSelf)
            {
                if (Config.CdType == CdTypes.Exhaust)
                {
                    Config.CurrentAttackedNum++;
                }
                Attack();
            }
            AddAttackedNum();
            yield return new WaitForSeconds(Config.AttackCd);
        }
        //如果是立刻进入冷却, 否则等持续时间结束
        if (Config.CdType == CdTypes.AtOnce)
        {
            lastFireTime = Time.time;
            TargetEnemy = null;
        }

        StartCoroutine(WaitEnd());
    }
    private IEnumerator WaitEnd()
    {
        while (Config.RestDuration > 0)
        {
            yield return null;
        }
        lastFireTime = Time.time;
        TargetEnemy = null;
    }
    public virtual void AddAttackedNum()
    {
        if (Config.CdType != CdTypes.Exhaust)
        {
            Config.CurrentAttackedNum++;
        }

    }
    public virtual void Attack()
    {

    }
    public virtual List<GameObject> FindRandomTarget(int count = 1)
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        enemies = enemies.Where(x => Config.DamagePos == "all" || x.Config.ActionType == "land").ToArray();
        List<GameObject> selectedEnemies = new();
        int length = enemies.Length;
        if (length == 0)
        {
            return null;
        }
        for (int i = 0; i < count; i++)
        {
            int _ = Random.Range(0, length);
            selectedEnemies.Add(enemies[_].gameObject);
        }
        if (count == 1)
        {
            TargetEnemy = selectedEnemies[0];
        }
        return selectedEnemies;
    }

    public virtual void FisrtFindTarget()
    {
        FindTargetNearestOrElite();
    }

    public virtual void OtherFindTarget()
    {
        FindRandomTarget();
    }
    public virtual ArmChildBase GetOneFromPool()
    {
        ArmChildBase obj = ObjectPoolManager.Instance.GetFromPool(GetType().Name.Replace("Arm", "") + "Pool", Config.Prefab).GetComponent<ArmChildBase>();
        return obj;
    }
    public void AttackMultipleOnce()
    {
        if (TargetEnemy == null) return;

        // 计算从枪口指向敌人的方向向量
        Vector3 baseDirection = (TargetEnemy.transform.position - transform.position).normalized;
        // 发射 MultipleLevel 数量的子弹
        var objs = IMultipleable.MutiInstantiate(Config.Prefab, transform.position, baseDirection);
        IMultipleable.InitObjs(objs);
    }
}
