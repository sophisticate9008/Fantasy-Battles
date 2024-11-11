// using UnityEditor;
// using UnityEngine;
// using System.IO;

// public class GetHotCode : EditorWindow
// {
//     [MenuItem("YooAsset/Get All HotCode DLLs")]
//     public static void GetAllCodeDllBytes()
//     {
//         // 定义源和目标文件夹路径
//         string sourceDir = Path.Combine(Application.dataPath, "..", "HybridCLRData", "HotUpdateDlls", "Android");
//         string targetDir = Path.Combine(Application.dataPath, "AssetPackage", "Codes", "AotDlls");

//         // 检查源文件夹是否存在
//         if (!Directory.Exists(sourceDir))
//         {
//             Debug.LogError("源文件夹不存在: " + sourceDir);
//             return;
//         }

//         // 检查目标文件夹是否存在，如果不存在则创建
//         if (!Directory.Exists(targetDir))
//         {
//             Directory.CreateDirectory(targetDir);
//         }

//         // 获取源文件夹中的所有 .dll 文件
//         string[] dllFiles = Directory.GetFiles(sourceDir, "*.dll");
//         if (dllFiles.Length == 0)
//         {
//             Debug.LogWarning("源文件夹中没有找到 .dll 文件");
//             return;
//         }

//         // 遍历每个 .dll 文件并复制到目标文件夹中
//         foreach (string dllFile in dllFiles)
//         {
//             string fileName = Path.GetFileName(dllFile);
//             string targetPath = Path.Combine(targetDir, fileName + ".bytes");

//             try
//             {
//                 File.Copy(dllFile, targetPath, true);
//                 Debug.Log("成功复制并重命名文件: " + dllFile + " -> " + targetPath);
//             }
//             catch (System.Exception ex)
//             {
//                 Debug.LogError("复制文件失败: " + ex.Message);
//             }
//         }

//         // 刷新 AssetDatabase 以使文件立即更新
//         AssetDatabase.Refresh();
//     }
// }
using UnityEditor;
using UnityEngine;
using System.IO;

public class GetHotCode : EditorWindow
{
    // 菜单命令添加到 Unity 编辑器
    [MenuItem("YooAsset/Get HotCode")]
    public static void GetCodeDllBytes()
    {
        // 定义源和目标文件路径
        string sourcePath = Path.Combine(Application.dataPath, "..", "HybridCLRData", "HotUpdateDlls", "Android", "HotUpdate.dll");
        string targetPath = Path.Combine(Application.dataPath, "AssetPackage", "Codes","HotDlls", "HotUpdate.dll.bytes");

        // 检查源文件是否存在
        if (!File.Exists(sourcePath))
        {
            Debug.LogError("源文件 HotUpdate.dll 不存在: " + sourcePath);
            return;
        }

        // 检查目标文件夹是否存在，如果不存在则创建
        string targetDir = Path.GetDirectoryName(targetPath);
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }

        // 复制文件并重命名为 HotUpdate.dll.bytes
        try
        {
            File.Copy(sourcePath, targetPath, true);
            Debug.Log("成功复制并重命名文件: " + sourcePath + " -> " + targetPath);
            
            // 刷新 AssetDatabase 以使文件立即更新
            AssetDatabase.Refresh();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("复制文件失败: " + ex.Message);
        }
    }
}
