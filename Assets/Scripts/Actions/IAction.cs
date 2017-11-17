/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction<T> {
    
    void OnActionUse(T parameter);
    void SetTarget(PhysicsObject target);
    string GetActionName();
    float GetCurrentValue();
}
