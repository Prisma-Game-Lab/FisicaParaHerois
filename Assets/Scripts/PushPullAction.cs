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

    void Start()
    {
        _object = gameObject.GetComponent<PhysicsObject>();
    }

    public void Do(float temp)
    {
        _object.physicsData.AddForce(temp * Vector2.right);
    }
}
