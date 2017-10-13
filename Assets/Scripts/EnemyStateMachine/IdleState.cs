using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public Enemy StateMachineController;

    public void ChangeState(IState newState)
    {
        StateMachineController.CurrentState = newState;
        Debug.Log(newState);
    }

    public void UpdateState()
    {
        if (StateMachineController.IsPlayerVisible())
        {
            ChangeState(StateMachineController.EnemyAttackState);
        }
    }
}