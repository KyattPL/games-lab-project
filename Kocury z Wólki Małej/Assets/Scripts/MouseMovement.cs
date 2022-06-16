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
    private float runDirectionChangeTime = 2.0f;
    private float turnSidesChangeTime = 0.9f;
    private float minDistance = 3.8f;
    private float shiftParam = 6.0f;
    private float time;
    private float runTime;
    private float turnSidesTime;
    public float distToWall = 2.5f;
    void Start()
    {
        msGatheringScript = playerGO.GetComponent<MouseGathering>();
        time = interpolationPeriod;
        runTime = runDirectionChangeTime;
        turnSidesTime = turnSidesChangeTime;
        agent = GetComponent<NavMeshAgent>();
        //agent.baseOffset = -1.1f;
    }

    void Update()
    {
        //Debug.Log(agent.speed);
        time += Time.deltaTime;
        runTime += Time.deltaTime;
        turnSidesTime += Time.deltaTime;
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
                Vector3 shift = (transform.position - player.transform.position) * shiftParam;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, distToWall))
                {
                    float rand = Random.value;
                    if (hit.transform.gameObject.tag == "Wall" || hit.transform.gameObject.name.ToLower().StartsWith("fence"))
                    {
                        if (rand >= 0.5f)
                        {
                            shift = new Vector3(Random.Range(xMin / 2, xMax / 2), transform.position.y, Random.Range(zMin / 2, zMax / 2)) * shiftParam;
                        }
                        else
                        {
                            shift = new Vector3(Random.Range(xMin / 2, xMax / 2), transform.position.y, Random.Range(zMin / 2, zMax / 2)) * shiftParam; ;
                        }
                        if (turnSidesTime >= turnSidesChangeTime)
                        {
                            
                            agent.speed = 4.0f;
                            agent.destination = shift;
                            turnSidesTime = 0.0f;
                        }
                    }
                    else if (rand >= 0.95f)
                    {
                        shift = Quaternion.Euler(-90.0f, 0.0f, 0.0f) * shift;
                    }
                    else if (rand >= 0.9f)
                    {
                        shift = Quaternion.Euler(90.0f, 0.0f, 0.0f) * shift;
                    }
                }
                else
                {
                    float rand = Random.value;
                    if (rand >= 0.95f)
                    {
                        shift = Quaternion.Euler(-90.0f, 0.0f, 0.0f) * shift;
                    }
                    else if (rand >= 0.9f)
                    {
                        shift = Quaternion.Euler(90.0f, 0.0f, 0.0f) * shift;
                    }
                }
                //Debug.DrawRay(transform.position, transform.forward, Color.green);
                if(runTime >= runDirectionChangeTime)
                {
                    agent.speed = 2.0f;
                    agent.destination = transform.position + shift;
                    runTime = 0.0f;
                }
               
            }
            else
            {
                agent.speed = 0.22f;
            }
            //Debug.DrawRay(transform.position + new Vector3(0f, 1.5f), transform.forward, Color.green);
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
