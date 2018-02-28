using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour {

	public AudioClip select;

	// Use this for initialization
	void Start () 
	{
		AudioSource.PlayClipAtPoint (select, transform.position);

	}

}
