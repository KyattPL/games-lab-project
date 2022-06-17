using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBarn : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
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
        playMenuUI.SetActive(true);
        isGamePaused = false;
        playerObject.GetComponent<FirstPersonController>().enabled = true;
        watergunObject.GetComponent<RaycastGunShot>().enabled = true;
        playerObject.GetComponent<DialogueStarter>().enabled = true;

        StarterAssetsInputs _input = playerObject.GetComponent<StarterAssetsInputs>();
        _input.jump = false;
        _input.interact = false;
        _input.shoot = false;

        Cursor.visible = false;
    }
    
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        playMenuUI.SetActive(false);
        isGamePaused = true;
        playerObject.GetComponent<FirstPersonController>().enabled = false;
        watergunObject.GetComponent<RaycastGunShot>().enabled = false;
        playerObject.GetComponent<DialogueStarter>().enabled = false;

        StarterAssetsInputs _input = playerObject.GetComponent<StarterAssetsInputs>();
        _input.jump = false;
        _input.interact = false;
        _input.shoot = false;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Quit()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}

