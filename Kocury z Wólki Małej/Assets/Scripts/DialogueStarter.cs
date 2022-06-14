using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject watergunObj;
    public Camera playerCamera;
    public Canvas interactionCanvas;
    public Dialogue dialogueObj;
    private StarterAssetsInputs _input;
    private float talkingDistance = 5.0f;
    private bool isTalking = false;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, talkingDistance) && !isTalking)
        {
            if (hit.transform.gameObject.tag == "NPC")
            {
                interactionCanvas.gameObject.SetActive(true);
                if (_input.interact)
                {
                    interactionCanvas.gameObject.SetActive(false);
                    StartDialogue();
                    _input.interact = false;
                }
            }
        }
        else if (isTalking)
        {
            if (!dialogueObj.gameObject.activeSelf)
            {
                ResumeGame();
            }
        }
        else
        {
            _input.interact = false;
            interactionCanvas.gameObject.SetActive(false);
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        playerObj.GetComponent<FirstPersonController>().enabled = false;
        watergunObj.GetComponent<RaycastGunShot>().enabled = false;
        dialogueObj.lines = new string[] {"Siema Rafa mam dla ciebie zadanko...", "Upoluj mi 7 myszy bo zbliża się dzień wypłaty i muszę zapłacić Łysemu, dzięki!"};
        dialogueObj.gameObject.SetActive(true);
    }

    void ResumeGame()
    {
        isTalking = false;
        playerObj.GetComponent<FirstPersonController>().enabled = true;
        watergunObj.GetComponent<RaycastGunShot>().enabled = true;
    }
}
