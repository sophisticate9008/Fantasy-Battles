using UnityEngine;
using UnityEngine.UI;

public class UpdateUIManager : ManagerBase<UpdateUIManager>
{
    public Image progressBar; // 进度条的 Image 组件
    public Text progressText;
    public Text statusMessage;
    public Text speedText;

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

    public void UpdateSpeed(string speed)
    {
        speedText.text = speed;
    }

    public void UpdateStatus(string message)
    {
        statusMessage.text = message;
    }
}
