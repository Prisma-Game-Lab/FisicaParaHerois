using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    public float Speed = 1f;
    public float maxHeight = 3f;
    public float minHeight = -3f;
    public Rigidbody2D rb;

    private Vector3 _initialPos;


    void OnValidate()
    {
        _initialPos = transform.position;
    }

    void Awake()
    {
        OnValidate();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 velocity = playerRb.velocity;
                velocity.y = Speed;
                playerRb.velocity = velocity;
                rb.velocity = velocity;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 velocity = Vector2.zero;
            rb.velocity = velocity;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_initialPos + new Vector3(0, minHeight, 0), _initialPos + new Vector3(0, maxHeight, 0));
    }
}
