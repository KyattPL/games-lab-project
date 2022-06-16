using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject playerObject;
    public GameObject watergunObject;
    public GameObject playMenuUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        playMenuUI.SetActive(true);
        Time.timeScale = 1.0f;
        isGamePaused = false;
        playerObject.GetComponent<FirstPersonController>().enabled = true;
        watergunObject.GetComponent<RaycastGunShot>().enabled = true;
        Cursor.visible = false;

        //Cursor.lockState = CursorLockMode.;
    }
    public void GoToOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        playMenuUI.SetActive(false);
        Time.timeScale = 0.0f;
        isGamePaused = true;
        playerObject.GetComponent<FirstPersonController>().enabled = false;
        watergunObject.GetComponent<RaycastGunShot>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        
    }
}
