using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastGunShot : MonoBehaviour
{
    public Camera playerCamera;
    public Transform streamOrigin;
    public ParticleSystem waterSplash;
    public float gunRange = 50f;
    public float streamDuration = 0.05f;
    public float fireRate = 0.2f;
    private StarterAssetsInputs inputSys;
    LineRenderer streamLine;
    float fireTimer;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        inputSys = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<StarterAssetsInputs>();
    }
    void Awake()
    {
        streamLine = GetComponent<LineRenderer>(); 
    }

    void Update()
    {
       fireTimer += Time.deltaTime;
       if(inputSys.shoot && fireTimer > fireRate)
       {
            fireTimer = 0;
            streamLine.SetPosition(0, streamOrigin.position);
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            waterSplash.Play();
            RaycastHit hit;
            if(Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, gunRange))
            {
                streamLine.SetPosition(1, hit.point);
                
                if(hit.transform.gameObject.tag == "Mouse")
                {
                    //Debug.Log("Mouse hit");
                    hit.transform.gameObject.GetComponent<MouseMovement>().Die();
                }
            } 
            else
            {
                streamLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * gunRange));
            }
            audioSource.PlayOneShot(audioSource.clip);
            StartCoroutine(ShootLaser());
            inputSys.shoot = false;
       } 
    }

    IEnumerator ShootLaser()
    {
        streamLine.enabled = true;
        yield return new WaitForSeconds(streamDuration);
        streamLine.enabled = false;
    }
}
