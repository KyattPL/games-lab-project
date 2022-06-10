using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayMenuBarn : MonoBehaviour
{
    private Text moneyState;
    
    void Start()
    {
        moneyState = gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
    }

    public void SetMoneyState(float money)
    {
        moneyState.text = "Money: " + money.ToString("0.00") + "$";
    }
}
