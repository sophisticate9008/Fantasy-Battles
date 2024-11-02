using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using System.Collections.Generic;

public class AnimatorControllerGenerator : Editor
{
    [MenuItem("Tools/Generate Animator Controllers")]
    public static void GenerateAnimatorControllers()
    {
        string clipsFolder = "Assets/AssetPackage/Enemys/clips"; // 动画剪辑的路径
        string controllersFolder = "Assets/AssetPackage/Enemys/AnimatorControllers"; // 保存 Animator Controller 的路径

        // 创建 Animator Controllers 文件夹
        if (!AssetDatabase.IsValidFolder(controllersFolder))
        {
            AssetDatabase.CreateFolder("Assets/AssetPackage/Enemys", "AnimatorControllers");
        }

        // 查找所有动画剪辑
        string[] clipFiles = AssetDatabase.FindAssets("t:AnimationClip", new[] { clipsFolder });
        HashSet<string> processedMonsters = new HashSet<string>();

        foreach (string clipFile in clipFiles)
        {
            string clipPath = AssetDatabase.GUIDToAssetPath(clipFile);
            string clipName = System.IO.Path.GetFileNameWithoutExtension(clipPath);
            string monsterName = clipName.Split('_')[0]; // 提取怪物名称

            // 确保每个怪物名称只处理一次
            if (!processedMonsters.Contains(monsterName))
            {
                CreateAnimatorController(monsterName, controllersFolder);
                processedMonsters.Add(monsterName);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateAnimatorController(string monsterName, string controllersFolder)
    {
        string controllerPath = $"{controllersFolder}/{monsterName}_Controller.controller";
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

        // 添加参数
        controller.AddParameter("isRunning", AnimatorControllerParameterType.Bool);
        controller.AddParameter("isSkill", AnimatorControllerParameterType.Bool);

        // 加载动画剪辑
        AnimationClip idleClip = LoadAnimationClip($"{monsterName}_Idle");
        AnimationClip runClip = LoadAnimationClip($"{monsterName}_Run");
        AnimationClip skillClip = LoadAnimationClip($"{monsterName}_Skill");
        AnimationClip dieClip = LoadAnimationClip($"{monsterName}_Die");

        // 创建状态并添加动画剪辑
        if (idleClip != null) AddStateWithMotion(controller, "Idle", idleClip);
        if (runClip != null) AddStateWithMotion(controller, "Run", runClip);
        if (skillClip != null) AddStateWithMotion(controller, "Skill", skillClip);
        if (dieClip != null) AddStateWithMotion(controller, "Die", dieClip);

        // 设置默认状态
        controller.layers[0].stateMachine.defaultState = controller.layers[0].stateMachine.states[0].state;

        // 设置过渡
        AddTransition(controller, "Idle", "Run", "isRunning", true);
        AddTransition(controller, "Run", "Idle", "isRunning", false);
        AddTransition(controller, "Idle", "Skill", "isSkill", true);
        AddTransition(controller, "Skill", "Idle", "isSkill", false);
    }

    private static void AddStateWithMotion(AnimatorController controller, string stateName, AnimationClip clip)
    {
        AnimatorState state = controller.layers[0].stateMachine.AddState(stateName);
        state.motion = clip;
    }

    private static AnimationClip LoadAnimationClip(string clipName)
    {
        string clipPath = $"Assets/AssetPackage/Enemys/clips/{clipName}.anim";
        Debug.Log($"Attempting to load animation clip at path: {clipPath}");
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);

        if (clip == null)
        {
            Debug.LogError($"Failed to load animation clip at path: {clipPath}");
        }
        else
        {
            Debug.Log($"Successfully loaded animation clip: {clip.name}");
        }

        return clip;
    }

    private static void AddTransition(AnimatorController controller, string fromStateName, string toStateName, string condition = null, bool conditionValue = true)
    {
        AnimatorState fromState = FindState(controller, fromStateName);
        AnimatorState toState = FindState(controller, toStateName);

        if (fromState != null && toState != null)
        {
            var transition = fromState.AddTransition(toState);
            if (!string.IsNullOrEmpty(condition))
            {
                if (conditionValue)
                {
                    transition.AddCondition(AnimatorConditionMode.If, 0, condition);
                }
                else
                {
                    transition.AddCondition(AnimatorConditionMode.IfNot, 0, condition);
                }
            }
        }
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
