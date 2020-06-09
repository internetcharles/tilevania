﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float ladderSpeed = 5f;

    private bool isAlive = true;

    private Rigidbody2D MyRigidbody2D;
    private Collider2D MyCollider2D;
    private Animator MyAnimator;
    private float gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        MyRigidbody2D = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
        MyCollider2D = GetComponent<Collider2D>();
        gravityScaleAtStart = MyRigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run(); 
        FlipSprite();
        Jump();
        ClimbLadder();
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
        if (!MyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            MyRigidbody2D.velocity += jumpVelocityToAdd;
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
        if (!MyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
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
