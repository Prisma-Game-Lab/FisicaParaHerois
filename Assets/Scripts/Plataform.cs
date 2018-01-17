using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag("Player")) {
		}
	}

	void OnTriggerStay2D(Collider2D other){

		if (other.gameObject.CompareTag("Player")) {
			Debug.Log (other.transform.position.x);

			other.transform.parent = transform.parent;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag("Player")) {
			other.transform.parent = null;
		}
	}
		
}
