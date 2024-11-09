using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YooAsset;

public class BeginPage : TheUIBase
{
    public Button confirmButton;
    public GameObject noticeWindow;
    public Button beginButton;

    private void Start()
    {
        confirmButton.onClick.AddListener(() =>
        {
            noticeWindow.SetActive(false);
        });

        beginButton.onClick.AddListener(BeginGame);
        Debug.Log("开始页面");
    }
    void BeginGame()
    {
        Debug.Log("开始游戏");
        // SceneManager.LoadScene("Fight");
        LoadScene();
    }
    private void LoadScene()
    {
        string location = "Fight";
        var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
        ResourcePackage package = YooAssets.GetPackage("DefaultPackage");
        package.LoadSceneSync(location, sceneMode);
    }
}