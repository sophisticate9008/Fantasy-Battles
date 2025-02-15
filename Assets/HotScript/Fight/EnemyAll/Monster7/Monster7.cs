using UnityEngine;

public class Monster7 : EnemyBase
{
    private int direction = 1;
    private float leftBoundary;
    private float rightBoundary;
    protected override void Start()
    {
        base.Start();
        float cameraHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        leftBoundary = -cameraHalfWidth;
        rightBoundary = cameraHalfWidth;
    }
    public override void IndeedMove()
    {
        transform.Translate(Config.Speed * Time.deltaTime * direction * Vector3.right + Config.Speed * Time.deltaTime * Vector3.down);
        if (transform.position.x >= rightBoundary)
        {
            direction = -1;
        }
        else if (transform.position.x <= leftBoundary)
        {
            direction = 1;
        }

    }
}