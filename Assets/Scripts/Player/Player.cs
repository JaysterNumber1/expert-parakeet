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
    public Animator animator;
    private void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


    }
    public void move(InputActionReference action)
    {
        Debug.Log("Move");
    }
    private void FixedUpdate()
    {
        moveCharacter(direction.x);
    }
    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
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
