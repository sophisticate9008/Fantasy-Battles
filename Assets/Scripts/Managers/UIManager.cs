using System.Collections;
using System.Collections.Generic;
using MyBase;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YooAsset;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private Stack<TheUIBase> uiStack = new Stack<TheUIBase>();
    private GameObject uiCanvas;
    private GameObject currentMask; // 当前遮罩

    public GameObject maskPrefab; // 遮罩预制体
    public GameObject MessagePrefab;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        uiCanvas = GameObject.Find("UICanvas");
        if (uiCanvas == null)
        {
            Debug.LogError("UICanvas not found. Please make sure there is a Canvas in the scene.");
        }
    }

    public void ShowUI(TheUIBase ui)
    {
        AddMask();
        // // 隐藏当前 UI（如果存在）
        // if (uiStack.Count > 0)
        // {
        //     var currentUI = uiStack.Peek();
        //     currentUI.gameObject.SetActive(false);
        // }

        uiStack.Push(ui);
        ui.transform.SetParent(uiCanvas.transform, false);
        ui.gameObject.SetActive(true);


        // 添加遮罩

    }
    private IEnumerator AutoCloseMessageUI()
    {
        yield return new WaitForSeconds(0.3f);
        CloseUI();
    }

    public void CloseUI()
    {
        if (uiStack.Count > 0)
        {
            var currentUI = uiStack.Pop();
            Destroy(currentUI.gameObject);

            // // 显示上一个 UI（如果存在）
            // if (uiStack.Count > 0)
            // {
            //     var previousUI = uiStack.Peek();
            //     previousUI.gameObject.SetActive(true);
            // }
        }

        // 移除遮罩
        RemoveMask();
    }

    private void AddMask()
    {
        if (maskPrefab == null) return;
        // 创建遮罩
        currentMask = Instantiate(maskPrefab, uiCanvas.transform);
        // 添加全屏透明遮罩，监听点击事件
        currentMask.AddComponent<MaskListener>().onMaskClicked += OnMaskClicked;
    }

    private void RemoveMask()
    {
        if (currentMask != null)
        {
            Destroy(currentMask);
        }
    }

    private void OnMaskClicked()
    {
        // 点击遮罩后，关闭最上层 UI
        CloseUI();
    }

    // 检测点击是否在UI元素之外

    public void OnMessage(string text)
    {
        if (text == null) return;
        TheUIBase theUIBase = Instantiate(MessagePrefab).GetComponent<TheUIBase>();
        theUIBase.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        ShowUI(theUIBase);
        StartCoroutine(AutoCloseMessageUI());
    }
    public void OnExchange(string goodName, string currencyName,int price)
    {
        GameObject exchangeBasePrefab = YooAssets.LoadAssetSync("ExchangeBase").AssetObject as GameObject;
        ExchangeBase exchangeBase = Instantiate(exchangeBasePrefab).GetComponent<ExchangeBase>();
        exchangeBase.goodName = goodName;
        exchangeBase.currencyName = currencyName;
        exchangeBase.price = price;
        exchangeBase.Init();
        ShowUI(exchangeBase);
    }
}

// 遮罩点击监听器
public class MaskListener : MonoBehaviour, IPointerClickHandler
{
    public delegate void MaskClickAction();
    public event MaskClickAction onMaskClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 如果点击不在 UI 元素上，触发遮罩点击事件
        onMaskClicked?.Invoke();

    }
}