using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    public Vector2 Speed = new Vector2(0,1f);
    public Vector2 MaxPos = new Vector2(0,3f);
    public Vector2 MinPos = new Vector2(0,-3f);
    public Rigidbody2D rb;

    private Vector3 _initialPos, _maxPos, _minPos;


    void OnValidate()
    {
        _initialPos = transform.position;
        _minPos = new Vector3(_initialPos.x + MinPos.x, _initialPos.y + MinPos.y, _initialPos.z);
        _maxPos = new Vector3(_initialPos.x + MaxPos.x, _initialPos.y + MaxPos.y, _initialPos.z);
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
                Vector2 velocity = Speed;
                //playerRb.velocity = velocity;
                rb.velocity = velocity;
            }
        }

        if ((transform.position.y >= _maxPos.y && Speed.y > 0) || (transform.position.y <= _minPos.y && Speed.y < 0))
        {
            Speed = new Vector2(Speed.x, -1*Speed.y);
        }

        if ((transform.position.x >= _maxPos.x && Speed.x > 0) || (transform.position.x <= _minPos.x && Speed.x < 0))
        {
            Speed = new Vector2(-1 * Speed.x, Speed.y);
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
        Gizmos.DrawLine(_minPos, _maxPos);
    }
}
