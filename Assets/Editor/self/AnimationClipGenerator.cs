using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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

        string[] actions = { "Idle", "Run", "Attack" };
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
        string pattern = $"skeleton-{action}_*.png";
        string[] frames = Directory.GetFiles(monsterFolder, pattern);
        Debug.Log($"Found {frames.Length} frames for action '{action}' in folder '{monsterFolder}'");
        return frames;
    }

    private static void CreateAnimationClip(string[] frames, string clipName)
    {
        if (frames.Length == 0) return;

        AnimationClip clip = new AnimationClip
        {
            wrapMode = WrapMode.Loop
        };

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[frames.Length];
        EditorCurveBinding spriteRendererBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        EditorCurveBinding imageBinding = new EditorCurveBinding
        {
            type = typeof(Image),
            path = "",
            propertyName = "m_Sprite"
        };

        for (int i = 0; i < frames.Length; i++)
        {
            string assetPath = frames[i].Replace('\\', '/');
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite == null)
            {
                Debug.LogWarning($"Could not load sprite at path: {assetPath}");
                continue;
            }

            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i * 0.08f,
                value = sprite
            };
        }

        if (keyframes.Length == 0)
        {
            Debug.LogWarning($"No valid keyframes for clip: {clipName}");
            return;
        }

        AnimationUtility.SetObjectReferenceCurve(clip, spriteRendererBinding, keyframes);
        AnimationUtility.SetObjectReferenceCurve(clip, imageBinding, keyframes);

        string clipPath = Path.Combine(ClipSavePath, $"{clipName}.anim").Replace('\\', '/');
        AssetDatabase.CreateAsset(clip, clipPath);
        Debug.Log($"Created clip: {clipName} at {clipPath}");

        AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
        clipSettings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
        AssetDatabase.SaveAssets();
    }
}
