/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]
public class ChangeSeesawAnchor : MonoBehaviour, IAction<float> {
	static private string _actionName = "Change anchor point";

	private PhysicsObject _physicsObj;

	private HingeJoint2D _hinge;

	public void OnActionUse(float newAnchor)
	{
		if (_hinge != null && Mathf.Abs (newAnchor) < 1.0f) {
			_hinge.anchor = new Vector2(newAnchor, 0.0f);
		}
	}

	public string GetActionName()
	{
		return _actionName;
	}

	public float GetCurrentValue()
	{
		if (_hinge) {
			return _hinge.anchor.x;
		}
		else return 0;
	}

	public void SetTarget(PhysicsObject target)
	{
		_physicsObj = target;
		_hinge = target.gameObject.GetComponent<HingeJoint2D> ();

	}

	// Use this for initialization
	void Start () {
		_physicsObj = gameObject.GetComponent<PhysicsObject>();

	}

	// Update is called once per frame
	void Update () {

	}
}
