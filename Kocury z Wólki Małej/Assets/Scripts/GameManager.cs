using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayMenuBarn playMenuDisp;
    void Start()
    {
        if (!PlayerPrefs.HasKey("Next level") && !PlayerPrefs.HasKey("Level completed"))
        {
            PlayerPrefs.SetInt("QuestNo", 1);
            PlayerPrefs.SetString("Next level", "Level1");
            PlayerPrefs.SetString("Level completed", "False");
            PlayerPrefs.SetInt("MiceCapability", 4);
            PlayerPrefs.SetInt("MiceToCollect", 7);
            PlayerPrefs.SetFloat("Money", 0.0f);
            PlayerPrefs.SetString("Dialogue", "quest-1");
        }
        if (PlayerPrefs.HasKey("Next level") && PlayerPrefs.HasKey("Level completed") && PlayerPrefs.GetString("Level completed") == "True")
        {
            PlayerPrefs.SetString("Dialogue", PlayerPrefs.GetString("Dialogue") + "-done");
        }
        if (PlayerPrefs.HasKey("Money"))
        {
            playMenuDisp.SetMoneyState(PlayerPrefs.GetFloat("Money"));
        }
    }
}
