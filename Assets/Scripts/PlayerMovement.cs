using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);

    float currentGravity;

    bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        currentGravity = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (isAlive)
        {
            Run();
            FlipSprite();
            ClimbLadder();
            Die();
        }
    }

    void OnMove(InputValue value)
    {
        if (isAlive)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    {
        if (isAlive)
        {
            if (value.isPressed && (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))))
            {
                myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            }
        }
    }

    void OnFire(InputValue value)
    {
        if (isAlive)
        {
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            Vector2 climbingVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            myRigidbody.velocity = climbingVelocity;
            myRigidbody.gravityScale = 0;

            bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
            if (playerHasVerticalSpeed)
            {
                myAnimator.SetBool("isClimbing", true);
            }

        }
        else
        {
            myRigidbody.gravityScale = currentGravity;
            myAnimator.SetBool("isClimbing", false);
        }

    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSesion>().ProccessPlayerDeath();
        }
    }
}
