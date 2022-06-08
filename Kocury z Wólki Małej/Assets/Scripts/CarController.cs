using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    public GameObject playerGO;
    private MouseGathering mouseGatherScr;
    private float startTimeStamp;
    private float timeToFinish = 600.0f;
    void Start()
    {
        startTimeStamp = Time.time;
        mouseGatherScr = playerGO.GetComponent<MouseGathering>();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(playerGO.transform.position, gameObject.transform.position) < 2.4f)
        {
            mouseGatherScr.removeMice();
            if (mouseGatherScr.miceCollected == mouseGatherScr.maxCapability)
            {
                if (Time.time - startTimeStamp <= timeToFinish)
                {
                    PlayerPrefs.DeleteKey("Next level");
                    PlayerPrefs.SetFloat("FinishedTime", Time.time - startTimeStamp);
                    PlayerPrefs.SetString("Level completed", "True");
                }
                else
                {
                    PlayerPrefs.SetString("Level completed", "False");
                }
                SceneManager.LoadScene(1);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            
            mouseGatherScr.removeMice();
            if (mouseGatherScr.miceCollected == mouseGatherScr.maxCapability)
            {
                if (Time.time - startTimeStamp <= timeToFinish)
                {
                    PlayerPrefs.DeleteKey("Next level");
                    PlayerPrefs.SetFloat("FinishedTime", Time.time - startTimeStamp);
                    PlayerPrefs.SetString("Level completed", "True");
                }
                else
                {
                    PlayerPrefs.SetString("Level completed", "False");
                }
                SceneManager.LoadScene(1);
            }
        }
    }

    
}
