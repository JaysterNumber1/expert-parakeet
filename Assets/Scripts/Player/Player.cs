using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{


    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
   
    public float sprintSpeedMultiplier = 1f;
    // private bool facingRight = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
  
    [Header("Components")]
    public Rigidbody2D rb;
    // public Animator animator;
    public LayerMask groundLayer;


    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1;
    public float fallMultiplier = 5;


    [Header("Collisions")]
    public bool isGrounded = false;
    public float groundLength = 0.6f;

    


  
    private void Update()
    {
        GroundCheck();


    }

    private void GroundCheck()
    {
       
        if (Physics2D.Raycast(transform.position, Vector2.down, groundLength, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void keysPressed()
    {
        direction.x = 0;
        sprintSpeedMultiplier = 1f;
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
        if (Keyboard.current.spaceKey.isPressed && isGrounded)
        {
            Jump();
        }
        
    }


    public void move(InputAction.CallbackContext context)
    {
        direction.x = context.ReadValue<Vector2>().x;
        direction.y = context.ReadValue<Vector2>().y;
    }
       
    private void FixedUpdate()
    {
        keysPressed();
        moveCharacter(direction.x);
        modifyPhysics();
    }
    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed * sprintSpeedMultiplier);

        if(Mathf.Abs(rb.velocity.x) > maxSpeed * sprintSpeedMultiplier)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        //animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
    }
   
    void modifyPhysics()
    {
        bool changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if (isGrounded)
        {
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
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            } else if(rb.velocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
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
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundLength);
    }

}
