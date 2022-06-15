using StarterAssets;

using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public bool isTalking;
    public StarterAssetsInputs _input;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
        isTalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.shoot)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
            _input.shoot = false;
        }
    }

    void OnEnable()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            textComponent.text = string.Empty;
        }
    }
}
