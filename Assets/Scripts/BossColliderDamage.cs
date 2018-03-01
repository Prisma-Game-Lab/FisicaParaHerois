using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossColliderDamage : MonoBehaviour {
	public Health BossHealth;

	void OnCollisionEnter2D(Collision2D col){
		if (col.collider.tag == "Player") {
			BossHealth.TakeDamage (1);
			Debug.Log ("Player acertou hit no boss");
		}

	}
}
