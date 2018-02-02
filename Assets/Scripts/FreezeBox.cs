using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBox : MonoBehaviour {
    private RigidbodyConstraints2D _defaultConstraints;

	// Use this for initialization
	void Start () {
        _defaultConstraints = GetComponent<Rigidbody2D>().constraints;
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
}
