using System;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIManager : MonoBehaviour
{
    public static UpdateUIManager Instance { get; private set; }
    protected virtual void Awake()
    {
        // 检查是否已经有实例存在
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 防止创建多个实例
        }
        else
        {
            Instance = this;
        }
    }
    public Image progressBar; // 进度条的 Image 组件
    public Text progressText;
    public Text statusMessage;
    public Text speedText;
    public Button cancelButton;
    public Button confirmButton;
    public Text updateInfo;
    public GameObject dialog;

    public Action<bool> OnUpdateConfirmed;
    public void UpdateProgress(float progress, string message)
    {
        // 设置填充进度
        progressBar.fillAmount = progress;

        // 更新进度文本内容
        progressText.text = $"{progress * 100f:0.0}%";
        statusMessage.text = message;

        // 根据填充度调整文本的位置
        Vector3 progressBarPosition = progressBar.rectTransform.position;
        float barWidth = progressBar.rectTransform.rect.width;
        progressText.rectTransform.position = new Vector3(
            progressBarPosition.x - (barWidth / 2) + (progress * barWidth),
            progressBarPosition.y,
            progressBarPosition.z
        );
    }
    public void ShowDialog(string text)
    {
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            // 用户点击确认
            OnUpdateConfirmed?.Invoke(true);  // 触发事件，传递 true 表示确认
            dialog.SetActive(false);  // 隐藏弹窗
        });

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            // 用户点击取消
            OnUpdateConfirmed?.Invoke(false);  // 触发事件，传递 false 表示取消
            dialog.SetActive(false);  // 隐藏弹窗
        });
        updateInfo.text = text;
        dialog.SetActive(true);  // 显示弹窗
    }
    public void UpdateSpeed(string speed)
    {
        speedText.text = speed;
    }

    public void UpdateStatus(string message)
    {
        statusMessage.text = message;
    }
}
