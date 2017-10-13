/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : IState
{
    public Enemy StateMachineController;

    public void ChangeState(IState newState)
    {
        StateMachineController.CurrentState = newState;
        Debug.Log(newState);
    }

    public void UpdateState()
    {
        //Se player está visível, vai para o estado de ataque
        if (StateMachineController.IsPlayerVisible())
        {
            ChangeState(StateMachineController.EnemyAttackState);
        }
    }
}
