using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogTrigger : MonoBehaviour {
	public TutorialDialog DialogToActivate;
	public GameObject PlanoFechado;
    public Animator Anim;

	// Use this for initialization
	void OnTriggerStay2D(Collider2D other){
		DialogToActivate.OnTrigger(other, PlanoFechado);
        Anim.SetBool("tutorial", true);
	}
}
