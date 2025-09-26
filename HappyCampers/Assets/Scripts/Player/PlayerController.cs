using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // Movement
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private TMPro.TextMeshProUGUI moveSpeedText;
    [SerializeField] private UnityEngine.UI.Slider moveSpeedSlider;

    private float capturedBulletTime = 0.0f;
    private float elapsedTime = 0.0f;   // Accumulated time since script start

    //Player Components
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0.0f; // No gravity
    }


    void Update()
    {

        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        // Set animator bool
        bool isWalking = movement != Vector2.zero;
        // animator.SetBool("isWalking", isWalking);

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Debug.Log("Mouse World Position: " + mouseWorldPos);

        // Flip sprite if moving left/right
        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
        // else if(rb.position.x < mouseWorldPos.x)
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.position.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        // else
        // {
        //     spriteRenderer.flipX = true;
        // }


        if (Input.GetKeyDown(KeyCode.H))
        {
            // score += 2000; // Increment score by 10 when H is presse`d
        }

    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        Debug.Log("Elapsed Time: " + elapsedTime);
        // rb.AddForce(movement * moveSpeed * Time.deltaTime);
        rb.linearVelocity = movement.normalized * moveSpeed;

        // Set animator parameters based on velocity direction
        Vector2 velocity = rb.linearVelocity;

        bool isWalkingHorizontal = velocity.x > 0.01f || velocity.x < -0.01f;
        animator.SetBool("isWalkingRight", isWalkingHorizontal);
        animator.SetBool("isWalkingUp", velocity.y > 0.01f);
        animator.SetBool("isWalkingDown", velocity.y < -0.01f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded");
            // isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded FAlse");
            // isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    }

    
}
