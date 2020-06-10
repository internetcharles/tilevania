using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float ladderSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new Vector2(25f, 25f);

    private bool isAlive = true;

    private Rigidbody2D MyRigidbody2D;
    private CapsuleCollider2D MyBodyCollider2D;
    private BoxCollider2D MyFeetCollider2D;
    private Animator MyAnimator;
    private float gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        MyRigidbody2D = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
        MyBodyCollider2D = GetComponent<CapsuleCollider2D>();
        MyFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = MyRigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        Run(); 
        FlipSprite();
        Jump();
        ClimbLadder();
        Die();
    }

    public void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 and +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, MyRigidbody2D.velocity.y);
        MyRigidbody2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(MyRigidbody2D.velocity.x) > Mathf.Epsilon;
        MyAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    public void Jump()
    {
        if (!MyFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            MyRigidbody2D.velocity += jumpVelocityToAdd;
        }
    }

    private void Die()
    {
        if (MyFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            MyAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            isAlive = false;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(MyRigidbody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        // if the player is moving horizontally
        {
            // reverse the current scaling
            transform.localScale = new Vector2(Mathf.Sign(MyRigidbody2D.velocity.x), 1f);
        }
    }

    public void ClimbLadder()
    {
        if (!MyFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            MyAnimator.SetBool("Climbing", false);
            MyRigidbody2D.gravityScale = gravityScaleAtStart;
            return;
        }
        else
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // value is between -1 and +1
            Vector2 ladderVelocity = new Vector2(MyRigidbody2D.velocity.x, controlThrow * ladderSpeed);
            MyRigidbody2D.velocity = ladderVelocity;
            MyRigidbody2D.gravityScale = 0f;

            bool playerHasVerticalSpeed = Mathf.Abs(MyRigidbody2D.velocity.y) > Mathf.Epsilon;
            MyAnimator.SetBool("Climbing", true);
        }
    }


}
