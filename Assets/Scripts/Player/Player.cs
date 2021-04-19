using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{


    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
   // private bool facingRight = true;

    [Header("Components")]
    public Rigidbody2D rb;
    // public Animator animator;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;



    private void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


    }
    public void move(InputActionReference action)
    {
        Debug.Log("Move");
    }
    private void FixedUpdate()
    {
        moveCharacter(direction.x);
        modifyPhysics();
    }
    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if(Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        //animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
    }
   
    void modifyPhysics()
    {
        bool changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if(Mathf.Abs(direction.x) < 0.9 || changingDirection)
        {
            rb.drag = linearDrag;
        }
        else
        {
            rb.drag = 0;
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
    //I am 7:50 in the movement video by Press Start for mariolike movement
}
