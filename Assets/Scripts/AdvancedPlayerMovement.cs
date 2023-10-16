using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AdvancedPlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float speed = 10f;
    public float jumpHeight = 7f; 
    public float dashSpeed = 20f;
    public float crouchHeight = 0.5f;
    public bool canDoubleJump = false;
    private bool isDashing = false; 
    private bool isCrouching = false;
    private bool facingRight = true;
    private Rigidbody2D body; 
    private Animator anim;  

    [Header("Ground Check")]
    public LayerMask whatIsGround; 
    public Transform groundCheckPoint; 
    public float groundCheckRadius = 0.2f; 

    [Header("Combat")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackRange = 1f;
    public LayerMask enemyLayers;
    public bool grounded;  

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        handleInput();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

        handleMovement();
    }

    private void handleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(horizontalInput*speed, body.velocity.y);
        if(horizontalInput != 0 && grounded)
        {
            AudioManager.instance.PlayFootstepSound();
        }
        anim.SetBool("walk", horizontalInput !=0);
        if((horizontalInput>0&& !facingRight)||(horizontalInput<0&&facingRight))
        {
            Flip();
        }
    }

    private void handleJump()
    {
        if(Input.GetKeyDown(KeyCode.Space)&& grounded)
        {
            canDoubleJump = true; 
            Jump();   
        }
        else if(Input.GetKeyDown(KeyCode.Space)&&canDoubleJump)
        {
            Jump();
            canDoubleJump = false;
        }
    }

    private void handleDash()
    {
        if(Input.GetKey(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private void handleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)&& grounded)
        {
            if(!isCrouching){
                transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
                isCrouching = true;
            }
        }
        else if (isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x,1f,transform.localScale.z);
            isCrouching = false; 
        }
    }
    
    private void handleAttack()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }

    private void handleInput()
    {
        handleJump();
        handleDash();
        handleCrouch();
        handleAttack();
    }

    private void Flip(){
        Vector3 currentScale = gameObject.transform.localScale; 
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale; 
        facingRight = !facingRight; 
    }
    private void Jump(){
        body.velocity = new Vector2(body.velocity.x, jumpHeight);
        anim.SetTrigger("jump");
        grounded = false;
        AudioManager.instance.PlayJumpSound();
    }

    IEnumerator Dash(){
        AudioManager.instance.PlayDashSound();
        float originalSpeed = speed; 
        speed = dashSpeed; 
        isDashing = true; 
        yield return new WaitForSeconds(0.2f);
        speed = originalSpeed; 
        isDashing = false; 
    }
    void Attack()
    {
        AudioManager.instance.PlayAttackSound();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if(enemyController !=null)
            {
                enemyController.TakeDamage(attackDamage);
                Debug.Log("Enemy Damaged");

            }
        }
    }
    void OnDrawGizmosSelected() {
    Gizmos.color = Color.red; 
    Gizmos.DrawWireSphere(transform.position,attackRange);    
    }
}
