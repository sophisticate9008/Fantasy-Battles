using System.Collections;
using System.Collections.Generic;
using UIBase;
using UnityEngine;
using YooAsset;

public class ResourceManager : MonoBehaviour
{
    public EPlayMode PlayMode = EPlayMode.OfflinePlayMode;
    public static ResourceManager Instance { get; private set; }
    private ResourcePackage package;
    private string packageName = "DefaultPackage";
    private string packageVersion;
    private ResourceDownloaderOperation downloader;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 如果已经存在，销毁这个实例
        }
    }
    private void Start()
    {

        YooAssets.Initialize();
        package = YooAssets.TryGetPackage("DefaultPackage");
        package ??= YooAssets.CreatePackage("DefaultPackage");
        YooAssets.SetDefaultPackage(package);
        switch (PlayMode)
        {
            case EPlayMode.EditorSimulateMode:
                {
                    StartCoroutine(EditorInitPackage());
                    break;
                }
            case EPlayMode.OfflinePlayMode:
                {
                    StartCoroutine(SingInitPackage());
                    break;
                }
            case EPlayMode.HostPlayMode:
                {

                    break;
                }
        }

    }
    private IEnumerator EditorInitPackage()
    {
        //注意：如果是原生文件系统选择EDefaultBuildPipeline.RawFileBuildPipeline
        var buildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline;
        var simulateBuildResult = EditorSimulateModeHelper.SimulateBuild(buildPipeline, packageName);
        var editorFileSystem = FileSystemParameters.CreateDefaultEditorFileSystemParameters(simulateBuildResult);
        var initParameters = new EditorSimulateModeParameters();
        initParameters.EditorFileSystemParameters = editorFileSystem;
        var initOperation = package.InitializeAsync(initParameters);
        yield return initOperation;

        if (initOperation.Status == EOperationStatus.Succeed)
            Debug.Log("资源包初始化成功！");
        else
            Debug.LogError($"资源包初始化失败：{initOperation.Error}");
    }
    private IEnumerator SingInitPackage()
    {
        var createParameters = new OfflinePlayModeParameters();
        createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
        var initializationOperation = package.InitializeAsync(createParameters);
        yield return initializationOperation;

        if (initializationOperation.Status == EOperationStatus.Succeed)
            Debug.Log("资源包初始化成功！");
        else
            Debug.LogError($"资源包初始化失败：{initializationOperation.Error}");
        StartCoroutine(UpdatePackageVersion());
    }
    private IEnumerator UpdatePackageVersion()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        var operation = package.RequestPackageVersionAsync();
        yield return operation;
        if (operation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning(operation.Error);

        }
        else
        {
            Debug.Log($"Request package version : {operation.PackageVersion}");
            packageVersion = operation.PackageVersion;
            StartCoroutine(UpdateManifest());
        }
    }
    private IEnumerator UpdateManifest()
    {
        yield return new WaitForSecondsRealtime(0.5f);


        var package = YooAssets.GetPackage(packageName);
        var operation = package.UpdatePackageManifestAsync(packageVersion);
        yield return operation;

        if (operation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning(operation.Error);
            yield break;
        }
        else
        {
            StartCoroutine(CreateDownloader());
        }
    }
    IEnumerator CreateDownloader()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        var package = YooAssets.GetPackage(packageName);
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

        if (downloader.TotalDownloadCount == 0)
        {
            Debug.Log("Not found any download files !");
        }
        else
        {
            // 发现新更新文件后，挂起流程系统
            // 注意：开发者需要在下载前检测磁盘空间不足
            int totalDownloadCount = downloader.TotalDownloadCount;
            long totalDownloadBytes = downloader.TotalDownloadBytes;
            // PatchEventDefine.FoundUpdateFiles.SendEventMessage(totalDownloadCount, totalDownloadBytes);
        }
    }

}