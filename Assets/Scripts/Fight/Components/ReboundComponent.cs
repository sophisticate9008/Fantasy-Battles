using ArmConfigs;
using MyBase;
using UnityEngine;

public class ReboundComponent : ComponentBase, IReboundable
{
    public int ReboundCount { get; set; }
    private float edgeBuffer = 0.01f; // 设置屏幕边缘的缓冲区，当接近该缓冲区时会反弹

    public ReboundComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {

    }
    public override void Init()
    {
        base.Init();
        ReboundCount = (ConfigManager.Instance.GetConfigByClassName("Bullet") as BulletConfig).ReboundCount;
    }
    public void Rebound()
    {
        if (ReboundCount <= 0) return;  // 反弹次数用完则不再反弹

        Vector3 position = SelfObj.transform.position;
        Vector2 direction = SelfObj.GetComponent<IArmChild>().Direction;

        // 获取物体在视口中的位置
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(position);

        // 检查是否接近屏幕边缘并反转方向
        bool rebounded = false;

        if (viewportPos.x < edgeBuffer || viewportPos.x > (1 - edgeBuffer))
        {
            direction.x = -direction.x; // 反转 x 方向
            rebounded = true;
        }

        if (viewportPos.y < edgeBuffer || viewportPos.y > (1 - edgeBuffer))
        {
            direction.y = -direction.y; // 反转 y 方向
            rebounded = true;
        }

        // 如果发生了反弹，则更新方向并减少反弹次数
        if (rebounded)
        {
            SelfObj.GetComponent<IArmChild>().Direction = direction;
            ReboundCount--;
        }
    }

    public override void TriggerExec(GameObject enemyObj)
    {
        Rebound();
    }
}
