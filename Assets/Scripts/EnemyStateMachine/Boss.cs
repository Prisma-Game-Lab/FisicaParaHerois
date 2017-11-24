/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BossAttackState))]
[RequireComponent(typeof(BossIdleState))]
public class Boss : MonoBehaviour {
    public List<EnemyActionInfo<float>> Actions = new List<EnemyActionInfo<float>>();
    public float Life = 100;



    //States
    //attack state cria uma nova caixa, flutua com ela até em cima do player e a derruba
    //idle state espera n segundos e começa attack state
    [HideInInspector] public IState CurrentState;
    [HideInInspector] public BossAttackState AttackState;
    [HideInInspector] public BossIdleState IdleState;


    public void ChangeState(IState newState)
    {
        CurrentState.StopState();
        CurrentState = newState;
        CurrentState.StartState();
    }

    // Use this for initialization
    void Awake()
    {
        AttackState = GetComponent<BossAttackState>();
        IdleState = GetComponent<BossIdleState>();

        AttackState.StateMachineController = this;
        IdleState.StateMachineController = this;

        //Enquanto não estou conseguindo colocar as Actions para aparecer no Inspector
        //Actions.Add(new EnemyActionInfo<float>(new PushPullAction(), 0.5f, -1, "push pull action negativa"));
        //Actions.Add(new EnemyActionInfo<float>(new PushPullAction(), 0.5f, 1, "push pull action positiva"));
    }

    void Start () {
        CurrentState = IdleState;
        CurrentState.StartState();
        Debug.Log("start\n");
	}
	
	// Update is called once per frame
	void Update () {

        //Checa se inimigo está vivo
        //faria uma interface IKillable depois
        if (Life <= 0)
        {
            OnDeath();
        }

        //se inimigo for atingido, interrompe estado atual, fica stunado, e depois recomeça estado atual


        //Roda o estado
        CurrentState.UpdateState();
	}

    /// <summary>
    /// Define o que ocorre quando o inimigo morre
    /// </summary>
    void OnDeath()
    {
        Destroy(this);
    }

    
}
