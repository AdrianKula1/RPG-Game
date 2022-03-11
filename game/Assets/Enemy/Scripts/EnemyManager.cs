using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public State currentState;
    public Player target;
    //public Rigidbody2D rigidBody;
    public float detectionRadious = 3f;
    public float attackRadious = 0.5f;
    public float speed = 4f;

    private void Awake()
    {
        //rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState(this);

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }

}
