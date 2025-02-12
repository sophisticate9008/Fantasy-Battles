using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUpgradePanel : PageSwitcher
{
    public Button confirm;
    public Image chipImg;
    public TextMeshProUGUI chipCount;
    public TextMeshProUGUI moneyCount;
    public Transform upgradePanel;
    public ConsumeBase consumeBase;
    public override void StartCallBack()
    {

        base.StartCallBack();
        AutoInjectFields();
        consumeBase = confirm.GetComponent<ConsumeBase>();
        PlayerDataConfig.OnDataChanged += UpgradeEquipment;
        UpdateInfo(currentPageIndex);
    }

    public override void ClickCallBack(int idx)
    {
        int tmp = 0;
        foreach (Button button in btns)
        {
            Image img = button.transform.GetChild(1).GetComponent<Image>();
            Color color = img.color;
            if(tmp == idx) {
                img.color = new Color(color.r, color.g, color.b, 1);
            }else {
                img.color = new Color(color.r, color.g, color.b, 0);
            }
            
            tmp++;
        }
    }
    public override void SwitchToPage(int newPageIndex, bool isRight)
    {
        GenerateNext(newPageIndex);
        base.SwitchToPage(newPageIndex, isRight);

    }

    void GenerateNext(int idx)
    {
        if(idx == currentPageIndex) {
            return;
        }
        GameObject currentCopy = Instantiate(upgradePanel.gameObject, transform);
        currentCopy.transform.SetSiblingIndex(currentPageIndex);
        upgradePanel.transform.SetSiblingIndex(idx);
        GameObject orgin = pages[idx].gameObject;
        pages[idx] = upgradePanel as RectTransform;
        if(orgin != upgradePanel.gameObject) {
            Destroy(orgin);
        }
        
        pages[currentPageIndex] = currentCopy.transform as RectTransform;
        UpdateInfo(idx);

    }
    void UpgradeEquipment(string fieldName)
    {
        if (fieldName == "money")
        {
            int idx = currentPageIndex;
            UpdateInfo(idx);
        }

    }

    void UpdateInfo(int idx)
    {
        int level = (int)PlayerDataConfig.GetValue("levelPlace" + (idx + 1));
        int moneyNeed = Constant.upgradeMoneyNeed.a1 + level * Constant.upgradeMoneyNeed.d;
        int chipNeed = Constant.upgradeChipNeed.a1 + level * Constant.upgradeChipNeed.d;
        moneyCount.text = ToolManager.Instance.GenerateNeedCountText("money", moneyNeed);
        chipCount.text = ToolManager.Instance.GenerateNeedCountText("equipmentChip" + (idx + 1), chipNeed);
        chipImg.sprite = CommonUtil.GetAssetByName<Sprite>("equipmentChip" + (idx + 1));
        consumeBase.ConsumeItemsData.Clear();
        consumeBase.ConsumeItemsData.Add(("money", moneyNeed));
        consumeBase.ConsumeItemsData.Add(("equipmentChip" + (idx + 1), chipNeed));
        consumeBase.afterAction = () =>
        {
            PlayerDataConfig.UpdateValueAdd("levelPlace" + (idx + 1), 1);
            UpdateInfo(idx);
        };
    }

}