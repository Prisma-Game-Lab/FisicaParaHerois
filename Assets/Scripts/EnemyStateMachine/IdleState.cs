using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[RequireComponent(typeof(Enemy))]
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

    void IState.ResumeState()
    {
        throw new System.NotImplementedException();
    }

    void IState.StartState()
    {
        throw new System.NotImplementedException();
    }

    void IState.StopState()
    {
        throw new System.NotImplementedException();
    }

    void IState.UpdateState()
    {
        throw new System.NotImplementedException();
    }
}