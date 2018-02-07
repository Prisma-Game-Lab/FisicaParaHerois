using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gangorra : MonoBehaviour {

	public GameObject base_gangorra;
	public float visual_distance_connecting_point;

	// Use this for initialization
	void Start () {
		visual_distance_connecting_point = this.transform.position.y - base_gangorra.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
