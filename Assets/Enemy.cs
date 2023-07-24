using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]private int health = 100;
    [SerializeField] private int moveSpeed = 1;

    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = -1;

        rb.velocity = new Vector2(moveSpeed * dirX, rb.velocity.y);


    }

    internal void TakeDamage(int damage)
    {
        health -= damage;


        if (health < damage) {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


        //state = (int) MovementState.colliding;

        PlayerShooting player = collision.gameObject.GetComponent<PlayerShooting>();

        if (player != null)
        {
            player.TakeDamage(34);

        }
    }
}
