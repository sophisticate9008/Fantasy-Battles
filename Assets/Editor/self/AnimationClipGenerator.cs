using UnityEditor;
using UnityEngine;
using System.IO;

public class AnimationClipGenerator : Editor
{
    private static readonly string RootFolderPath = "Assets/AssetPackage/Animator/pngs"; // 根文件夹路径
    private static readonly string ClipSavePath = "Assets/AssetPackage/Animator/clips"; // 动画剪辑保存路径

    [MenuItem("Tools/Generate Animation Clips")]
    public static void GenerateAnimationClips()
    {
        Debug.Log($"Scanning for monster folders in: {RootFolderPath}");

        if (!Directory.Exists(RootFolderPath))
        {
            Debug.LogError($"Root folder not found: {RootFolderPath}");
            return;
        }

        // 确保保存目录存在
        if (!Directory.Exists(ClipSavePath))
        {
            Directory.CreateDirectory(ClipSavePath);
            Debug.Log($"Created clip save directory: {ClipSavePath}");
        }

        string[] monsterFolders = Directory.GetDirectories(RootFolderPath);

        if (monsterFolders.Length == 0)
        {
            Debug.LogWarning("No monster folders found.");
            return;
        }

        foreach (var monsterFolder in monsterFolders)
        {
            Debug.Log($"Processing folder: {monsterFolder}");
            CreateAnimationClipsForMonster(monsterFolder);
        }
    }

    private static void CreateAnimationClipsForMonster(string monsterFolder)
    {
        string monsterName = Path.GetFileName(monsterFolder);
        Debug.Log($"Creating animation clips for monster: {monsterName}");

        string[] actions = { "Idle", "Run", "Attack" }; // 动作列表
        foreach (var action in actions)
        {
            var frames = LoadAnimationFrames(monsterFolder, action);
            if (frames.Length > 0)
            {
                CreateAnimationClip(frames, $"{monsterName}_{action}");
            }
            else
            {
                Debug.LogWarning($"No frames found for action '{action}' in {monsterName}");
            }
        }
    }

    private static string[] LoadAnimationFrames(string monsterFolder, string action)
    {
        string pattern = $"skeleton-{action}_*.png"; // 匹配动作文件
        string[] frames = Directory.GetFiles(monsterFolder, pattern);
        Debug.Log($"Found {frames.Length} frames for action '{action}' in folder '{monsterFolder}'");
        return frames;
    }

    private static void CreateAnimationClip(string[] frames, string clipName)
    {
        if (frames.Length == 0) return;

        AnimationClip clip = new AnimationClip
        {
            wrapMode = WrapMode.Loop // 设置为循环播放
        };

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[frames.Length];
        EditorCurveBinding curveBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer), // 使用 SpriteRenderer 作为动画类型
            path = "", // 相对于 GameObject 的路径
            propertyName = "m_Sprite" // SpriteRenderer 的 Sprite 属性
        };

        for (int i = 0; i < frames.Length; i++)
        {
            string assetPath = frames[i].Replace('\\', '/'); // 修复路径分隔符

            // 加载 Sprite
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite == null)
            {
                Debug.LogWarning($"Could not load sprite at path: {assetPath}");
                continue;
            }

            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i * 0.08f, // 0.08秒一帧（12.5fps）
                value = sprite
            };
        }

        // 检查是否有有效关键帧
        if (keyframes.Length == 0)
        {
            Debug.LogWarning($"No valid keyframes for clip: {clipName}");
            return;
        }

        // 设置关键帧
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);

        // 保存动画剪辑
        string clipPath = Path.Combine(ClipSavePath, $"{clipName}.anim").Replace('\\', '/');
        AssetDatabase.CreateAsset(clip, clipPath);
        Debug.Log($"Created clip: {clipName} at {clipPath}");

        // 设置循环时间
        AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
        clipSettings.loopTime = true; // 设置为循环播放
        AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
        AssetDatabase.SaveAssets();
    }
}
