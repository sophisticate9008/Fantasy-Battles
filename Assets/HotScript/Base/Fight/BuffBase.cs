
using System.Collections;

using UnityEngine;


public abstract class BuffBase : IBuff
{
    public EnemyBase EnemyBase => EnemyObj.GetComponent<EnemyBase>();
    private ArmChildBase _armChildBase;
    public ArmChildBase ArmChildBase
    {
        get
        {
            if (SelfObj == null) {
                return null;
            }
            if(_armChildBase == null) {
                _armChildBase = SelfObj.GetComponent<ArmChildBase>();
            }
            return _armChildBase;
        }
    }

    public GameObject EnemyObj { get; set; }

    public float Duration { get; set; } = 5f;

    public string BuffName { get; set; }

    public GameObject SelfObj { get; set; }

    public BuffBase(string buffName, float duration, GameObject selfObj, GameObject enemyObj)
    {
        BuffName = buffName;
        Duration = duration;
        EnemyObj = enemyObj;
        SelfObj = selfObj;
    }
    public abstract void Effect();  // 留给子类实现具体效果
    public abstract void Remove();  // 留给子类实现具体移除逻辑
    public void EffectAndAutoRemove()
    {
        Effect();
        EnemyBase.buffEffects.Add(BuffName);
        UpdateEndtimes();
        ToolManager.Instance.StartCoroutine(AutoRemove());
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
            yield return null;
            float now = Time.time;
            if (now > EnemyBase.BuffEndTimes[BuffName])
            {
                EnemyBase.buffEffects.Remove(BuffName);
                Remove();
                break;
            }

            if (EnemyObj == null || EnemyObj.activeSelf == false)
            {
                EnemyBase.buffEffects.Remove(BuffName);
                Remove();
                break;
            }
        }
    }
    public virtual void RemoveControl()
    {

        float now = Time.time;
        if (now >= EnemyBase.HardControlEndTime)
        {
            EnemyBase.CanAction = true;
        }
    }


}
