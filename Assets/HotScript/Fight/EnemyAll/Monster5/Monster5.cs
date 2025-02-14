using UnityEngine;

public class Monster5 : EnemyBase
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ArmChildBase armChildBase = other.transform.GetComponent<ArmChildBase>();
        if (armChildBase.Config.ComponentStrs.Contains("穿透"))
        {
            ToolManager.Instance.SetTimeout(() =>
            {
                armChildBase.Direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            }, 0.05f);

        }
    }
}