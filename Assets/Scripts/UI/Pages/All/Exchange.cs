using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exchange : MonoBehaviour
{
    // Start is called before the first frame update    
    public string goodName;
    public int price;
    public string currencyName;
    public int goodCount = 2;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ToExchange);
    }
    private void ToExchange()
    {
        UIManager.Instance.OnExchange(goodName, currencyName, price, goodCount);
    }
}
