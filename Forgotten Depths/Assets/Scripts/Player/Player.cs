using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;

    [Header("Unity Core Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Inputs")]
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("Vertical", Mathf.Abs(vertical));
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal, vertical) * speed;
    }
}