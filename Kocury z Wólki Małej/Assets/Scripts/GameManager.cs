using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayMenuBarn playMenuDisp;
    void Start()
    {
        PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("Next level") && !PlayerPrefs.HasKey("Level completed"))
        {
            PlayerPrefs.SetString("Next level", "Level1");
            PlayerPrefs.SetString("Level completed", "False");
            PlayerPrefs.SetInt("MiceCapability", 4);
            PlayerPrefs.SetInt("MiceToCollect", 7);
            PlayerPrefs.SetFloat("Money", 0.0f);
        }
        if (PlayerPrefs.HasKey("Next level") && PlayerPrefs.HasKey("Level completed") && PlayerPrefs.GetString("Level completed") == "True")
        {
            float fTime = PlayerPrefs.GetFloat("FinishedTime");
            float money;
            if (fTime < 100.0f)
            {
                money = 400.0f;
            }
            else if (fTime < 300.0f)
            {
                money = 200.0f;
            }
            else if (fTime < 500.0f)
            {
                money = 130.0f;
            }
            else
            {
                money = 50.0f;
            }
            PlayerPrefs.SetFloat("Money", money);
        }
        //if (PlayerPrefs.HasKey("Next level") && PlayerPrefs.GetString("Next level") == "Level1")
        //{
        //    //Proper dialog
        //}
        //if (PlayerPrefs.HasKey("Next level") && PlayerPrefs.GetString("Next level") == "Level2")
        //{
        //    //Proper dialog
        //}
        if (PlayerPrefs.HasKey("Money"))
        {
            playMenuDisp.SetMoneyState(PlayerPrefs.GetFloat("Money"));
        }
    }
}
