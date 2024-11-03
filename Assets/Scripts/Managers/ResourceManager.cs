using System.Collections;
using UnityEngine;
using YooAsset;

public class ResourceManager : ManagerBase<ResourceManager>
{
    public EPlayMode PlayMode = EPlayMode.OfflinePlayMode;

    private ResourcePackage package;
    private string packageName = "DefaultPackage";
    private string packageVersion;
    private ResourceDownloaderOperation downloader;
    protected override void AwakeCallBack()
    {
        base.AwakeCallBack();
        DontDestroyOnLoad(gameObject);
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
                    StartCoroutine(HostInitPackage());
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

    private IEnumerator HostInitPackage()
    {
        string defaultHostServer = "http://localhost:8800/fire%20at%20zombies/";
        string fallbackHostServer = "http://localhost:8800/fire%20at%20zombies/";
        IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);

        var buildinFileSystem = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
        var cacheFileSystem = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
        var initParameters = new HostPlayModeParameters
        {
            BuildinFileSystemParameters = buildinFileSystem,
            CacheFileSystemParameters = cacheFileSystem
        };

        var initializationOperation = package.InitializeAsync(initParameters);
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
            downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
            downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
            downloader.OnDownloadOverCallback = OnDownloadOverFunction;
            downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;
            downloader.BeginDownload();
            yield return downloader;

            //检测下载结果
            if (downloader.Status == EOperationStatus.Succeed)
            {
                //下载成功
                Debug.Log("更新完成");
            }
            else
            {
                //下载失败
                Debug.Log("更新失败");
            }
            // PatchEventDefine.FoundUpdateFiles.SendEventMessage(totalDownloadCount, totalDownloadBytes);
        }
    }



    /// <summary>
    /// 开始下载
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sizeBytes"></param>
    private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
    {
        Debug.Log(string.Format("开始下载：文件名：{0}，文件大小：{1}", fileName, sizeBytes));
    }

    /// <summary>
    /// 下载完成
    /// </summary>
    /// <param name="isSucceed"></param>
    private void OnDownloadOverFunction(bool isSucceed)
    {
        Debug.Log("下载" + (isSucceed ? "成功" : "失败"));
    }

    /// <summary>
    /// 更新中
    /// </summary>
    /// <param name="totalDownloadCount"></param>
    /// <param name="currentDownloadCount"></param>
    /// <param name="totalDownloadBytes"></param>
    /// <param name="currentDownloadBytes"></param>
    private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        Debug.Log(string.Format("文件总数：{0}，已下载文件数：{1}，下载总大小：{2}，已下载大小{3}", totalDownloadCount, currentDownloadCount, totalDownloadBytes, currentDownloadBytes));
    }

    /// <summary>
    /// 下载出错
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="error"></param>
    private void OnDownloadErrorFunction(string fileName, string error)
    {
        Debug.Log(string.Format("下载出错：文件名：{0}，错误信息：{1}", fileName, error));
    }



    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    private class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }

        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }

        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }

}