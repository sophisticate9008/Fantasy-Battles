using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class FileUploaderEditor : EditorWindow
{
    private static readonly string serverUrl = "https://unity.wdyplus.xyz/upload";  // 你的服务器地址
    private string assetFolderPath = "";  // 当前选择的文件夹路径

    [MenuItem("YooAsset/Upload Assets")]
    public static void ShowWindow()
    {
        GetWindow<FileUploaderEditor>("Upload Assets");
    }

    private void OnGUI()
    {
        GUILayout.Label("Upload Assets to Server", EditorStyles.boldLabel);

        if (GUILayout.Button("选择目录"))
        {
            // 直接选择文件夹
            string selectedPath = EditorUtility.OpenFolderPanel("选择上传目录", "", "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                // 将选择的完整路径赋值给 assetFolderPath
                assetFolderPath = selectedPath;
            }
        }

        // 显示当前选择的文件夹路径
        if (!string.IsNullOrEmpty(assetFolderPath))
        {
            GUILayout.Label("当前目录: " + assetFolderPath);
        }
        else
        {
            GUILayout.Label("当前目录: 未选择");
        }

        if (GUILayout.Button("上传所有文件") && !string.IsNullOrEmpty(assetFolderPath))
        {
            UploadAllAssets(assetFolderPath);
        }
    }

    private async void UploadAllAssets(string folderPath)
    {
        // 获取选择的文件夹内的所有文件
        string[] files = Directory.GetFiles(folderPath);

        // 遍历文件，逐个上传
        foreach (string filePath in files)
        {
            string fileName = Path.GetFileName(filePath);
            Debug.Log($"准备上传文件: {fileName}");
            // 先检查文件是否存在
            bool fileExists = await CheckFileExists(fileName);
            if (fileExists)
            {
                Debug.Log($"文件已存在，跳过上传: {fileName}");
                continue; // 文件已存在，跳过上传
            }

            await UploadFile(filePath);
        }
    }

    private async Task<bool> CheckFileExists(string fileName)
    {
        // 排除这三个文件
        if (fileName == "OutputCache" || fileName == "OutputCache.manifest" || fileName == "PackageManifest_DefaultPackage.version")
        {
            Debug.Log($"文件 {fileName} 被排除在外，无需检查是否存在。");
            return false; // 直接返回 false，表示文件不存在
        }
        // 使用 UnityWebRequest 包装请求，确保可以与 async/await 一起使用
        using (UnityWebRequest request = UnityWebRequest.Get($"{serverUrl}?filename={fileName}"))
        {
            request.timeout = 10;
            var operation = request.SendWebRequest();  // 异步操作

            await ToUniTask(operation);  // 等待操作完成

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseContent = request.downloadHandler.text;
                Debug.Log($"检查文件是否存在，响应内容: {responseContent}");
                return responseContent.Contains("文件已存在"); // 判断返回内容是否包含"文件已存在"
            }
            else
            {
                Debug.LogError($"检查文件存在时发生错误: {request.error}");
                return false; // 默认文件不存在
            }
        }
    }

    private async Task UploadFile(string filePath)
    {
        // 上传单个文件
        byte[] fileData = File.ReadAllBytes(filePath);

        // 自定义 boundary 字符串
        string boundary = "--------------------------" + DateTime.Now.Ticks.ToString("x");

        // 创建 multipart/form-data 的数据内容
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("--" + boundary);
        sb.AppendLine("Content-Disposition: form-data; name=\"file\"; filename=\"" + Path.GetFileName(filePath) + "\"");
        sb.AppendLine("Content-Type: application/octet-stream");
        sb.AppendLine();

        byte[] headerBytes = Encoding.UTF8.GetBytes(sb.ToString());

        // 使用 UnityWebRequest 上传
        using (UnityWebRequest request = new UnityWebRequest(serverUrl, UnityWebRequest.kHttpVerbPOST))
        {
            // 设置请求头
            request.SetRequestHeader("Content-Type", "multipart/form-data; boundary=" + boundary);

            // 设置上传数据
            byte[] endBoundary = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            byte[] fileBytes = new byte[headerBytes.Length + fileData.Length + endBoundary.Length];

            // 拼接 header + 文件数据 + boundary
            headerBytes.CopyTo(fileBytes, 0);
            fileData.CopyTo(fileBytes, headerBytes.Length);
            endBoundary.CopyTo(fileBytes, headerBytes.Length + fileData.Length);

            request.uploadHandler = new UploadHandlerRaw(fileBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = 30;

            // 发送 POST 请求
            var operation = request.SendWebRequest();  // 异步操作
            await ToUniTask(operation);  // 等待操作完成

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"文件上传成功: {Path.GetFileName(filePath)}");
            }
            else
            {
                Debug.LogError($"文件上传失败: {Path.GetFileName(filePath)}。状态: {request.responseCode}");
                Debug.LogError($"错误内容: {request.error}");
            }
        }
    }

    // 将 UnityWebRequestAsyncOperation 转换为 Task
    private static async Task ToUniTask(UnityWebRequestAsyncOperation operation)
    {
        while (!operation.isDone)
        {
            await Task.Yield();  // Yield to continue waiting
        }
    }
}
