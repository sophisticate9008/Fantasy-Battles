using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BloodControl : TheUIBase
{
    // Start is called before the first frame update
    public EnemyBase enemyObj;
    public bool isInit = false;
    public Transform BloodUI;
    public Image[] BloodLayers;
    public TextMeshProUGUI textMeshProUGUI;

    Camera mainCamera;
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        mainCamera = Camera.main;
        enemyObj = transform.parent.GetComponent<EnemyBase>();
        // 将敌人的世界坐标转换为屏幕空间坐标
        BloodUI = transform.RecursiveFind("血条UI");
        isInit = true;
        BloodLayers = new Image[2];
        BloodLayers[0] = transform.RecursiveFind("血条红").GetComponent<Image>();
        BloodLayers[1] = transform.RecursiveFind("血条橙").GetComponent<Image>();
        textMeshProUGUI = transform.GetComponentInChildren<TextMeshProUGUI>();
    }
    void UpdatePosition()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        BloodUI.position = screenPosition;

    }
    void UpdateBloodUI()
    {
        // 每条血量
        int bloodPerBar = enemyObj.MaxLife / enemyObj.Config.BloodBarCount;
        // 当前血条数
        int bloodBarCountNow = (enemyObj.NowLife - 1) / bloodPerBar + 1; //90001 是 10, 90000是9
        textMeshProUGUI.text = "x" + bloodBarCountNow;

        // 当前血条血量
        int bloodBarNow = enemyObj.NowLife % bloodPerBar;
        if (bloodBarNow == 0 && enemyObj.NowLife == bloodPerBar * bloodBarCountNow) // 修正满血条情况，
        {
            bloodBarNow = bloodPerBar;
        }

        // 当前血量比率 (使用浮点数计算)
        float bloodBarRatio = (float)bloodBarNow / bloodPerBar;
        int showIdx = bloodBarCountNow % 2;
        int backIdx = (showIdx + 1) % 2;
        BloodLayers[showIdx].fillAmount = bloodBarRatio;
        BloodLayers[backIdx].fillAmount = bloodBarCountNow == 1 ? 0 : 1;
        BloodLayers[showIdx].transform.SetSiblingIndex(1);
        BloodLayers[backIdx].transform.SetSiblingIndex(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit)
        {
            return;
        }
        UpdatePosition();
        UpdateBloodUI();
    }
}
