using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using System.Collections.Generic;

public class AnimatorControllerGenerator : Editor
{
    private const string ClipsFolder = "Assets/AssetPackage/Animator/clips"; // 动画剪辑的路径
    private const string ControllersFolder = "Assets/AssetPackage/Animator/controllers"; // 保存 Animator Controller 的路径

    [MenuItem("Tools/Generate Animator Controllers")]
    public static void GenerateAnimatorControllers()
    {
        Debug.Log("Starting Animator Controller generation...");
        // 查找动画剪辑
        string[] clipFiles = AssetDatabase.FindAssets("t:AnimationClip", new[] { ClipsFolder });
        if (clipFiles.Length == 0)
        {
            Debug.LogWarning("No animation clips found. Please check the clips folder.");
            return;
        }

        HashSet<string> processedMonsters = new HashSet<string>();

        foreach (string clipFile in clipFiles)
        {
            string clipPath = AssetDatabase.GUIDToAssetPath(clipFile);
            string clipName = System.IO.Path.GetFileNameWithoutExtension(clipPath);
            string monsterName = clipName.Split('_')[0]; // 提取怪物名称

            if (processedMonsters.Add(monsterName)) // 确保每个怪物只处理一次
            {
                Debug.Log($"Processing monster: {monsterName}");
                CreateAnimatorController(monsterName);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Animator Controller generation completed!");
    }

    private static void CreateAnimatorController(string monsterName)
    {
        string controllerPath = $"{ControllersFolder}/{monsterName}_Controller.controller";

        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

        // 添加参数
        AddParameter(controller, "isRunning", AnimatorControllerParameterType.Bool);
        AddParameter(controller, "isAttacking", AnimatorControllerParameterType.Bool);

        // 加载动画剪辑
        var clips = new Dictionary<string, AnimationClip>
        {
            { "Idle", LoadAnimationClip($"{monsterName}_Idle") },
            { "Run", LoadAnimationClip($"{monsterName}_Run") },
            { "Attack", LoadAnimationClip($"{monsterName}_Attack") }
        };

        // 创建状态并设置动画
        foreach (var clipPair in clips)
        {
            if (clipPair.Value != null)
            {
                AddStateWithMotion(controller, clipPair.Key, clipPair.Value);
            }
        }

        // 设置默认状态
        controller.layers[0].stateMachine.defaultState = FindState(controller, "Idle");

        // 添加状态过渡
        AddTransition(controller, "Idle", "Run", "isRunning", true);
        AddTransition(controller, "Run", "Idle", "isRunning", false);
        AddTransition(controller, "Idle", "Attack", "isAttacking", true);
        AddTransition(controller, "Attack", "Idle", "isAttacking", false);
    }

    private static void AddParameter(AnimatorController controller, string name, AnimatorControllerParameterType type)
    {
        if (controller.parameters.Length == 0 || !System.Array.Exists(controller.parameters, p => p.name == name))
        {
            controller.AddParameter(name, type);
        }
    }

    private static void AddStateWithMotion(AnimatorController controller, string stateName, AnimationClip clip)
    {
        var state = controller.layers[0].stateMachine.AddState(stateName);
        state.motion = clip;
    }

    private static AnimationClip LoadAnimationClip(string clipName)
    {
        string clipPath = $"{ClipsFolder}/{clipName}.anim";
        var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);

        if (clip == null)
        {
            Debug.LogWarning($"Animation clip not found: {clipPath}");
        }
        else
        {
            Debug.Log($"Loaded animation clip: {clip.name}");
        }

        return clip;
    }

    private static void AddTransition(AnimatorController controller, string fromStateName, string toStateName, string condition, bool conditionValue)
    {
        var fromState = FindState(controller, fromStateName);
        var toState = FindState(controller, toStateName);

        if (fromState == null || toState == null)
        {
            Debug.LogWarning($"Cannot create transition from '{fromStateName}' to '{toStateName}' - state(s) not found.");
            return;
        }

        var transition = fromState.AddTransition(toState);
        transition.AddCondition(conditionValue ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot, 0, condition);
        transition.hasExitTime = false; // 即时过渡
        transition.duration = 0f; // 无过渡时间
    }

    private static AnimatorState FindState(AnimatorController controller, string stateName)
    {
        foreach (var state in controller.layers[0].stateMachine.states)
        {
            if (state.state.name == stateName)
            {
                return state.state;
            }
        }
        return null;
    }
}
