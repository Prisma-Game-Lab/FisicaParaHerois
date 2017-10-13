/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {

    void UpdateState();

    void ChangeState(IState newState);
}
