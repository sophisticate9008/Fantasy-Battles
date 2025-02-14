using UnityEngine;

public class Boss2 : EnemyBase
{
    public override void Update()
    {
        base.Update();
        if (Time.time - lastAddBloodTime > 1f)
        {
            lastAddBloodTime = Time.time;
            if(isFreezen) {
                AddLife((int)(MaxLife * 0.01f));
            }
        }


    }
}