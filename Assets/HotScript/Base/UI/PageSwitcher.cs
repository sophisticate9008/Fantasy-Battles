using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageSwitcher : MonoBehaviour
{
    public int initIdx = 0;
    public List<RectTransform> pages; // 所有页面的 RectTransform
    public int currentPageIndex = 0; // 当前页面索引
    public float transitionDuration = 0.2f; // 动画时间
    public bool ScanChildren = true;

    public Transform btnsParent;
    public List<Button> btns;
    private int lastIdx = -1;

    public virtual void Start()
    {
        if (ScanChildren)
        {
            pages.Clear();
            foreach (Transform child in transform)
            {
                if (child.GetComponent<RectTransform>() != null)
                {
                    pages.Add(child.GetComponent<RectTransform>());
                    child.gameObject.SetActive(false);
                }
            }
        }

        if (btnsParent != null)
        {
            BindBtns();
        }
        StartCallBack();
        if (btnsParent != null)
        {
            InitActive();
        }

    }
    public virtual void InitActive()
    {
        pages[initIdx].gameObject.SetActive(true);
        float tmp = transitionDuration;
        transitionDuration = 0;
        btns[initIdx].onClick.Invoke();
        transitionDuration = tmp;
    }
    public virtual void StartCallBack()
    {

    }

    /// <summary>
    /// 绑定按钮并设置点击事件
    /// </summary>
    public void BindBtns()
    {
        btns = btnsParent.GetComponentsInDirectChildren<Button>();
        for (int i = 0; i < btns.Count; i++)
        {

            int idx = i;
            btns[idx].onClick.AddListener(() =>
            {
                if (idx == lastIdx)
                {
                    return;
                }
                bool isRight = idx > lastIdx;
                lastIdx = idx;
                SwitchToPage(idx, isRight);
                ClickCallBack(idx);
            });
        }
    }

    /// <summary>
    /// 切换到指定页面
    /// </summary>
    public void SwitchToPage(int newPageIndex, bool isRight)
    {
        if (newPageIndex < 0 || newPageIndex >= pages.Count || newPageIndex == currentPageIndex)
            return; // 无效操作

        RectTransform current = pages[currentPageIndex];
        RectTransform target = pages[newPageIndex];
        // 确保目标页面激活
        target.gameObject.SetActive(true);

        // 设置目标页面初始位置（保持 y 坐标不变）
        target.anchoredPosition = isRight
            ? new Vector2(Screen.width, current.anchoredPosition.y) // 右滑：目标页面从右侧进入，保持 y 坐标
            : new Vector2(-Screen.width, current.anchoredPosition.y); // 左滑：目标页面从左侧进入，保持 y 坐标

        // 确保目标页面和当前页面都有 CanvasGroup
        CanvasGroup currentCanvasGroup = current.GetComponent<CanvasGroup>();
        if (currentCanvasGroup == null)
        {
            currentCanvasGroup = current.gameObject.AddComponent<CanvasGroup>();
        }

        CanvasGroup targetCanvasGroup = target.GetComponent<CanvasGroup>();
        if (targetCanvasGroup == null)
        {
            targetCanvasGroup = target.gameObject.AddComponent<CanvasGroup>();
        }

        // 启动切换动画
        StartCoroutine(AnimateSwitch(current, target, newPageIndex, currentCanvasGroup, targetCanvasGroup));
    }

    private System.Collections.IEnumerator AnimateSwitch(RectTransform current, RectTransform target, int newPageIndex, CanvasGroup currentCanvasGroup, CanvasGroup targetCanvasGroup)
    {
        float elapsed = 0f;

        // 动画起始和结束位置
        Vector2 currentStart = current.anchoredPosition;
        Vector2 currentEnd = new Vector2(
            target.anchoredPosition.x > 0 ? -Screen.width : Screen.width,
            current.anchoredPosition.y // 保持 y 坐标不变
        );

        Vector2 targetStart = target.anchoredPosition;
        Vector2 targetEnd = new Vector2(0, target.anchoredPosition.y); // 保持目标页面的 y 坐标

        // 设置初始透明度
        currentCanvasGroup.alpha = 1f;
        targetCanvasGroup.alpha = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / transitionDuration;
            // 平滑过渡
            current.anchoredPosition = Vector2.Lerp(currentStart, currentEnd, t);
            target.anchoredPosition = Vector2.Lerp(targetStart, targetEnd, t);
            
            // 淡入淡出效果
            currentCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            targetCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        // 动画结束后修正位置和透明度
        current.anchoredPosition = currentEnd;
        target.anchoredPosition = targetEnd;
        currentCanvasGroup.alpha = 0f;
        targetCanvasGroup.alpha = 1f;

        // 关闭当前页面
        current.gameObject.SetActive(false);

        // 更新当前页面索引
        currentPageIndex = newPageIndex;
    }

    /// <summary>
    /// 切换到下一个页面
    /// </summary>
    public void NextPage()
    {
        int newIndex = (currentPageIndex + 1) % pages.Count;
        SwitchToPage(newIndex, true); // 右滑
    }

    /// <summary>
    /// 切换到上一个页面
    /// </summary>
    public void PreviousPage()
    {
        int newIndex = (currentPageIndex - 1 + pages.Count) % pages.Count;
        SwitchToPage(newIndex, false); // 左滑
    }

    /// <summary>
    /// 按钮点击回调，可重写实现自定义逻辑
    /// </summary>
    public virtual void ClickCallBack(int idx)
    {
    }
}
