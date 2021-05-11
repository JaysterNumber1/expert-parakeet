using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    // private InputHandler input;
    //
    //private CharacterController controller;


    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    public float horizontalMomentum = 1f;

    public float sprintSpeedMultiplier = 1f;
    // private bool facingRight = true;


    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = .25f;
    private float jumpTimer;
    private float dragY;

    [Header("Components")]
    public Rigidbody2D rb;
    // public Animator animator;
    public LayerMask groundLayer;


    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;


    [Header("Collisions")]
    public bool isGrounded = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;





    private void Update()
    {
        GroundCheck();

        if (Keyboard.current.spaceKey.isPressed && isGrounded)
        {
            jumpTimer = Time.time + jumpDelay;

        }
    }

    private void GroundCheck()
    {

        isGrounded = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer)) || (Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer));

    }
       
    private void keysPressed()
    {
        direction.x = 0;
       
        if (Keyboard.current.dKey.isPressed)
        {
            direction.x = 1;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            direction.x = -1;
        }
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            sprintSpeedMultiplier = 1.5f;
        }
        else
        {
            sprintSpeedMultiplier = 1f;
        }
       

    }


    public void LateUpdate()
    {

    }

    public void movement(InputAction.CallbackContext context)
    {
        direction.x = context.ReadValue<Vector2>().x;
        direction.y = context.ReadValue<Vector2>().y;
    }

    private void FixedUpdate()
    {
        keysPressed();
        moveCharacter(direction.x);
        modifyPhysics();
        if (jumpTimer > Time.time && isGrounded)
        {
            Jump();
        }
    }
    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed * sprintSpeedMultiplier);

        if (Mathf.Abs(rb.velocity.x) > maxSpeed * sprintSpeedMultiplier)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }


        //animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
    }

    void modifyPhysics()
    {
        Debug.Log(rb.velocity.x);
        //Debug.Log(rb.velocity.y);
        bool changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if (isGrounded)
        {
            gravity = 0f;
            rb.gravityScale = gravity;


            if (Mathf.Abs(direction.x) == 0 || changingDirection)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0;

            }
            

        }
        else
        {
            gravity = 1f;
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            //dragY = rb.drag;
            //rb.drag = 0;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;// * dragY;
                
            }
            else if (rb.velocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);// * dragY;
               
            }
            // rb.velocity = new Vector2(horizontalMomentum, rb.velocity.y);
            //rb.drag = 0;
           
                if (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
                {
                    rb.AddForce(Vector2.right * direction.x * Mathf.Abs(horizontalMomentum));
                }
            horizontalMomentum = rb.velocity.x;

        }
    }

    public void EarlyFixedUpdate()
    {
        
      

    }
    public void LateFixedUpdate()
    {

        
    }
    /* if((horizontal > 0 && !facingRight) || (horizontal< 0 && facingRight)){
     Flip();
}
 void Flip()
 {
     facingRight = !facingRight;
     transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
 }

 */

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

}
