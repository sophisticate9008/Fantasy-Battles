using UnityEngine;
using UnityEngine.UI;

public class RangeFireShow : TheUIBase
{
    private Camera mainCamera; // 主摄像机
    private Image Mask;
    public float rangeFire = 0;
    private void Start()
    {
        Mask = GetComponent<Image>();
        maskRectTransform = Mask.GetComponent<RectTransform>();
    }
    RectTransform maskRectTransform;
    float ConvertWorldDistanceToScreenDistance(float worldDistance, float depth = 0)
    {
        mainCamera = mainCamera != null ? mainCamera : Camera.main;
        if (mainCamera.orthographic)
        {
            // 正交投影：屏幕距离与世界距离是线性关系
            float orthoSize = mainCamera.orthographicSize;
            float screenHeight = 2 * orthoSize; // 世界单位中的屏幕高度
            float pixelsPerUnit = Screen.height / screenHeight; // 每单位世界距离对应的像素数
            return worldDistance * pixelsPerUnit;
        }
        else
        {
            // 透视投影：根据视锥体计算屏幕距离
            float halfFOV = mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad;
            float screenHeightAtDepth = 2 * depth * Mathf.Tan(halfFOV); // 世界单位中的屏幕高度
            float pixelsPerUnit = Screen.height / screenHeightAtDepth; // 每单位世界距离对应的像素数
            return worldDistance * pixelsPerUnit;
        }
    }
    public void Show(float rangeFire)
    {
        gameObject.SetActive(true);
        UIManager.Instance.OnListenedToclose(this.gameObject, isSkipSelf: true);
        this.rangeFire = rangeFire;
        float screenDistance = ConvertWorldDistanceToScreenDistance(rangeFire);
        Mask = Mask != null ? Mask : GetComponent<Image>();
        maskRectTransform = maskRectTransform != null ? maskRectTransform : Mask.GetComponent<RectTransform>();
        maskRectTransform.sizeDelta = new Vector2(screenDistance * 2, screenDistance * 2);
    }
}