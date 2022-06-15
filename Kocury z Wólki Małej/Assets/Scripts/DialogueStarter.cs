using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Newtonsoft.Json;
public class DialogueStarter : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject watergunObj;
    public GameObject menuObj;
    public Camera playerCamera;
    public Canvas interactionCanvas;
    public Dialogue dialogueObj;
    public TextAsset jsonDialogues;
    public GameObject easterBox;
    private StarterAssetsInputs _input;
    private float talkingDistance = 5.0f;
    private bool isTalking = false;
    private bool isInputVisible = false;
    private Dictionary<string, Dictionary<string, string[]>> _dialogues;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _dialogues = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string[]>>>(jsonDialogues.text);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, talkingDistance) && !isTalking)
        {
            if (hit.transform.gameObject.tag == "NPC")
            {
                interactionCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Wciśnij E by porozmawiać";
                interactionCanvas.gameObject.SetActive(true);
                if (_input.interact)
                {
                    int currentQuest = PlayerPrefs.GetInt("quest", 1);
                    interactionCanvas.gameObject.SetActive(false);
                    string[] lines = _dialogues[$"quest-{currentQuest}"]["texts"];
                    StartDialogue(lines);
                }
            }
            else if (hit.transform.gameObject.tag == "Car")
            {
                interactionCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Wciśnij E by pojechać na misję";
                interactionCanvas.gameObject.SetActive(true);
                if (_input.interact)
                {
                    SceneManager.LoadScene("Level1");
                }
            }
            else if (hit.transform.gameObject.tag == "Crate")
            {
                interactionCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Wciśnij E by kupić worek";
                interactionCanvas.gameObject.SetActive(true);
            }
            else if (hit.transform.gameObject.tag == "EasterShroom")
            {
                interactionCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "???";
                interactionCanvas.gameObject.SetActive(true);
                if (_input.interact)
                {
                    interactionCanvas.gameObject.SetActive(false);
                    string[] lines = _dialogues["eastershroom-1"]["texts"];
                    StartDialogue(lines);
                    isInputVisible = true;
                }
            }
            else
            {
                interactionCanvas.gameObject.SetActive(false);
            }
        }
        else if (isTalking)
        {
            if (!dialogueObj.gameObject.activeSelf && !isInputVisible)
            {
                ResumeGame();
            }
            else if (isInputVisible) {
                easterBox.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            _input.interact = false;
            interactionCanvas.gameObject.SetActive(false);
        }
    }

    void StartDialogue(string[] texts)
    {
        isTalking = true;
        playerObj.GetComponent<FirstPersonController>().enabled = false;
        watergunObj.GetComponent<RaycastGunShot>().enabled = false;
        menuObj.GetComponent<PauseMenuBarn>().enabled = false;
        dialogueObj.lines = texts;
        dialogueObj.gameObject.SetActive(true);
    }

    void ResumeGame()
    {
        isTalking = false;
        playerObj.GetComponent<FirstPersonController>().enabled = true;
        watergunObj.GetComponent<RaycastGunShot>().enabled = true;
        menuObj.GetComponent<PauseMenuBarn>().enabled = true;
        _input.interact = false;
    }
}
