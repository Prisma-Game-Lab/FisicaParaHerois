/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullAction : MonoBehaviour, IAction<float> {
    private PhysicsObject _object;
    static private string _actionName = "Push/Pull";

    void Start()
    {
        _object = gameObject.GetComponent<PhysicsObject>();
    }

    public string GetActionName()
    {
        return _actionName;
    }

    public float GetCurrentValue()
    {
        return 0; //não faz sentido GetCurrentValue para essa Action
    }

    public void SetTarget(PhysicsObject target)
    {
        _object = target;
    }

    public void Do(float temp)
    {
        _object.physicsData.AddForce(temp * Vector2.right);
    }
}
