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

    private NavMeshAgent agent;
    private float interpolationPeriod = 12.0f;
    private float minDistance = 3.8f;
    private float time;
    void Start()
    {
        time = interpolationPeriod;
        agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = -1.1f;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= interpolationPeriod)
        {
            //Debug.Log("We are changing position");
            time = 0.0f;
            Vector3 pos = new Vector3(Random.Range(xMin, xMax), transform.position.y, Random.Range(zMin, zMax));
            agent.destination = pos;
        }
        if (Vector3.Distance(transform.position, player.position) < minDistance)
        {
            //Debug.Log("We entered here");
        }
    }
}
