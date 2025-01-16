using UnityEngine;
using UnityEngine.UI;

public class FightPage : TheUIBase
{

    public Button next;
    public Button prev;
    public PageSwitcher ps;
    public MissionBase mb => MissionManager.Instance.mb;
    public MissionRecord mr => MissionManager.Instance.mr;
    public int cmpi => MissionManager.Instance.CurrentMaxPassId;
    private void Start() {
        FindNecessary();
        BindButton();
        LoadMapImage(0);

    }
    void FindNecessary() {
        prev = transform.RecursiveFind("上一关").GetComponent<Button>();
        next = transform.RecursiveFind("下一关").GetComponent<Button>();
        ps = transform.RecursiveFind("maps").GetComponent<PageSwitcher>();
    }
    void BindButton() {
        prev.onClick.AddListener(PreLevel);
        next.onClick.AddListener(NextLevel);
    }
    public void GenerateMissionInfo()
    {
        
    }
    public void PreLevel() {
        if(mb.level == 0) {
            UIManager.Instance.OnMessage("已经是第一关了");
            return;
        }
        SwitchLevel(mb.level - 1);
        ps.PreviousPage();
        
    }
    public void NextLevel() {
        if(mb.level >= cmpi) {
            UIManager.Instance.OnMessage("新一关还未解锁");
            return;
        }
        SwitchLevel(mb.level + 1);
        ps.NextPage();
        
    }
    public void SwitchLevel(int id) {
        MissionManager.Instance.mr = MissionManager.Instance.GetMissionRecordById(id);
        ChangeNextImage();
    }

    void LoadMapImage(int childId) {
        ps.transform.GetChild(childId).GetComponent<Image>().sprite = CommonUtil.GetAssetByName<Sprite>("map_" + mb.MapIdToMapName());
    }
    void ChangeNextImage() {
        var npi = (ps.currentPageIndex + 1) % 2;
        LoadMapImage(npi);
    }
}