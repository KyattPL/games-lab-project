using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum StateName { RunToPlayer, RunAway, Patrol, Attack}
public abstract class AbstractState
{
    public StateName Current { get; set; }
    public bool WasShot { get; set; }
    protected EnemyCat enemy;
    protected Transform playerTransform;
    public AbstractState(EnemyCat enCat, Transform pTransform)
    {
        playerTransform = pTransform;
        enemy = enCat;
    }
    public virtual void Entry() { }
    public virtual void Run() { }
    public virtual void Exit() { }
    public void MoveTo(AbstractState newState)
    {
        Exit();
        newState.Entry();
        enemy.ChangeState(newState);
    }
}

public class PatrolState : AbstractState
{
    
    private NavMeshAgent navAgent;
    private float hearDistance = 6.0f;
    private float changeGoalPeriod = 10.0f;
    private float currTime;
    private float xMin = -21.53f;
    private float xMax = 1.55f;
    private float zMin = -26.55f;
    private float zMax = 0.79f;

    public PatrolState(EnemyCat enemy, Transform pTransform) : base(enemy, pTransform)
    {
        Current = StateName.Patrol;
    }

    public override void Entry()
    {
        currTime = changeGoalPeriod;
        Animator animator = enemy.GetComponent<Animator>();
        navAgent = enemy.GetComponent<NavMeshAgent>();
        navAgent.speed = 0.7f;
        animator.SetInteger("state", 0);
        WasShot = false;
    }

    public override void Run()
    {
        currTime += Time.deltaTime;
        if (Vector3.Distance(enemy.transform.position, playerTransform.position) < hearDistance)
        {
            MoveTo(new RunToPlayerState(enemy, playerTransform));
        }
        else if (currTime >= changeGoalPeriod)
        {
            currTime = 0.0f;
            Vector3 pos = new Vector3(Random.Range(xMin, xMax), enemy.transform.position.y, Random.Range(zMin, zMax));
            navAgent.destination = pos;
        }
        else if (WasShot)
        {
            MoveTo(new RunAwayState(enemy, playerTransform));
        }
    }

}

public class RunToPlayerState : AbstractState
{
    
    private NavMeshAgent navAgent;
    private float hearDistance = 6.0f;
    private float attackDistance = 0.8f;
    public RunToPlayerState(EnemyCat enemy, Transform pTransform) : base(enemy, pTransform)
    {

    }
    public override void Entry()
    {
        Animator animator = enemy.GetComponent<Animator>();
        navAgent = enemy.GetComponent<NavMeshAgent>();
        navAgent.speed = 3.2f;
        animator.SetInteger("state", 1);
        WasShot = false;
        
    }
    public override void Run()
    {
        navAgent.destination = playerTransform.position;
        if (WasShot)
        {
            MoveTo(new RunAwayState(enemy, playerTransform));
        }
        else if (Vector3.Distance(enemy.transform.position, playerTransform.position) <= attackDistance)
        {
            MoveTo(new AttackState(enemy, playerTransform));
        }
        else if (Vector3.Distance(enemy.transform.position, playerTransform.position) > hearDistance)
        {
            MoveTo(new PatrolState(enemy, playerTransform));
        }
    }
}

public class RunAwayState : AbstractState
{
    private float currTime;
    private float timeToPass = 3.0f;
    private float hearDistance = 6.0f;
    private float attackDistance = 0.8f;

    public RunAwayState(EnemyCat enemy, Transform pTransform) : base(enemy, pTransform)
    {

    }
    public override void Entry()
    {
        WasShot = false;
        Animator animator = enemy.GetComponent<Animator>();
        animator.SetInteger("state", 1);
        Vector3 runShift = new Vector3(0.0f, 0.0f, -7.0f);
        NavMeshAgent navAgent = enemy.GetComponent<NavMeshAgent>();
        navAgent.speed = 2.7f;
        navAgent.destination = enemy.transform.position + runShift;
    }

    public override void Run()
    {
        currTime += Time.deltaTime;
        if (currTime >= timeToPass)
        {
            if (Vector3.Distance(enemy.transform.position, playerTransform.position) <= attackDistance)
            {
                MoveTo(new AttackState(enemy, playerTransform));
            }
            else if (Vector3.Distance(enemy.transform.position, playerTransform.position) <= hearDistance)
            {
                MoveTo(new RunToPlayerState(enemy, playerTransform));
            }
            else
            {
                MoveTo(new PatrolState(enemy, playerTransform));
            }
        }
        
    }
    public override void Exit()
    {
        WasShot = false;
    }
}

public class AttackState : AbstractState
{
    private float attackDistance = 0.8f;
    private NavMeshAgent navAgent;
    public AttackState(EnemyCat enemy, Transform pTransform) : base(enemy, pTransform)
    {

    }
    public override void Entry()
    {
        WasShot = false;
        Animator animator = enemy.GetComponent<Animator>();
        animator.SetInteger("state", 2);
        navAgent = enemy.GetComponent<NavMeshAgent>();
        navAgent.speed = 2.0f;
        navAgent.updatePosition = false;
    }
    public override void Run()
    {
        if (WasShot)
        {
            MoveTo(new RunAwayState(enemy, playerTransform));
        }
        else if (Vector3.Distance(enemy.transform.position, playerTransform.position) > attackDistance)
        {
            MoveTo(new RunToPlayerState(enemy, playerTransform));
        }
    }

    public override void Exit()
    {
        navAgent.updatePosition = true;
    }
}