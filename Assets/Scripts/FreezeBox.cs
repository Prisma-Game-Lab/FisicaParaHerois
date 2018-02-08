using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBox : MonoBehaviour {
    private Rigidbody2D _rb;
    private RigidbodyConstraints2D _defaultConstraints;
    [HideInInspector] public BoxCollider2D Collider;
    //private bool _touchingFloor = false;

	// Use this for initialization
	void Start () {
        Collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _defaultConstraints = _rb.constraints;
	}

    void Update()
    {
        if (_rb.gravityScale < 0 /*!_touchingFloor*/ && ActionPanel.Instance.gameObject.activeInHierarchy == false)
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

    /*
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
     * */
}
