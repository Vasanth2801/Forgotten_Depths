using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [Header("Enemy References")]
    private Rigidbody2D rb;
    private Transform player;
    private EnemyState enemyState;
    [SerializeField] private Animator anim;

    [Header("Enemy Settings")]
    [SerializeField] private float speed;
    [SerializeField] private int facingDirection = 1;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCoolDown = 2f;
    private float attackCoolDownTimer;
    [SerializeField] private float playerdetectRange = 5;
    [SerializeField] private Transform detectionPoint;
    [SerializeField] private LayerMask playerLayer;

    private void Start()
    {
        ChangeState(EnemyState.Idle);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckForPlayer();

        if(attackCoolDown > 0)
        {
            attackCoolDownTimer -= Time.deltaTime;
        }

        if (enemyState == EnemyState.Chasing)
        {
            Chase();
        }
        else if(enemyState == EnemyState.Attacking)
        {
            // Do the Attack thing 
            rb.velocity = Vector2.zero;
        }
    }

    void Chase()
    {
        if(player.position.x > transform.position.x && facingDirection == -1 ||
            player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void ChangeState(EnemyState newState)
    {
        //Exit the Current Animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", false);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            anim.SetBool("isChasing", false);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            anim.SetBool("isAttacking", false);
        }

        //Update our new State
        enemyState = newState;

        // Update the Animtion
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", true);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            anim.SetBool("isChasing", true);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            anim.SetBool("isAttacking", true);
        }
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerdetectRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;

            // if player is in attackRnage and cooldown is ready
            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange && attackCoolDownTimer <= 0)
            {
                attackCoolDownTimer = attackCoolDown;
                ChangeState(EnemyState.Attacking);
            }
            else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerdetectRange);
    }

}


public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
}