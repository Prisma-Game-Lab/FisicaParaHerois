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
    static private string _actionName = "Change gravity";

    private PhysicsObject _physicsObj;

    public void OnActionUse(float newGravity)
    {
        _physicsObj.physicsData.gravityScale = newGravity;
    }

    public string GetActionName()
    {
        return _actionName;
    }

    public float GetCurrentValue()
    {
        return _physicsObj.physicsData.gravityScale;
    }

    public void SetTarget(PhysicsObject target)
    {
        _physicsObj = target;
    }

    // Use this for initialization
    void Start () {
        _physicsObj = gameObject.GetComponent<PhysicsObject>();
            
    }
	
	// Update is called once per frame
	void Update () {

	}
}
