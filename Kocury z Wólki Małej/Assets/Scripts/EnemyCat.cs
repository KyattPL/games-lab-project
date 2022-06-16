using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCat : MonoBehaviour
{
    public AbstractState currState;
    public Transform playerTr;

    void Awake()
    {
        currState = new PatrolState(this, playerTr);
        currState.Entry();
    }

    void Update()
    {
        currState.Run();
    }

    public void ChangeState(AbstractState newState)
    {
        currState = newState;
    }

    public void TakeOneLife()
    {
        playerTr.gameObject.GetComponent<LivesManager>().TakeOneLife();
    }
}
