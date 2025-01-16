using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Equipments : TheUIBase
{
    private JewelHandleUIBase EquipmentPanel;//点别的时候复用一个
    private CommonUIBase theCommonUI;//点别的时候复用一个
    public List<Button> EquipmentButtons;
    private void Start()
    {
        EquipmentButtons = transform.GetComponentsInDirectChildren<Button>();
        PlayerDataConfig.OnDataChanged += OnJewelChange;
        BindButtons();
        ChangeEmbedUIColor();
    }

    public override void OnDestroy()
    {
        PlayerDataConfig.OnDataChanged -= OnJewelChange;
    }
    private void OnJewelChange(string fieldName)
    {
        if (fieldName == "jewelChange")
        {
            ChangeEmbedUIColor();
        }
    }
    void ChangeEmbedUIColor()
    {
        foreach (var item in EquipmentButtons)
        {
            List<Image> childImages = item.transform.RecursiveFind("JewelPreview").GetComponentsInDirectChildren<Image>();
            List<JewelBase> jewels = (List<JewelBase>)PlayerDataConfig.GetValue(item.gameObject.name);
            for (int i = 0; i < 5; i++)
            {
                if (i < jewels.Count)
                {
                    Color newColor = ItemUtil.LevelToColor(jewels[i].level);
                    newColor.a = 0.6f;
                    float hdrIntensity = jewels[i].level < 7 ? 1.13954f : 2.456513f;
                    childImages[i].color = newColor * hdrIntensity;
                }
                else
                {
                    Color newColor = new Color(0, 0, 0, 0.5f);
                    childImages[i].color = newColor;
                }
            }
        }
    }
    void BindButtons()
    {
        int idx = 0;
        foreach (var item in EquipmentButtons)
        {
            int _ = ++idx;
            item.transform.GetComponent<Button>().onClick.AddListener(() => ShowPlaceJewel(_));
        }
    }
    void ShowPlaceJewel(int placeId)
    {
        if (EquipmentPanel)
        {
            EquipmentPanel.itemInfo = new ItemBase
            {
                placeId = placeId
            };
            EquipmentPanel.InitByEquipment();
            theCommonUI.titleUI.text = ItemUtil.PlaceIdToPlaceName(placeId);
        }
        else
        {
            GameObject jewelHandle = transform.parent.RecursiveFind("JewelHandle").gameObject;
            GameObject backup = Instantiate(jewelHandle);
            JewelHandleUIBase theUIBase = backup.GetComponent<JewelHandleUIBase>();
            EquipmentPanel = theUIBase;
            theUIBase.itemInfo = new ItemBase
            {
                placeId = placeId
            };
            theUIBase.InitByEquipment();
            theCommonUI = UIManager.Instance.OnCommonUI(ItemUtil.PlaceIdToPlaceName(placeId), theUIBase);
        }

    }
}