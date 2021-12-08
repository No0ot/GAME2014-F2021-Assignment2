using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAIScript : MonoBehaviour
{
    public AIState state;
    EnemyScript controller;

    float idleTimerMax = 2.0f;
    float idleTimer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<EnemyScript>();
        state = AIState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        ResolveAI();
    }

    private void ResolveAI()
    {
        switch(state)
        {
            case AIState.IDLE:
                Idle();
                if (controller.HasLOS())
                {
                    state = AIState.ATTACK;
                    idleTimer = 0;
                }
                break;
            case AIState.PATROL:
                controller.Move();
                controller.LookAhead();
                controller.UpdateAnimator();
                if (!controller.isGroundAhead || controller.isWallAhead)
                    controller.Flip();
                if (controller.HasLOS())
                    state = AIState.ATTACK;
                break;
            case AIState.ATTACK:
                controller.UpdateAnimator();
                if (controller.InRangeOfPlayer())
                    controller.Attack();
                else
                    controller.Move();
                if (!controller.HasLOS())
                    state = AIState.IDLE;
                break;
            case AIState.FLEE:
                break;
        }
    }

    private void Idle()
    {
        controller.UpdateAnimator();
        if (idleTimer < idleTimerMax)
            idleTimer += Time.deltaTime;
        else
        {
            state = AIState.PATROL;
            idleTimer = 0;
        }
    }
}
