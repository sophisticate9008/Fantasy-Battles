using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallController : TheUIBase
{
    public GameObject walls;
    public Image bar;
    public Text blood;
    public Text bloodChangeText;
    private WallConfig WallConfig => ConfigManager.Instance.GetConfigByClassName("Wall") as WallConfig;
    private RectTransform fillRectTransform;
    private RectTransform textRectTransform;
    private HealthStage currentStage;
    private int lastLife;  // 用于检测血量变化
    private GameObject[] flashOverlays; // 用于存储每个挡位的遮罩
    private enum HealthStage
    {
        High,
        Medium,
        Low,
        Zero
    }

    void Start()
    {
        fillRectTransform = bar.GetComponent<RectTransform>();
        textRectTransform = bloodChangeText.GetComponent<RectTransform>();
        lastLife = WallConfig.CurrentLife;  // 初始化为当前生命值
        CreateMasks();

    }
    private void CreateMasks()
    {
        flashOverlays = new GameObject[3];
        for (int i = 0; i < walls.transform.childCount / 2; i++)
        {
            GameObject currentWall = walls.transform.GetChild(i).gameObject;
            flashOverlays[i] = Instantiate(currentWall, currentWall.transform.parent);
            Image overlayImage = flashOverlays[i].GetComponent<Image>();
            overlayImage.color = new Color(1, 0, 0, 0.2f); // 半透明红色
            overlayImage.enabled = false; // 初始状态为不可见
        }
    }
    private void Update()
    {
        float fillAmount = WallConfig.CurrentLife / (float)WallConfig.LifeMax;
        bar.fillAmount = fillAmount;
        Vector2 textPosition = new Vector2(fillRectTransform.rect.width * (fillAmount - 0.5f) , textRectTransform.anchoredPosition.y);
        textRectTransform.anchoredPosition = textPosition;
        string _ = WallConfig.ImmunityCount > 0 ? $"({WallConfig.ImmunityCount})" : "";
        blood.text = $"<color=#6BE2AF>❤</color> {WallConfig.CurrentLife} <color=yellow>{_}</color>";
        OnMsg();
        OnBloodChange();
        UpdateWallStage();
    }
    private void OnBloodChange()
    {
        if (WallConfig.CurrentLife < lastLife)
        {
            StartCoroutine(FlashRedEffect());
            lastLife = WallConfig.CurrentLife;  // 更新最后的生命值
        }
    }
    private void OnMsg()
    {
        Queue<string> queue = FighteManager.Instance.bloodMsgs;
        if (queue.Count > 0)
        {
            string message = queue.Dequeue();
            StartCoroutine(ShowFloatingText(message));
        }
    }

    private IEnumerator ShowFloatingText(string message)
    {
        Text floatingTextInstance = Instantiate(bloodChangeText, bloodChangeText.transform.parent);
        floatingTextInstance.text = message;
        floatingTextInstance.enabled = true;

        float duration = 0.1f;
        RectTransform instanceRectTransform = floatingTextInstance.GetComponent<RectTransform>();
        Vector2 startPos = instanceRectTransform.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0, 20);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            instanceRectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        floatingTextInstance.enabled = false;
        Destroy(floatingTextInstance.gameObject);
    }

    private void UpdateWallStage()
    {
        float healthPercentage = WallConfig.CurrentLife / (float)WallConfig.LifeMax;
        HealthStage newStage;

        if (healthPercentage > 0.66f)
        {
            newStage = HealthStage.High;
        }
        else if (healthPercentage > 0.33f)
        {
            newStage = HealthStage.Medium;
        }
        else if (healthPercentage > 0 && healthPercentage < 0.33f)
        {
            newStage = HealthStage.Low;
        }
        else
        {
            newStage = HealthStage.Zero;
        }


        if (newStage != currentStage)
        {
            currentStage = newStage;
            ChangeWallPic();
        }
    }

    private void ChangeWallPic()
    {
        for (int i = 0; i < walls.transform.childCount; i++)
        {
            walls.transform.GetChild(i).gameObject.SetActive(false);
        }
        if (currentStage == HealthStage.High)
            walls.transform.GetChild(0).gameObject.SetActive(true);
        else if (currentStage == HealthStage.Medium)
            walls.transform.GetChild(1).gameObject.SetActive(true);
        else if (currentStage == HealthStage.Low)
            walls.transform.GetChild(2).gameObject.SetActive(true);
        else if (currentStage == HealthStage.Zero)
            walls.transform.GetChild(3).gameObject.SetActive(true);
    }

    private IEnumerator FlashRedEffect()
    {
        float flashDuration = 0.5f;  // 闪红效果总持续时间
        int flashCount = 1;          // 闪烁次数

        // 获取对应挡位的遮罩
        GameObject flashOverlay = flashOverlays[(int)currentStage];
        Image overlayImage = flashOverlay.GetComponent<Image>();

        overlayImage.enabled = true; // 启用遮罩

        // 闪烁效果
        for (int i = 0; i < flashCount; i++)
        {
            overlayImage.enabled = true;
            yield return new WaitForSeconds(flashDuration / (flashCount * 2));
            overlayImage.enabled = false;
            yield return new WaitForSeconds(flashDuration / (flashCount * 2));
        }

        overlayImage.enabled = false; // 确保在闪烁后再次禁用
    }
}
