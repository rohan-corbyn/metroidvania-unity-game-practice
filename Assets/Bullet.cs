using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    private Rigidbody2D rb;
    private enum MovementState {travelling, colliding}
    private Animator anim;
    public GameObject impactEffect;
    // Start is called before the first frame update
    private int state = (int) MovementState.travelling;
    


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        state = (int)MovementState.colliding;

        Enemy enemy = hitInfo.GetComponent<Enemy>();

        if (enemy != null) {
            enemy.TakeDamage(34);
            
        }

        Destroy(gameObject);

        GameObject impact = Instantiate(impactEffect, transform.position, transform.rotation);

        Destroy(impact, 0.4f);


    
    }

}
