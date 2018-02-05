using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBox : MonoBehaviour {
    private RigidbodyConstraints2D _defaultConstraints;
    private bool _touchingFloor = false;

	// Use this for initialization
	void Start () {
        _defaultConstraints = GetComponent<Rigidbody2D>().constraints;
	}

    void Update()
    {
        if (!_touchingFloor)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

	void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            }
        }
	}

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.gameObject.CompareTag("Floor") || col.collider.gameObject.CompareTag("Gangorra"))
        {
            _touchingFloor = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.gameObject.CompareTag("Floor"))
        {
            _touchingFloor = false;
        }
    }
}
