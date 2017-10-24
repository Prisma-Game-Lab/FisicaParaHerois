using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesControl : MonoBehaviour {

	public ParticleSystem particleSystem;

	public void Play(){

		particleSystem.Play ();

	}

	public void Stop(){

		particleSystem.Stop ();

	}

}
