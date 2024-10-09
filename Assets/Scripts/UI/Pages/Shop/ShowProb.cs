using UnityEngine;
using UnityEngine.UI;

public class ShowProb : TheUIBase
{
    public enum ProbabilityType
    {
        ProbDict1, // 使用字典1
        ProbDict2  // 使用字典2
    }
    [SerializeField] private ProbabilityType probabilitySelection = ProbabilityType.ProbDict1;

    public void Show() {
        if (probabilitySelection == ProbabilityType.ProbDict1) {
            UIManager.Instance.OnCommonUI("蓝色宝箱概率", LevelUtil.ProbDictToString(LevelUtil.probDictBlue));
        }else {
            UIManager.Instance.OnCommonUI("紫色宝箱概率", LevelUtil.ProbDictToString(LevelUtil.probDictPurple));
        }
    }
    private void Start() {
        GetComponent<Button>().onClick.AddListener(Show);
    }
}