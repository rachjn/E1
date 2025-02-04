using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 1f;
    float direction = 0f;
    [SerializeField] float jumpHeight = 3f;
    bool isGrounded = false;
    bool canDash = true;
    bool isDashing;
    float dashingPower = 24f;
    float dashingTime = 0.2f;
    float dashingCooldown = 0.8f;
    [SerializeField] TrailRenderer tr;
    bool doubleJump;
    bool isFacingRight = true;

    Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (isDashing)
        {
            return;
        }
        Move(direction);
        if ((isFacingRight && direction == -1) || (!isFacingRight && direction == 1)) {
            Flip();
        }
    }
    void OnMove(InputValue value) {
        float v = value.Get<float>();
        direction = v;
    }

    void Move(float dir) {
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
        anim.SetBool("isRunning", dir != 0);
    }

    void OnJump() {
        if(isGrounded || doubleJump)
        {
            Jump();
            doubleJump = !doubleJump;
        }
    }

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    void OnCollisionEnter2D(Collision2D collision) {
       
    }

    void OnCollisionStay2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            for (int i = 0; i < collision.contactCount; i++ ) {
                if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) < 45f) {
                    isGrounded = true;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnDash() {
        if (canDash) {
            StartCoroutine(Dash(direction));
        }
    }

    private IEnumerator Dash(float dir) {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dir * dashingPower, 0f); // fix
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void Flip() {
        isFacingRight = !isFacingRight;
        Vector3 newLocalScale = transform.localScale;
        newLocalScale.x *= -1f;
        transform.localScale = newLocalScale;
    }
    
}
