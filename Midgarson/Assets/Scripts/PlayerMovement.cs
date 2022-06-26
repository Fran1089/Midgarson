using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveForce = 8f;
    [SerializeField]
    private float jumpForce = 5f;
    private float movementX, yVelocity;
    private bool bend, isGrounded, isOnStairs;
    private Rigidbody2D myBody;
    private SpriteRenderer mySprite;

    private string WALK_ANIMATION = "Walk";
    private string RUN_ANIMATION = "Run";
    private string JUMP_ANIMATION = "Jump";
    private string FALL_ANIMATION = "Fall";
    private string ATTACK_ANIMATION = "Attack";
    private Animator anim;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {

        yVelocity = myBody.velocity.y;

        if (yVelocity < 0 && !isOnStairs)
        {
            anim.SetBool(FALL_ANIMATION, true);
        }

        PlayerMoveKeyboard();
        WalkAnimation();
        Run();
        Jump();
        Attack();
    }
    void PlayerMoveKeyboard()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        bend = Input.GetButtonDown("Fire1");



        /*anim.SetBool(WALK_ANIMATION, movementX != 0.0f && movementX2 == 0f);
         anim.SetBool(RUN_ANIMATION, movementX != 0.0f && movementX2 != 0f);
        anim.SetBool(BEND_ANIMATION, movementX == 0.0f && movementX2 == 0f && bend);
        anim.SetBool(JUMP_ANIMATION, false);*/


        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime * moveForce;

        anim.SetFloat("yVelocity", yVelocity);
    }

    void WalkAnimation()
    {
        if (movementX > 0 && isGrounded && yVelocity == 0)
        {
            mySprite.flipX = false;
            anim.SetBool(WALK_ANIMATION, true);
        }
        else if (movementX < 0 && isGrounded && yVelocity == 0)
        {
            mySprite.flipX = true;
            anim.SetBool(WALK_ANIMATION, true);
        }
        else
        {
            anim.SetBool(WALK_ANIMATION, false);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            anim.SetBool(JUMP_ANIMATION, true);
        }
    }

    void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && movementX != 0 && isGrounded && yVelocity == 0)
        {
            moveForce = 12f;
            anim.SetBool(RUN_ANIMATION, true);
        }
        if (Input.GetKey(KeyCode.LeftShift) && movementX != 0 && isGrounded && yVelocity == 0)
        {
            anim.SetBool(RUN_ANIMATION, true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveForce = 8f;
            anim.SetBool(RUN_ANIMATION, false);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            anim.SetBool(ATTACK_ANIMATION, true);
        }

        if (Input.GetMouseButtonUp(0) && isGrounded)
        {
            anim.SetBool(ATTACK_ANIMATION, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isOnStairs = false;
            anim.SetBool(JUMP_ANIMATION, false);
            anim.SetBool(FALL_ANIMATION, false);
            myBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if (collision.gameObject.CompareTag("Stairs"))
        {
            isGrounded = true;
            isOnStairs = true;
            anim.SetBool(JUMP_ANIMATION, false);
            anim.SetBool(FALL_ANIMATION, false);
            myBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
