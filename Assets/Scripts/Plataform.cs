using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){

		if (!(other.gameObject.name == "FloorTiles") || !(other.gameObject.name == "Gangorra")) {
		}
	}

	void OnTriggerStay2D(Collider2D other){

		if (!(other.gameObject.name == "FloorTiles") || !(other.gameObject.name == "Gangorra")) {
			other.transform.parent = transform.parent;
		}
		/*if (other.gameObject.CompareTag("Player")) {
			other.transform.parent = transform.parent;
		}*/
		//other.transform.parent = transform.parent;
	}

	void OnTriggerExit2D(Collider2D other){

		if (!(other.gameObject.name == "FloorTiles") || !(other.gameObject.name == "Gangorra")) {
			other.transform.parent = null;
		}
		/*if (other.gameObject.CompareTag("Player")) {
			other.transform.parent = null;
		}*/
	}
		
}
