using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipExchange: TheUIBase{
    public Transform poolContent;
    public Button confirmBtn;
    public Transform leftTarget;
    public Transform rightTarget;
    private List<string> allArmType;
    public ArmPropBase leftArmPropbase;
    public ArmPropBase rightArmPropbase;
    void BindButton() {
        confirmBtn.onClick.AddListener(OnExchange);
    }
    void OnExchange() {
        var tmp = UIManager.Instance.OnExchange(rightArmPropbase.chipFieldName, leftArmPropbase.chipFieldName, 50, 40);
        tmp.needItem.sprite = CommonUtil.GetAssetByName<Sprite>(leftArmPropbase.chipResName);
    }
    void Start() {
        AutoInjectFields();
        BindButton();
        allArmType = ArmUtil.AllArmTypes;
        GameObject chipItemPrefab = CommonUtil.GetAssetByName<GameObject>("ChipItem");
        foreach (var armType in allArmType) {
            ArmPropBase tmp = new(1, armType);
            DraggableChip draggableChip = Instantiate(chipItemPrefab,poolContent).GetComponent<DraggableChip>();
            draggableChip.armPropBase = tmp;
            draggableChip.targets.Add(leftTarget.GetComponent<RectTransform>());
            draggableChip.targets.Add(rightTarget.GetComponent<RectTransform>());
            draggableChip.gameObject.SetActive(true);
        }
    }
    void Update() {
        if(leftTarget.childCount == 0 || rightTarget.childCount == 0) {
            confirmBtn.interactable = false;
        }else {
            confirmBtn.interactable = true;
            leftArmPropbase = leftTarget.GetChild(0).GetComponent<DraggableChip>().armPropBase;
            rightArmPropbase = rightTarget.GetChild(0).GetComponent<DraggableChip>().armPropBase;
        }
    }
}