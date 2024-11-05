using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HybridCLR;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        string defaultHostServer = "https://unity.wdyplus.xyz/fire_at_zombies/";
        string fallbackHostServer = "https://unity.wdyplus.xyz/fire_at_zombies/";
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
            yield return CreateDownloader();
            yield return LoadDllBytes();
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

    IEnumerator LoadDllBytes()
    {
        var assets = new List<string> { "HotUpdate.dll" }.Concat(AOTMetaAssemblyFiles);
        foreach (var asset in assets)
        {
            var handle = package.LoadAssetAsync<TextAsset>(asset);
            yield return handle;
            var assetObj = handle.AssetObject as TextAsset;
            s_assetDatas[asset] = assetObj;
            Debug.Log($"dll:{asset}   {assetObj == null}");
        }
        StartGame();

    }

    /// <summary>
    /// 开始下载
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sizeBytes"></param>
    private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
    {
        Debug.Log(string.Format("开始下载：文件名：{0}，文件大小：{1}", fileName, sizeBytes));
        UpdateUIManager.Instance.UpdateStatus($"开始下载 {fileName}，大小：{sizeBytes / (1024f * 1024f):0.00} MB");
    }

    /// <summary>
    /// 下载完成
    /// </summary>
    /// <param name="isSucceed"></param>
    private void OnDownloadOverFunction(bool isSucceed)
    {
        Debug.Log("下载" + (isSucceed ? "成功" : "失败"));
        UpdateUIManager.Instance.UpdateStatus(isSucceed ? "下载完成！" : "下载失败！");
    }

    /// <summary>
    /// 更新中
    /// </summary>
    /// <param name="totalDownloadCount"></param>
    /// <param name="currentDownloadCount"></param>
    /// <param name="totalDownloadBytes"></param>
    /// <param name="currentDownloadBytes"></param>

    private long lastDownloadedBytes = 0;
    private float lastUpdateTime = 0f;
    private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {

        Debug.Log(string.Format("文件总数：{0}，已下载文件数：{1}，下载总大小：{2}，已下载大小{3}", totalDownloadCount, currentDownloadCount, totalDownloadBytes, currentDownloadBytes));
        float progress = (float)currentDownloadBytes / totalDownloadBytes;
        float currentTime = Time.time;
        float deltaTime = currentTime - lastUpdateTime;

        if (deltaTime > 0.5f) // 每 0.5 秒更新一次速度
        {
            long bytesDownloadedSinceLastUpdate = currentDownloadBytes - lastDownloadedBytes;
            float speed = bytesDownloadedSinceLastUpdate / deltaTime; // 每秒下载的字节数
            string speedText = FormatSpeed(speed);
            UpdateUIManager.Instance.UpdateSpeed(speedText);

            // 更新上次下载的数据
            lastDownloadedBytes = currentDownloadBytes;
            lastUpdateTime = currentTime;
        }

        // 更新 UI 的进度条和进度信息
        UpdateUIManager.Instance.UpdateProgress(progress, $"下载进度：{currentDownloadCount}/{totalDownloadCount}");
    }
    private string FormatSpeed(float speed)
    {
        if (speed > 1024 * 1024)
            return $"{(speed / (1024 * 1024)):0.00} MB/s";
        else if (speed > 1024)
            return $"{(speed / 1024):0.00} KB/s";
        else
            return $"{speed:0.00} B/s";
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
    #region 补充元数据

    //补充元数据dll的列表
    //通过RuntimeApi.LoadMetadataForAOTAssembly()函数来补充AOT泛型的原始元数据
    private static List<string> AOTMetaAssemblyFiles { get; } = new() { };
    private static Dictionary<string, TextAsset> s_assetDatas = new Dictionary<string, TextAsset>();
    private static Assembly _hotUpdateAss;

    public static byte[] ReadBytesFromStreamingAssets(string dllName)
    {
        if (s_assetDatas.ContainsKey(dllName))
        {
            return s_assetDatas[dllName].bytes;
        }

        return Array.Empty<byte>();
    }



    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyFiles)
        {
            byte[] dllBytes = ReadBytesFromStreamingAssets(aotDllName);
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
    #endregion
    

    #region  更新完毕
    void StartGame() {
        // 加载AOT dll的元数据
        LoadMetadataForAOTAssemblies();
        LoadHotUpdateDll();
    }
    private void LoadHotUpdateDll()
    {
        Debug.Log("加载热更资源");
        #if !UNITY_EDITOR
                _hotUpdateAss = Assembly.Load(ReadBytesFromStreamingAssets("HotUpdate.dll"));
        #else
                _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
        #endif
                StartCoroutine(Run_InstantiateComponentByAsset());
    }

    IEnumerator Run_InstantiateComponentByAsset()
    {
        // 通过实例化assetbundle中的资源，还原资源上的热更新脚本
        var package = YooAssets.GetPackage("DefaultPackage");
        var handle = package.LoadAssetAsync<GameObject>("EntryPrefab");
        yield return handle;
        handle.Completed += Handle_Completed;
    }

    private void Handle_Completed(AssetHandle obj)
    {
        Debug.Log("准备实例化");
        GameObject go = obj.InstantiateSync();
        Debug.Log($"Prefab name is {go.name}");
        // SceneManager.LoadScene("Main");
        // LoadScene();
    }
    private void LoadScene()
    {
        string location = "Main";
        var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
        var physicsMode = LocalPhysicsMode.Physics2D;
        // bool suspendLoad = false;
        package.LoadSceneSync(location, sceneMode, physicsMode);
    }
    #endregion
}