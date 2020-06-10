using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;

    private Rigidbody2D MyRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        MyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }


    public void MoveEnemy()
    {
        if (IsFacingRight())
        {
            Vector2 enemyVelocity = new Vector2(moveSpeed, 0f);
            MyRigidbody2D.velocity = enemyVelocity;
        }
        else
        {
            Vector2 enemyVelocity = new Vector2(-moveSpeed, 0f);
            MyRigidbody2D.velocity = enemyVelocity;
        }
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(MyRigidbody2D.velocity.x)), 1f);
    }

}
