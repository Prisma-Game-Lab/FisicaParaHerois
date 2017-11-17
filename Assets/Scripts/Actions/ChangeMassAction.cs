/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]
public class ChangeMassAction : MonoBehaviour, IAction<float>
{
    static private string _actionName = "Change mass";

    private PhysicsObject _physicsObj;
    private float _defaultMass;

    public void OnActionUse(float newMass)
    {
        if (newMass >= 0)
        {
            _physicsObj.physicsData.mass = newMass;
        }
    }

    public void SetTarget(PhysicsObject target)
    {
        _physicsObj = target;
    }

    public string GetActionName()
    {
        return _actionName;
    }

    public float GetCurrentValue()
    {
        return _physicsObj.physicsData.mass;
    }

    // Use this for initialization
    void Start()
    {
        _physicsObj = gameObject.GetComponent<PhysicsObject>();
        _defaultMass = _physicsObj.physicsData.mass;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
