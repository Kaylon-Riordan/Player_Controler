using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float jumpHeight = 7f;
    private Rigidbody2D rBody;
    private Animator anim;
    private bool grounded;
    private bool faceRight = true;
    private void Awake(){
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rBody.velocity = new Vector2(horizontalInput * speed, rBody.velocity.y);
        anim.SetBool("Walk", horizontalInput!=0);

        if((horizontalInput>0 && !faceRight)||(horizontalInput<0 && faceRight)){
            Flip();
        }

        if(Input.GetKey(KeyCode.Space) && grounded){
            jump();
        }
    }

    private void jump(){
        rBody.velocity = new Vector2(rBody.velocity.x, jumpHeight);
        anim.SetTrigger("Jump");
        grounded = false;
    }
    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Ground"))
        grounded = true;
    }
    private void Flip(){
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        faceRight = !faceRight;
    }
}
