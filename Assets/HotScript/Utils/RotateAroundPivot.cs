using UnityEngine;

public class RotateAroundPivot : MonoBehaviour
{
    public float rotationSpeed = 10f;  // 旋转速度，单位：度/秒

    void Update()
    {
        // 绕物体自身的中心点旋转
        // 使用物体的局部坐标系进行旋转
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
