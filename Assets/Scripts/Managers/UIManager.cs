using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YooAsset;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private Stack<TheUIBase> uiStack = new Stack<TheUIBase>();
    private Stack<TheUIBase> maskStack = new Stack<TheUIBase>();
    private GameObject uiCanvas;
    private GameObject currentMask; // 当前遮罩

    public GameObject maskPrefab; // 遮罩预制体
    public GameObject MessagePrefab;
    public GameObject listenedToClose;
    public string[] excluedTypes;
    public bool enableClick = true;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //拦截鼠标点击事件
            typeof(ExecuteEvents).GetField("s_PointerClickHandler", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new ExecuteEvents.EventFunction<IPointerClickHandler>(OnPointerClick));
            // 拦截鼠标按下事件
            // typeof(ExecuteEvents).GetField("s_PointerDownHandler", BindingFlags.NonPublic | BindingFlags.Static)
            //     .SetValue(null, new ExecuteEvents.EventFunction<IPointerDownHandler>(OnPointerDown));
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
    // private void OnPointerDown(IPointerDownHandler handler, BaseEventData eventData)
    // {
    //     PointerEventData pointerEventData = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
    //     if (pointerEventData != null)
    //     {
    //         // 检测条件，设置 preventClick
    //         return;
    //     }
    // }
    void OnPointerClick(IPointerClickHandler handler, BaseEventData eventData)
    {
        PointerEventData pointerEventData = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
        if (pointerEventData != null)
        {
            if (enableClick)
            {
                handler.OnPointerClick(pointerEventData);
            }

        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            //如果包含自身面板，则不处理
            foreach (var result in results)
            {
                if (result.gameObject == listenedToClose)
                {
                    return;
                }
            }
            //排除列表
            foreach (var result in results)
            {
                foreach (string exclude in excluedTypes)
                {
                    // 获取点击对象的组件
                    var component = result.gameObject.GetComponent(exclude);
                    if (component != null)
                    {
                        // 包含在排除列表中，跳过关闭面板
                        return;
                    }
                }
            }
            if (listenedToClose != null && listenedToClose.activeSelf == true)
            {
                enableClick = false;
                OnListenedclose();
                ToolManager.Instance.SetTimeout(() => enableClick = true, 0.3f);
            }
            //否则关闭


        }
    }

    public void OnListenedToclose(GameObject theUI, string[] excluedTypes)
    {
        listenedToClose = theUI;
        this.excluedTypes = excluedTypes;
    }
    private void OnListenedclose()
    {
        listenedToClose.SetActive(false);
        listenedToClose = null;

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
        maskStack.Push(currentMask.GetComponent<TheUIBase>());
    }

    private void RemoveMask()
    {
        if (maskStack.Count > 0)
        {
            currentMask = maskStack.Pop().gameObject;
            if (currentMask != null)
            {
                Destroy(currentMask);
            }
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
        theUIBase.gameObject.transform.GetChild(0).GetComponent<Text>().text = text;
        ShowUI(theUIBase);
        StartCoroutine(AutoCloseMessageUI());
    }
    public void OnExchange(string goodName, string currencyName, int price, int goodCount = 1)
    {
        GameObject exchangeBasePrefab = YooAssets.LoadAssetSync("ExchangeBase").AssetObject as GameObject;
        ExchangeBase exchangeBase = Instantiate(exchangeBasePrefab).GetComponent<ExchangeBase>();
        exchangeBase.goodName = goodName;
        exchangeBase.currencyName = currencyName;
        exchangeBase.price = price;
        exchangeBase.goodCount = goodCount;
        exchangeBase.Init();
        ShowUI(exchangeBase);
    }
    public void OnCommonUI(string title, string text)
    {
        GameObject CommonUIPrefab = YooAssets.LoadAssetSync("CommonUI").AssetObject as GameObject;
        CommonUIBase commonUIBase = Instantiate(CommonUIPrefab).AddComponent<CommonUIBase>();
        commonUIBase.title = title;
        commonUIBase.innerText = text;
        commonUIBase.Init();
        ShowUI(commonUIBase);
    }
    public CommonUIBase OnCommonUI(string title, TheUIBase ui)
    {
        ui.gameObject.SetActive(true);
        GameObject CommonUIPrefab = YooAssets.LoadAssetSync("CommonUI").AssetObject as GameObject;
        CommonUIBase commonUIBase = Instantiate(CommonUIPrefab).AddComponent<CommonUIBase>();
        commonUIBase.title = title;
        commonUIBase.Init();
        commonUIBase.ReplaceInner(ui);
        ShowUI(commonUIBase);
        return commonUIBase;
    }
    public void OnCommonUI(string title, TheUIBase ui, Action action)
    {
        CommonUIBase commonUIBase = OnCommonUI(title, ui);
        Button confirm = commonUIBase.transform.RecursiveFind("Confirm").GetComponent<Button>();
        confirm.gameObject.SetActive(true);
        confirm.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
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
        Destroy(gameObject);
    }
}
