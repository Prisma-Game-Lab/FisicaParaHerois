using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[RequireComponent(typeof(Enemy))]
public class AttackState : IState
{
    public Enemy StateMachineController;

    public bool DoingAction = false; //Diz se está no meio de uma ação, nesse caso, a escolha de uma nova ação não ocorre
    private float _totalActionWeight; //Guarda a soma dos pesos das probabilidades das ações

    public void ChangeState(IState newState)
    {
        StateMachineController.CurrentState = newState;
        Debug.Log(newState);
    }

    public void UpdateState()
    {
        if (DoingAction)
        {
            return; //está ocupado realizando ação, não fazer nada
        }

        //Verificar se player está visível (se não, troca o estado)
        if (!StateMachineController.IsPlayerVisible())
        {
            ChangeState(StateMachineController.EnemySearchState);
            return;
        }

        //Escolhe ação
        DoingAction = true;
        EnemyActionInfo<float> actionToDo = ChooseAction();
        if(actionToDo != null)
        {
            actionToDo.Action.OnActionUse(actionToDo.Argument);
            Debug.Log(StateMachineController.gameObject.name + " usou a ação " + actionToDo.Description);
        }
        DoingAction = false;
    }

    EnemyActionInfo<float> ChooseAction()
    {
        //Soma das ações ainda não foi calculada
        if(_totalActionWeight == 0) 
        {
            _totalActionWeight = 0;

            //Para cada ação, soma a probabilidade da mesma na probabilidade total
            foreach (EnemyActionInfo<float> action in StateMachineController.Actions)
            {
                _totalActionWeight += action.Probability;
            }
        }

        //Define um número aleatório para escolher uma ação
        float actionChosen = Random.Range(0, _totalActionWeight);

        //Verifica a ação escolhida
        foreach(EnemyActionInfo<float> action in StateMachineController.Actions)
        {
            actionChosen -= action.Probability;
            if(actionChosen <= 0)
            {
                return action;
            }
        }

        return null;
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