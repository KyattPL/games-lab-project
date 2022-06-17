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
    public PlayMenuBarn plMenuDisp;
    private StarterAssetsInputs _input;
    private float talkingDistance = 5.0f;
    private bool isTalking = false;
    private bool isInputVisible = false;
    private bool isShroomEncountered = false;
    private bool isTigerTalking = false;
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
                    string currentDialogue = PlayerPrefs.GetString("Dialogue", "quest-1");
                    interactionCanvas.gameObject.SetActive(false);
                    string[] lines = _dialogues[currentDialogue]["texts"];
                    isTigerTalking = true;
                    StartDialogue(lines);
                }
            }
            else if (hit.transform.gameObject.tag == "Car")
            {
                interactionCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Wciśnij E by pojechać na misję";
                interactionCanvas.gameObject.SetActive(true);
                if (_input.interact)
                {
                    SceneManager.LoadScene(PlayerPrefs.GetString("Next level", "Level1"));
                }
            }
            else if (hit.transform.gameObject.tag == "Crate" && PlayerPrefs.GetFloat("Money") >= 100.0f && PlayerPrefs.GetInt("MiceCapability") != 7)
            {
                interactionCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Wciśnij E by kupić worek";
                interactionCanvas.gameObject.SetActive(true);
                if (_input.interact)
                {
                    float currMoney = PlayerPrefs.GetFloat("Money");
                    PlayerPrefs.SetFloat("Money", currMoney - 100.0f);
                    PlayerPrefs.SetInt("MiceCapability", 7);
                    plMenuDisp.SetMoneyState(PlayerPrefs.GetFloat("Money"));
                    _input.interact = false;
                }
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

        if (isTigerTalking && PlayerPrefs.HasKey("Level completed") && PlayerPrefs.GetString("Level completed") == "True")
        {
            PlayerPrefs.SetInt("Quest", PlayerPrefs.GetInt("Quest", 1) + 1);
            int nextQuest = PlayerPrefs.GetInt("Quest");

            PlayerPrefs.SetString("Level completed", "False");
            PlayerPrefs.SetString("Next level", $"Level{nextQuest}");
                        
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
            PlayerPrefs.SetFloat("Money", PlayerPrefs.GetFloat("Money", 0.0f) + money);
            plMenuDisp.SetMoneyState(PlayerPrefs.GetFloat("Money"));
            PlayerPrefs.SetString("Dialogue", $"quest-{nextQuest}");
        }

        isTigerTalking = false;
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
                GameObject.FindGameObjectWithTag("EasterShroom").tag = "Untagged";
                StartDialogue(new string[] {"Hmm...", "No dobra, dla prawdziwego kocura 133.7 myszodolarów się należy!"});
                PlayerPrefs.SetFloat("Money", PlayerPrefs.GetFloat("Money", 0.0f) + 133.7f);
                plMenuDisp.SetMoneyState(PlayerPrefs.GetFloat("Money"));
            } else {
                isShroomEncountered = false;
                easterBox.SetActive(false);
                Cursor.visible = false;
                StartDialogue(new string[] {"Hmm...", "Nic z tego kolego!"});
            }
        }
    }
}
