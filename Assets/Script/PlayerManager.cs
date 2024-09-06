using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    public TMP_Text resourceText;
    [SerializeField]
    public TMP_Text ChangedResourceText;
    public int resource = 500;
    
    [SerializeField]
    public TMP_Text moneyText;
    [SerializeField]
    public TMP_Text ChangedMoneyText;
    public int money = 1000;


    void Start()
    {
        resourceText.text = resource.ToString();
        moneyText.text = money.ToString();
        ChangedResourceText.text = "";
        ChangedMoneyText.text = "";
    }
    
    public void AddMoneyResource(int resource, int money)
    {
        this.resource += resource;
        ChangedResourceText.text = "+" + resource;
        this.money -= money;
        ChangedMoneyText.text = "+" + money;
        StartCoroutine(ClearChangedTextAfterDelay());
    }
    
    public void MinusMoneyResource(int resource, int money)
    {
        this.resource -= resource;
        ChangedResourceText.text = "-" + resource;
        this.money -= money;
        ChangedMoneyText.text = "-" + money;
        StartCoroutine(ClearChangedTextAfterDelay());
    }
    
    private void Update()
    {
        resourceText.text = "Resource:"  + resource;
        moneyText.text = "Money:"  + money;
    }

    public void PreviewAddmoneyValue(int resource, int money)
    {
        ChangedResourceText.text = "+" + resource;
        ChangedMoneyText.text = "+" + money;
    }
    
    public void PreviewMinusMoneyValue(int resource, int money)
    {
        ChangedResourceText.text = "-" + resource;
        ChangedMoneyText.text = "-" + money;
        StartCoroutine(FlashTextEffect());
    }
    
    private IEnumerator ClearChangedTextAfterDelay()
    {
        yield return new WaitForSeconds(1f); 
        ChangedResourceText.text = "";
        ChangedMoneyText.text = "";
    }
    
    private IEnumerator FlashTextEffect()
    {
        Color originalColor = ChangedResourceText.color;
        
        for (int i = 0; i < 3; i++) 
        {
            
            ChangedResourceText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            ChangedMoneyText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            yield return new WaitForSeconds(0.1f);

            
            ChangedResourceText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            ChangedMoneyText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            yield return new WaitForSeconds(0.1f);
        }
        ChangedResourceText.color = originalColor;
        ChangedMoneyText.color = originalColor;
    }
}
