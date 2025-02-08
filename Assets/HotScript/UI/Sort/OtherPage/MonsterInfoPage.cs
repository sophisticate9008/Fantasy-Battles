using UnityEngine;

public class MonsterInfoPage: TheUIBase {
    public Transform TujianList;
    public GameObject uiPrefab;
    private void Start() {
        AutoInjectFields();
        uiPrefab = CommonUtil.GetAssetByName<GameObject>("InfoSingle");
        GenerateInfoSingle();

    }
    void GenerateInfoSingle() {
        for(int i = 1; i <= EnemyUtil.MonsterMaxId + EnemyUtil.BossMaxId; i++) {
            GameObject copy = Instantiate(uiPrefab, TujianList);
            copy.SetActive(true);
            InfoSingle infoSingle = copy.AddComponent<InfoSingle>();
            infoSingle.id = i;
            infoSingle.Init();
        }
    }
    
}