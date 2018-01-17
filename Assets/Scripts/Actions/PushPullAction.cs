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
    private float tempMultiplier = 100;

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
        if (target != null && !target.AvailableActions.PushPullAction)
        {
            return;
        }

        _object = target;
    }

    public void OnActionUse(float temp)
    {
        
        //_object.physicsData.AddForce(temp * tempMultiplier * Vector2.right);
        _object.SendMessage("OnPushPullActionUsed", null, SendMessageOptions.DontRequireReceiver);
    }
}
