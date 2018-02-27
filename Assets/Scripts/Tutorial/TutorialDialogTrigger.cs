using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogTrigger : MonoBehaviour {
	public TutorialDialog DialogToActivate;
	public GameObject PlanoFechado;

	// Use this for initialization
	void OnTriggerStay2D(Collider2D other){
		DialogToActivate.OnTriggerStay2D(other, PlanoFechado);
	}
}
