using UnityEngine;
using UnityEngine.UI;

public class BeginPage : TheUIBase {
    public Button confirmButton;
    public GameObject noticeWindow;
    public Button beginButton;

    private void Start() {
        confirmButton.onClick.AddListener(() => {
            noticeWindow.SetActive(false);
        });

        beginButton.onClick.AddListener(BeginGame);
    }
    void BeginGame() {
        
    }
}