
using UnityEngine;
using UnityEngine.UI;

public class ConsumeBase : TheUIBase, IConsume
{
    public int _ConsumeCount;
    public string _itemName;

    public virtual string ItemName { get => _itemName; set => _itemName = value; }
    public virtual int ConsumeCount { get => _ConsumeCount; set => _ConsumeCount = value; }
    public virtual void PreConsume()
    {
        if ((int)PlayerDataConfig.GetValue(ItemName) >= ConsumeCount)
        {
            if(PostConsume()) {
                AfterConsume();
            }
        }else {
            UIManager.Instance.OnMessage("Not enough " + ItemName);
            Debug.Log("Not enough " + ItemName);
        }
    }
    public virtual bool PostConsume()
    {
        return true;
    }
    public virtual void AfterConsume()
    {
        PlayerDataConfig.UpdateValue(ItemName, (int)PlayerDataConfig.GetValue(ItemName) - ConsumeCount);
    }
    private void Start() {
        BindButton();
    }
    public virtual void BindButton() {
        GetComponent<Button>().onClick.AddListener(PreConsume);
    }
}