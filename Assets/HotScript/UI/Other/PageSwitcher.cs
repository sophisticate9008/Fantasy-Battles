using System.Collections.Generic;
using UnityEngine;

public class PageSwitcher : MonoBehaviour
{
    public List<RectTransform> pages; // 所有页面的 RectTransform
    private int currentPageIndex = 0; // 当前页面索引
    public float transitionDuration = 0.2f; // 动画时间
    public bool ScanChildren = true;

    private void Start()
    {
        if (ScanChildren)
        {
            pages.Clear();
            foreach (Transform child in transform)
            {
                if (child.GetComponent<RectTransform>() != null)
                {
                    pages.Add(child.GetComponent<RectTransform>());
                }
            }
        }
    }

    /// <summary>
    /// 切换到指定页面
    /// </summary>
    /// <param name="newPageIndex">目标页面索引</param>
    /// <param name="isRight">滑动方向，true 为右滑，false 为左滑</param>
    public void SwitchToPage(int newPageIndex, bool isRight)
    {
        if (newPageIndex < 0 || newPageIndex >= pages.Count || newPageIndex == currentPageIndex)
            return; // 无效操作

        RectTransform current = pages[currentPageIndex];
        RectTransform target = pages[newPageIndex];

        // 确保目标页面激活
        target.gameObject.SetActive(true);

        // 根据滑动方向设置目标页面初始位置
        target.anchoredPosition = isRight
            ? new Vector2(Screen.width, 0) // 右滑：目标页面从右侧进入
            : new Vector2(-Screen.width, 0); // 左滑：目标页面从左侧进入

        // 启动切换动画
        StartCoroutine(AnimateSwitch(current, target, newPageIndex));
    }

    private System.Collections.IEnumerator AnimateSwitch(RectTransform current, RectTransform target, int newPageIndex)
    {
        float elapsed = 0f;

        // 动画起始和结束位置
        Vector2 currentStart = current.anchoredPosition;
        Vector2 currentEnd = target.anchoredPosition.x > 0
            ? new Vector2(-Screen.width, 0) // 当前页面移到左侧
            : new Vector2(Screen.width, 0); // 当前页面移到右侧

        Vector2 targetStart = target.anchoredPosition;
        Vector2 targetEnd = Vector2.zero; // 目标页面移到中心

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            // 平滑过渡
            current.anchoredPosition = Vector2.Lerp(currentStart, currentEnd, t);
            target.anchoredPosition = Vector2.Lerp(targetStart, targetEnd, t);

            yield return null;
        }

        // 动画结束后修正位置
        current.anchoredPosition = currentEnd;
        target.anchoredPosition = targetEnd;

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
}
