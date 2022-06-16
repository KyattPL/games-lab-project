using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesManager : MonoBehaviour
{
    public GameObject playMenuDisplay;
    private PlayMenu playMenu;
    private int lives = 3;
    void Start()
    {
        playMenu = playMenuDisplay.GetComponent<PlayMenu>();
    }

    public void TakeOneLife()
    {
        lives--;
        playMenu.ChangeLivesNumber(lives);
        if (lives == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
