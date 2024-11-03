using UnityEditor;
using UnityEngine;
using System.IO;

public class AnimationClipGenerator : Editor
{
    [MenuItem("Tools/Generate Animation Clips")]
    public static void GenerateAnimationClips()
    {
        string rootFolderPath = "Assets/AssetPackage/Enemys/Animator"; // 替换为你的根文件夹路径
        Debug.Log($"Scanning for monster folders in: {rootFolderPath}");

        string[] monsterFolders = Directory.GetDirectories(rootFolderPath);

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

        var idleFrames = LoadAnimationFrames(monsterFolder, "idle");
        var runFrames = LoadAnimationFrames(monsterFolder, "run");
        var skillFrames = LoadAnimationFrames(monsterFolder, "skill1");
        var deathFrames = LoadAnimationFrames(monsterFolder, "death1");

        if (idleFrames.Length > 0)
            CreateAnimationClip(idleFrames, $"{monsterName}_Idle");
        else
            Debug.LogWarning($"No idle frames found for {monsterName}");

        if (runFrames.Length > 0)
            CreateAnimationClip(runFrames, $"{monsterName}_Run");
        else
            Debug.LogWarning($"No run frames found for {monsterName}");

        if (skillFrames.Length > 0)
            CreateAnimationClip(skillFrames, $"{monsterName}_Skill");
        else
            Debug.LogWarning($"No skill frames found for {monsterName}");

        if (deathFrames.Length > 0)
            CreateAnimationClip(deathFrames, $"{monsterName}_Die");
        else
        {
            deathFrames = LoadAnimationFrames(monsterFolder, "death");
            if (deathFrames.Length > 0)
                CreateAnimationClip(deathFrames, $"{monsterName}_Die");
            else
            {
                Debug.LogWarning($"No death frames found for {monsterName}");
            }
        }
    }

    private static string[] LoadAnimationFrames(string monsterFolder, string action)
    {
        string pattern = $"*-{action}_*.png"; // 匹配动作
        string[] frames = Directory.GetFiles(monsterFolder, pattern);
        Debug.Log($"Searching for action '{action}' in '{monsterFolder}', found: {frames.Length} frames");
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
            path = "", // 根据需要设置路径
            propertyName = "m_Sprite" // 确保使用正确的属性名
        };

        for (int i = 0; i < frames.Length; i++)
        {
            string fileName = Path.GetFileName(frames[i]); // 获取文件名
            string assetPath = Path.Combine(Path.GetDirectoryName(frames[i]), fileName).Replace('\\', '/');

            // 直接加载 Sprite
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite == null)
            {
                Debug.LogWarning($"Could not load sprite at path: {assetPath}");
                continue;
            }

            // 将 Sprite 添加到关键帧
            keyframes[i] = new ObjectReferenceKeyframe { time = i * 0.08f, value = sprite }; // 0.1秒一帧（10fps）
        }

        // 检查是否有有效的关键帧
        if (keyframes.Length == 0)
        {
            Debug.LogWarning($"No valid keyframes created for clip: {clipName}");
            return;
        }

        // 设置关键帧到动画剪辑
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);

        // 保存动画剪辑
        AssetDatabase.CreateAsset(clip, $"Assets/AssetPackage/Enemys/clips/{clipName}.anim");
        Debug.Log($"Created clip: {clipName}");

        // 设置循环时间
        AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
        clipSettings.loopTime = true; // 设置为循环播放
        AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
        AssetDatabase.SaveAssets();
    }


}
