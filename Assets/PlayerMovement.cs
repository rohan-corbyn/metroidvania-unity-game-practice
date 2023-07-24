using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private float dirX;
    private float dirY;
    private enum MovementState { idle, running, jumping, falling, crouching, dashing, idleDrawn, runningDrawn }
    [SerializeField] private LayerMask jumpableGround;



    private SpriteRenderer sprite;
    [Header("Current movement")]
    MovementState state;
    [SerializeField] private float forceX = 7f;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float accelRate = 7f;
    [SerializeField] private float jumpForce = 15f;

    [Header("Run Settings")]

    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float runAccel = 7f;
    [SerializeField] private float runJumpForce = 7f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 24f;
    [SerializeField] private float dashAccel = 3f;
    [SerializeField] private float dashJumpForce = 3f;

    [SerializeField] public float dashLength = 3f;
    [SerializeField] public float dashCooldown = 3f;
    public float dashCounter = 1f;
    private float dashCoolCounter;
    private PlayerShooting playerShooting;
    private TrailRenderer trailRenderer;
    private Boolean crouching;
    public Boolean isGrounded = false;
    public Boolean facingRight = true;
    [SerializeField] private Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerShooting = GetComponent<PlayerShooting>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        UpdateAnimationState();
    }

    private void MovePlayer()
    {
        isGrounded = IsGrounded();

        DashPlayer(isGrounded);

        //movement
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = rb.velocity.y;

        //crouch
        //crouching = Input.GetKey(KeyCode.S) && isGrounded && dashCounter <= 0;
        crouching = false;


        if (!playerShooting.isAiming && !crouching)
        {
            //run
            if (!crouching)
            {

                if (!isDashing)
                {
                    float speedDiff = (maxSpeed * dirX) - rb.velocity.x;
                    forceX = speedDiff * accelRate;
                    rb.AddForce(forceX * Vector2.right);
                }
                else
                {
                    rb.velocity = new Vector2(dashSpeed* dirX, rb.velocity.y);
                }
                }
            else
            {
                rb.velocity = new Vector2(0, dirY);
            }

            //jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }



    bool dashJumped = false;
    bool isDashing = false;
    private void DashPlayer(Boolean isGrounded)
    {
        // start dash
        if (Input.GetKeyDown(KeyCode.S) && (state == MovementState.running || state == MovementState.runningDrawn))
        {
           
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                dashJumped = false;
                maxSpeed = dashSpeed;
                accelRate = dashAccel;
                dashCounter = dashLength;
                jumpForce = dashJumpForce;
                trailRenderer.emitting = true;
                isDashing = true;
            }
        }

        //mid dash
        if (dashCounter > 0)
        {


            if (isGrounded && dashJumped)
            {
                dashCounter = -1.0f;
            }
            else if (isGrounded) {
                dashCounter -= Time.deltaTime;
            }
            else
            {
                dashJumped = true;
            }

            //  ?? stop dash after wall collision
            // stop dash
            if (dashCounter <= 0 || (dirX < 0.1f && dirX > -0.1f))
            {
                maxSpeed = runSpeed;
                accelRate = runAccel;
                dashCoolCounter = dashCooldown;
                jumpForce = runJumpForce;
                trailRenderer.emitting = false;
                isDashing = false;
            }

        }

        // dash cooldown
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    private void UpdateAnimationState()
    {

        if (playerShooting.isAiming)
        {
            state = MovementState.idleDrawn;
        }
        //Jumping
        else if (dirY > 0.1f)
        {
            state = MovementState.jumping;
        }
        //Falling
        else if (dirY < -0.1f)
        {
            state = MovementState.falling;
        }

        //Crouching
        else if (crouching == true && dashCounter <= 0)
        {
            state = MovementState.crouching;
        }
        else if (playerShooting.weaponDrawn)
        {
            if (playerShooting.isAiming)
            {
                state = MovementState.idleDrawn;
            }
            else if (dirX > 0.1 || dirX < -0.1)
            {
                if (dashCounter > 0)
                {
                    state = MovementState.dashing;
                }
                else
                {
                    state = MovementState.runningDrawn;
                }
            }
            else
            {
                state = MovementState.idleDrawn;
            }
        }
        //Running/Dashing
        else if (dirX > 0.1 || dirX < -0.1)
        {
            if (dashCounter > 0)
            {
                state = MovementState.dashing;
            }
            else
            {
                state = MovementState.running;
            }
        }
        //Idle
        else
        {
            state = MovementState.idle;
        }

        //Jumping
        if (dirY > 0.1f)
        {
            state = MovementState.jumping;
        }
        //Falling
        else if (dirY < -0.1f)
        {
            state = MovementState.falling;
        }


        //Flip Sprite X-Axis
        if (dirX > 0)
        {
            sprite.flipX = false;
            facingRight = true;
            
        }
        else if (dirX < 0)
        {
            sprite.flipX = true;
            facingRight = false;
            
        }

        //Debug.Log((int)state);
        anim.SetInteger("state", (int) state);
    }

    private Boolean IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .5f, jumpableGround);
    }
}
