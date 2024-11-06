using System.Collections;
using System.Collections.Generic;
using ArmConfigs;
using FightBases;
using UnityEngine;

public class Tornado : ArmChildBase
{
    private float smoothDamp = 1f; 
    public TornadoConfig tornadoConfig => Config as TornadoConfig;
    public List<GameObject> exceptObjs = new();
    public override void Init()
    {
        base.Init();
        exceptObjs.Clear();
    }

    public override void Move()
    {
        int maxExcept = (EnemyManager.Instance.liveCount + 100) / 4;
        if (TargetEnemy == null)
        {
            //排除当前目标
            FindTargetInScope(exceptObjs: exceptObjs);
            exceptObjs.Add(TargetEnemy);
            //找不到敌人了
            if (TargetEnemy == null || exceptObjs.Count > maxExcept)
            {
                exceptObjs.Clear();
            }

        }

        //设置朝向目标的方向一直移动
        SetDirectionToTarget();

        Vector3 targetPosition = transform.position + Config.Speed * Time.deltaTime * Direction;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Config.Speed * Time.deltaTime);

    }
    public override UnityEngine.Vector3 Direction
    {
        get; set;
    }

    public override void OnTriggerStay2D(Collider2D collider)
    {

        ApplyForce(collider);
        base.OnTriggerStay2D(collider);
        if (TargetEnemy == collider.gameObject)
        {
            ToolManager.Instance.SetTimeout(() => TargetEnemy = null,  0.05f);
            TargetEnemy = null;

        }

    }
    public override void SetDirectionToTarget()
    {
        if (TargetEnemy != null)
        {
            Vector3 targetDirection = (TargetEnemy.transform.position - transform.position).normalized;
            Direction = Vector3.Lerp(Direction, targetDirection, Time.deltaTime * smoothDamp);
        }

    }


}


