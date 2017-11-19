using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossIdleState : MonoBehaviour, IState
{
    [HideInInspector] public Boss StateMachineController;

    public float idleTime = 1.0f;

    public void Awake()
    {
        StateMachineController = GetComponent<Boss>();
    }


    public void ResumeState()
    {
        this.StartState();
    }

    public void StartState()
    {
        StartCoroutine("WaitAndChange");
    }

    public void StopState()
    {
        StopCoroutine("WaitAndChange");
    }

    public void UpdateState()
    {
        
    }

    IEnumerator WaitAndChange()
    {
        yield return new WaitForSeconds(idleTime);
        StateMachineController.ChangeState(StateMachineController.AttackState);
    }
}