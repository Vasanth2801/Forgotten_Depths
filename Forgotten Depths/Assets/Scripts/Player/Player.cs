using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private int facingDirection = 1;

    [Header("Unity Core Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Inputs")]
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private bool isKnockedBack = false;


    private void Update()
    {
        if (isKnockedBack == false)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            

            if (horizontal > 0 && transform.localScale.x < 0
              || horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }

            anim.SetFloat("Horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("Vertical", Mathf.Abs(vertical));
        }
    }

    private void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            rb.velocity = new Vector2(horizontal, vertical) * speed;
        }
    }


    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void Knockback(Transform enemy,float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.velocity = direction * force;
        StartCoroutine(PushBack(stunTime));
    }

    IEnumerator PushBack(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }
}