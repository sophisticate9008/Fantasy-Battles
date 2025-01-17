using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YooAsset;

public class UIManager : ManagerBase<UIManager>
{
    private static UIManager _instance;


    private readonly Stack<TheUIBase> uiStack = new();
    private readonly Stack<TheUIBase> maskStack = new();
    private GameObject uiCanvas;
    private GameObject currentMask; // 当前遮罩

    private GameObject listenedToClose;
    private string[] excluedTypes = new string[0];
    private bool enableClick = true;
    protected override void AwakeCallBack()
    {
        base.AwakeCallBack();
        typeof(ExecuteEvents).GetField("s_PointerClickHandler", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new ExecuteEvents.EventFunction<IPointerClickHandler>(OnPointerClick));
        // 拦截鼠标按下事件
        // typeof(ExecuteEvents).GetField("s_PointerDownHandler", BindingFlags.NonPublic | BindingFlags.Static)
        //     .SetValue(null, new ExecuteEvents.EventFunction<IPointerDownHandler>(OnPointerDown));
        DontDestroyOnLoad(gameObject);
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
            currentUI.gameObject.SetActive(false);
            ToolManager.Instance.SetTimeout(() =>
            {
                currentUI.SelfDestory();
            }, 0.1f);

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
        GameObject maskPrefab = CommonUtil.GetAssetByName<GameObject>("UIMask");
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
        GameObject MessagePrefab = CommonUtil.GetAssetByName<GameObject>("Message");
        TheUIBase theUIBase = Instantiate(MessagePrefab).AddComponent<TheUIBase>();
        TextMeshProUGUI textMeshProUGUI = theUIBase.transform.RecursiveFind("Text").GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = text;
        ShowUI(theUIBase);
        StartCoroutine(AutoCloseMessageUI());
    }
    public void OnExchange(string goodName, string currencyName, int price, int goodCount = 1)
    {
        GameObject exchangeBasePrefab = CommonUtil.GetAssetByName<GameObject>("ExchangeBase");
        ExchangeBase exchangeBase = Instantiate(exchangeBasePrefab).AddComponent<ExchangeBase>();
        exchangeBase.goodName = goodName;
        exchangeBase.currencyName = currencyName;
        exchangeBase.price = price;
        exchangeBase.goodCount = goodCount;
        exchangeBase.Init();
        ShowUI(exchangeBase);
    }
    public void OnCommonUI(string title, string text)
    {
        GameObject CommonUIPrefab = CommonUtil.GetAssetByName<GameObject>("CommonUI");
        CommonUIBase commonUIBase = Instantiate(CommonUIPrefab).AddComponent<CommonUIBase>();
        commonUIBase.title = title;
        commonUIBase.innerText = text;
        commonUIBase.Init();
        ShowUI(commonUIBase);
    }
    public CommonUIBase OnCommonUI(string title, TheUIBase ui)
    {
        ui.gameObject.SetActive(true);
        GameObject CommonUIPrefab = CommonUtil.GetAssetByName<GameObject>("CommonUI");
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
    public TheUIBase OnItemUIShow<T>(string title, List<T> items, float delay = 0.02f, Action action = null) where T : ItemBase
    {
        // 创建一个 ItemBase 类型的备份列表
        List<ItemBase> itemBaseList = items.Cast<ItemBase>().ToList();

        GameObject ItemUIShow = CommonUtil.GetAssetByName<GameObject>("ItemUIShow");
        TheUIBase theUIBase = Instantiate(ItemUIShow).AddComponent<TheUIBase>();
        Transform parent = theUIBase.transform.RecursiveFind("Content");
        int idx = 0;

        foreach (ItemBase item in itemBaseList)
        {
            void _()
            {
                if (parent == null || theUIBase == null)
                {
                    return;
                }

                // 从池中获取 ItemUI 并初始化
                var itemUI = ToolManager.Instance.GetItemUIFromPool();
                itemUI.itemInfo = item;
                itemUI.Init();

                // 设置父物体
                itemUI.transform.SetParent(parent);


            }
            ToolManager.Instance.SetTimeout(_, delay * idx++);
        }

        // 刷新布局
        ToolManager.Instance.SetTimeout(() =>
        {
            parent.gameObject.SetActive(false);
            parent.gameObject.SetActive(true);
        }, delay * idx++);

        // 设置通用 UI
        if (action == null)
        {
            OnCommonUI(title, theUIBase);
        }
        else
        {
            OnCommonUI(title, theUIBase, action);
        }

        return theUIBase;
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
