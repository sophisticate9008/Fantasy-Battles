
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoSingle : TheUIBase
{
    public int id;
    public string enemyType => EnemyUtil.IdToEnemyType(id);
    public TextMeshProUGUI NameText;
    public Image Pic;
    public Button btn;
    public Animator animator;

    public override void Init()
    {
        base.Init();
        AutoInjectFields();
        btn = GetComponent<Button>();
        animator = Pic.GetComponent<Animator>();
        BindButton();
        InitController();
        NameText.text = EnemyUtil.EnemyTypeToName(enemyType);
    }
    void BindButton()
    {
        btn.onClick.AddListener(ShowDes);
    }
    void InitController()
    {
        animator.runtimeAnimatorController = EnemyUtil.EnemyTypeToController(enemyType);
    }
    void ShowDes()
    {
        UIManager.Instance.OnCommonUI(EnemyUtil.EnemyTypeToName(enemyType), EnemyUtil.EnemyTypeToDes(enemyType));
    }
}