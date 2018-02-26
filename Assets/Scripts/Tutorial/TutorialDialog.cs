using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialog : MonoBehaviour {
	private bool _alreadyDisplayedText = false;

	public void OnTriggerStay2D(Collider2D other){
		if (!_alreadyDisplayedText) {
			ShowText ();
		}
	}

	public void ShowText(){
		gameObject.SetActive (true);
		_alreadyDisplayedText = true;
	}

	public void HideText(){
		gameObject.SetActive (false);
	}
}

