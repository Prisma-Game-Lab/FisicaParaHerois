/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsObject : MonoBehaviour {

    [HideInInspector]
    public Rigidbody2D physicsData;

	// Use this for initialization
	void Start () {
        physicsData = gameObject.GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
