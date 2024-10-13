using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private static AnimatorManager _instance;
    private Dictionary<int, string> animationNames = new();
    public static AnimatorManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("AnimatorManager instance is null!");
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);  // 保证单例
        }
    }

    private void LoadAnimationNames(Animator animator)
    {
        // 假设你有一个 AnimatorController 连接到 animator
        if (animator.runtimeAnimatorController != null)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                int hash = Animator.StringToHash(clip.name);
                animationNames[hash] = clip.name;
            }
        }
    }
    // 播放动画 anim（速度为 speed）
    public void PlayAnim(Animator anim, float speed)
    {
        if (anim != null)
        {
            anim.enabled = true;
            anim.speed = speed;
        }
    }

    // 暂停动画 anim
    public void StopAnim(Animator anim)
    {
        if (anim != null)
        {
            anim.speed = 0;
        }
    }

    // 播放特定的动画 clip
    public void PlaySpecificAnim(Animator anim, string animName, float speed = 1f)
    {
        if (anim != null)
        {
            anim.speed = speed;
            anim.Play(animName);  // 播放指定的动画名
        }
    }

    // 重置动画播放状态
    public void ResetAnim(Animator anim)
    {
        if (anim != null)
        {
            anim.Rebind();  // 重置动画到初始状态
            anim.Update(0);
        }
    }

    // 设置动画参数
    public void SetAnimParameter(Animator anim, string paramName, bool value)
    {
        if (anim != null && HasParameter(anim, paramName))
        {
            anim.SetBool(paramName, value);  // 设置动画布尔参数
        }
    }

    // 检查动画是否有指定参数
    public bool HasParameter(Animator anim, string paramName)
    {
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    // 恢复动画播放
    public void ResumeAnim(Animator anim, float speed = 1f)
    {
        if (anim != null)
        {
            anim.speed = speed;
        }
    }

    // 停止所有动画
    public void StopAllAnim(Animator anim)
    {
        if (anim != null)
        {
            anim.enabled = false;  // 直接禁用 Animator
        }
    }
    public void PlayAnimWithCallback(Animator anim, string animName, Action callback, float speed = 1f)
    {
        if (anim != null)
        {
            anim.speed = speed;
            anim.Play(animName); // 直接播放新动画
            LoadAnimationNames(anim);
            StartCoroutine(WaitForAnimation(anim, animName, callback)); // 等待动画播放完毕
        }
    }

    // 协程等待动画播放完成
    private IEnumerator WaitForAnimation(Animator anim, string animName, Action callback)
    {
        while (true)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // 打印当前播放的动画名称
            // if (animationNames.TryGetValue(stateInfo.shortNameHash, out string tanimName))
            // {
            //     Debug.Log("当前动画: " + tanimName);
            // }

            // 检查当前状态是否为目标动画
            if (stateInfo.IsName(animName))
            {
                // 如果正在播放目标动画，并且没有过渡
                if (stateInfo.normalizedTime >= 1f) // 动画完成
                {
                    callback?.Invoke(); // 执行回调
                    yield break; // 退出协程
                }
            }


            yield return null; // 等待一帧
        }
    }
}
