using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggableChip: DraggableToTarget {
    private GridLayoutGroup glg;
    public TextMeshProUGUI chipCount;
    public ArmPropBase armPropBase;
    public override void BeginDrag()
    {
        base.BeginDrag();
        glg.enabled = false;
    }
    public override void ArriveTarget(RectTransform target)
    {
        base.ArriveTarget(target);
        glg.enabled = true;
    }
    public override void Start() {
        base.Start();
        AutoInjectFields();
        glg = transform.parent.GetComponent<GridLayoutGroup>();
        UpdateCount(armPropBase.chipFieldName);
        PlayerDataConfig.OnDataChanged += UpdateCount;
        GetComponent<Image>().sprite = CommonUtil.GetAssetByName<Sprite>(armPropBase.chipResName);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        PlayerDataConfig.OnDataChanged -= UpdateCount;

    }
    void UpdateCount(string fieldName) {
        if(fieldName == armPropBase.chipFieldName) {
            chipCount.text = PlayerDataConfig.GetValue(armPropBase.chipFieldName).ToString();
        }
    }
}