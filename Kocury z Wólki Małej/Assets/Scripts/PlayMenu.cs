using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayMenu : MonoBehaviour
{
    private Text miceLeft;
    private Text miceCarried;
    private Text livesLeft;
    void Start()
    {
        miceLeft = gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        miceCarried = gameObject.transform.GetChild(2).gameObject.GetComponent<Text>();
        livesLeft = gameObject.transform.GetChild(3).gameObject.GetComponent<Text>();

    }

    public void ChangeLivesNumber(int newLivesNum)
    {
        livesLeft.text = "¯ycia: " + newLivesNum;
    }

    public void ChangeMiceCarriedNumber(int newMiceNum, int maxMiceCarried)
    {
        miceCarried.text = "Myszy w worku: " + newMiceNum + "/" + maxMiceCarried;
    }

    public void ChangeMiceLeftNumber(int newMiceNum, int miceToCollect)
    {
        miceLeft.text = "Myszy do zebrania: " + newMiceNum + "/" + miceToCollect;
    }
}
