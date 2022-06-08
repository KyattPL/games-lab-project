using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseMovement : MonoBehaviour
{
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;
    public Transform player;
    public GameObject playerGO;
    
    private NavMeshAgent agent;
    private MouseGathering msGatheringScript;
    private float interpolationPeriod = 12.0f;
    private float minDistance = 3.8f;
    private float shiftParam = 0.65f;
    private float time;
    void Start()
    {
        msGatheringScript = playerGO.GetComponent<MouseGathering>();
        time = interpolationPeriod;
        agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = -1.1f;
    }

    void Update()
    {
        //Debug.Log(agent.speed);
        time += Time.deltaTime;
        if (agent.enabled)
        {
            if (time >= interpolationPeriod)
            {
                //Debug.Log("We are changing position");
                time = 0.0f;
                Vector3 pos = new Vector3(Random.Range(xMin, xMax), transform.position.y, Random.Range(zMin, zMax));
                agent.destination = pos;
            }
            if (Vector3.Distance(transform.position, player.position) < minDistance)
            {
                Vector3 shift = (transform.position - player.position) * shiftParam;
                agent.speed = 2.0f;
                agent.destination = transform.position + shift;
            }
            else
            {
                agent.speed = 0.22f;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (!agent.enabled && collision.gameObject.tag == "Player")
        {
            //Debug.Log("Mouse gathered");
            if (msGatheringScript.micesCarried < msGatheringScript.maxCapability)
            {
                msGatheringScript.addNewMouse();
                Destroy(gameObject);
            }
        }
    }
    
    public void Die()
    {
        agent.enabled = false;
    }
}
