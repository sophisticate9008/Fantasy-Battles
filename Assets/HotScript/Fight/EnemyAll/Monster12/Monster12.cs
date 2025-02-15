using UnityEngine;

public class Monster12 : EnemyBase
{
    public bool isSkilled = false;
    public bool isSkilling = false;
    public override void Init()
    {
        base.Init();
        isSkilled = false;


    }
    public override void Update()
    {
        base.Update();
        if (isSkilled)
        {
            return;
        }
        if (NowLife < MaxLife * 0.5f)
        {
            isSkilled = true;
            Skill();

        }
        if (ScreenDistanceToWall < 7f)
        {
            isSkilled = true;
        }
    }
    public void Skill()
    {
        int originImmunityCount = Config.ImmunityCount;
        Config.ImmunityCount += 999;
        Config.Speed *= 3;
        animator.speed *= 3;
        isSkilling = true;
        ToolManager.Instance.SetTimeout(() =>
        {
            Config.ImmunityCount = originImmunityCount;
            isSkilling = false;
            Config.Speed /= 3;
            animator.speed /= 3;
        }, 2f);
    }
    public override void AddBuff(string buffName, GameObject selfObj, float duration, params object[] args)
    {
        if (!isSkilling)
        {
            base.AddBuff(buffName, selfObj, duration, args);
        }
    }
}