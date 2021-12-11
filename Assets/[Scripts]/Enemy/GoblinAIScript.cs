using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAIScript : MonoBehaviour
{
    public AIState state;
    EnemyScript controller;

    float idleTimerMax = 2.0f;
    float idleTimer;

    float deathTimerMax = 2.0f;
    float deathTimer;

    private void OnEnable()
    {
        state = AIState.IDLE;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<EnemyScript>();
        state = AIState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isDead)
            state = AIState.DEATH;

       ResolveAI();

    }

    private void ResolveAI()
    {
        switch(state)
        {
            case AIState.IDLE:
                Idle();
                controller.LookAhead();
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
                //if (!controller.isGroundAhead || controller.isWallAhead)
                //    controller.Flip();
                if (controller.HasLOS())
                    state = AIState.ATTACK;
                break;
            case AIState.ATTACK:
                controller.UpdateAnimator();
                if (controller.InRangeOfPlayer())
                    controller.Attack();
                else
                {
                    controller.LookAhead();
                    controller.Move();
                    //if (!controller.isGroundAhead || controller.isWallAhead)
                    //    controller.Flip();
                }
                if (!controller.HasLOS())
                    state = AIState.IDLE;
                break;
            case AIState.FLEE:
                break;
            case AIState.DEATH:
                Death();
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

    private void Death()
    {
        controller.attackCollider.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        if (deathTimer < deathTimerMax)
            deathTimer += Time.deltaTime;
        else
        {
            transform.parent = EnemyManager.Instance.transform;
            gameObject.SetActive(false);
            deathTimer = 0;
        }
    }
}
