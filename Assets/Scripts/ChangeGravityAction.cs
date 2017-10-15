/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]
public class ChangeGravityAction : MonoBehaviour, IAction<float> {

    

    private PhysicsObject _physicsObj;

    public void Do(float newGravity)
    {
        _physicsObj.physicsData.gravityScale = newGravity;
    }

    // Use this for initialization
    void Start () {
        _physicsObj = gameObject.GetComponent<PhysicsObject>();
            
    }
	
	// Update is called once per frame
	void Update () {

	}
}
