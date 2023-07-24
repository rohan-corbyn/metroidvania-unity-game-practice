using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public bool weaponDrawn = false;
    public bool isAiming = false;
    private float holsterTimer = 0f;
    [SerializeField] private float holserTime = 0.5f;
    private PlayerMovement playerMovement;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private enum MovementState { idle, running, jumping, falling, crouching, dashing, idleDrawn, runningDrawn }
    private Camera mainCamera;
    private float aimAngleFloat = 3;
    private int aimAngle = 3;
    private Animator anim;

    [SerializeField] private int health = 100;


     private float drawTimer = 0.0f;
    [SerializeField] private float drawTime = 0.3f;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Draw





        if (Input.GetButton("Fire2") && playerMovement.dashCounter <= 0 && playerMovement.isGrounded)
        {
            weaponDrawn = true;
            if (!isAiming) {
                drawTimer = drawTime;
            }

            isAiming = true;
            anim.SetBool("isAiming", true);
            holsterTimer = holserTime;

            
            if (Input.GetButtonDown("Fire1") && drawTimer <= 0)
            {
                Shoot();
            }

            if (drawTimer > 0.0f) {
                drawTimer -= Time.deltaTime;
            }


            float mouseY = Input.GetAxis("Mouse Y");
            if ((aimAngleFloat < 3 && mouseY < 0) || (aimAngleFloat > 1 && mouseY > 0))
            {
                aimAngleFloat -= mouseY / 4;
                aimAngle = (int)Math.Floor(aimAngleFloat);
                anim.SetInteger("aimAngleAnalog", aimAngle);
            }

        }
        else {
            isAiming = false;
            anim.SetBool("isAiming", false);
            aimAngleFloat = 3.0f;
            aimAngle = 3;
            anim.SetInteger("aimAngleAnalog", 3);
        }

        if (!isAiming && holsterTimer > 0) {

            holsterTimer -= Time.deltaTime;

            if (holsterTimer <= 0) {
                weaponDrawn = false;
            }
        }
    }

    internal void TakeDamage(int v)
    {
        health -= v;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Shoot()
    {
        float[] xPos = new float[] { -0.347f, 0.358f, 0.792f, 1.0896f};

        switch (aimAngle) {
            case 0:
                firePoint.localPosition = new Vector3(playerMovement.facingRight ? -0.347f : 0.347f, 1.183f, firePoint.localPosition.z);
                firePoint.localEulerAngles = new Vector3(0, 0, 90);
                break;
            case 1:
                firePoint.localPosition = new Vector3(playerMovement.facingRight ? 0.358f : -0.358f, 0.988f, firePoint.localPosition.z);
                firePoint.localEulerAngles = new Vector3(0, 0, playerMovement.facingRight ? 60.0f : 120.0f);
                break;
            case 2:
                firePoint.localPosition = new Vector3(playerMovement.facingRight ? 0.792f : -0.792f, 0.559f, firePoint.localPosition.z);
                firePoint.localEulerAngles = new Vector3(0, 0, playerMovement.facingRight ? 30.0f : 150.0f);

                break;
            default:
                firePoint.localPosition = new Vector3(playerMovement.facingRight ? 1.0896f : -1.0896f, -0.1545999f, firePoint.localPosition.z);
                firePoint.localEulerAngles = new Vector3(0, 0, playerMovement.facingRight ? 0.0f : 180.0f);

                break;
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    }


    private void Die()
{
    Destroy(gameObject);
}
}