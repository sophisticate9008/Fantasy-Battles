using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    public Image healthFill;
    public Text level;
    void Update()
    {
        healthFill.fillAmount = (float)FighteManager.Instance.exp / FighteManager.Instance.CurrentNeedExp;
        level.text = "LV" + FighteManager.Instance.level;
    }
}