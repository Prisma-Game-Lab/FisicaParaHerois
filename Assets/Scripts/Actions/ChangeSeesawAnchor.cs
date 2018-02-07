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

	private Gangorra _gan;

	private HingeJoint2D _hinge;

	private GameObject _base;

	public void OnActionUse(float newAnchor)
	{
		if (_hinge != null && Mathf.Abs (newAnchor) < 1.0f) {
			_hinge.anchor = new Vector2(newAnchor / 2.0f, 0.0f);
			_physicsObj.transform.SetPositionAndRotation (new Vector3 (_physicsObj.transform.position.x, _base.transform.position.y + _gan.visual_distance_connecting_point, _physicsObj.transform.position.z), Quaternion.identity);
			_base.transform.SetPositionAndRotation (new Vector3 (_physicsObj.transform.position.x + _hinge.anchor.x * _physicsObj.transform.lossyScale.x, _base.transform.position.y, _base.transform.position.z), _base.transform.rotation);

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
		//o objeto deve ser uma gangorra:
		if(!target.CompareTag("Gangorra")) _physicsObj = null;

		_physicsObj = target;
		_hinge = target.gameObject.GetComponent<HingeJoint2D> ();
		//objeto tem que ter um hingeJoint2D
		if (_hinge == null) {
			_physicsObj = null;
			return;
		}
		
		_gan = _physicsObj.GetComponent<Gangorra> ();
		_base = _gan.base_gangorra;

	}

	// Use this for initialization
	void Start () {
		_physicsObj = gameObject.GetComponent<PhysicsObject>();

	}

	// Update is called once per frame
	void Update () {

	}
}
