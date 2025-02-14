using UnityEngine;

public class Boss1 : EnemyBase
{
    public override void Update()
    {
        base.Update();
        if (Time.time - lastAddBloodTime > 20f)
        {
            lastAddBloodTime = Time.time;
            for (int i = 0; i < 5; i++)
            {
                ToolManager.Instance.SetTimeout(() =>
                {
                    float rate = isFire ? 0.01f : 0.02f;
                    AddLife((int)(MaxLife * rate));
                }, i * 0.5f);
            }


        }


    }
}