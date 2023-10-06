using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedPlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float jumpHeight = 7f;
    public float dashspped = 20f;
    public float crouchHeight = 0.5f;
    public LayerMask whatIsGround;
    public Transform groundCheckPoint;
    public float groundCheckRadious = 0.2f;

    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip footstepSound;

    private Rigidbody2D body;
    private Animator anim;
    private AudioSource audioSource;
    private bool grounded;
    private bool canDoubleJump = false;
    private bool isDashing = false;
    private bool isCrouching = false;
    private bool faceRight = true;

    // Start is called before the first frame update
    void Awake()
    {
       body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadious, whatIsGround);

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        anim.SetBool("Walk", horizontalInput != 0);

        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
            canDoubleJump = true;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
        {
            Jump();
            canDoubleJump = false;
        }

        if((horizontalInput>0 && !faceRight)||(horizontalInput<0 && faceRight))
        {
            Flip();
        }

        if(Input.GetKeyDown(KeyCode.DownArrow) && grounded)
        {
            if(!isCrouching)
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
                isCrouching = true;
            }
            else if(isCrouching)
            {
                transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
                isCrouching = false;
            }
        }
    }

    private void Jump()
    {
    body.velocity = new Vector2(body.velocity.x, jumpHeight);
    anim.SetTrigger("Jump");
    grounded = false;
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        faceRight = !faceRight;
    }
}