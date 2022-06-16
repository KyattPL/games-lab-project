using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


using System.Collections.Generic;

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
    private bool isShroomEncountered = false;
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
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, talkingDistance) && !isTalking && !isShroomEncountered)
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
                    isShroomEncountered = true;
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
                StopTalking();
                ResumeGame();
            }
        }
        else
        {
            _input.interact = false;
            interactionCanvas.gameObject.SetActive(false);
        }

        if (!isTalking && isShroomEncountered) {
            easterBox.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            playerObj.GetComponent<FirstPersonController>().enabled = false;
            watergunObj.GetComponent<RaycastGunShot>().enabled = false;
            menuObj.GetComponent<PauseMenuBarn>().enabled = false;
            easterBox.GetComponentInChildren<Button>().onClick.AddListener(CheckSecret);
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

    void StopTalking()
    {
        isTalking = false;
    }

    void ResumeGame()
    {
        playerObj.GetComponent<FirstPersonController>().enabled = true;
        watergunObj.GetComponent<RaycastGunShot>().enabled = true;
        menuObj.GetComponent<PauseMenuBarn>().enabled = true;
        _input.interact = false;
    }

    void CheckSecret()
    {
        GameObject inputForm = GameObject.Find("TextInput");

        if (inputForm != null) {
            string inputVal = inputForm.GetComponent<TextMeshProUGUI>().text;
            _input.shoot = false;
            inputVal = inputVal.Substring(0, inputVal.Length - 1);

            if (inputVal.Trim() == "eksperta") {
                isShroomEncountered = false;
                easterBox.SetActive(false);
                Cursor.visible = false;
                StartDialogue(new string[] {"Dobrze byku!"});
            } else {
                isShroomEncountered = false;
                easterBox.SetActive(false);
                Cursor.visible = false;
                StartDialogue(new string[] {"Nic z tego kolego!"});
            }
        }
    }
}
