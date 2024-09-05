using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    public TMP_Text moneyText;
    public int money = 1000;

    void Start()
    {
        moneyText.text = money.ToString();
    }
    
    public void AddMoney(int money)
    {
        this.money += money;
    }
    
    public void MinusMoney(int money)
    {
        this.money -= money;
    }
    
    private void Update()
    {
        moneyText.text = "Money:"  + money;
    }

    
}
