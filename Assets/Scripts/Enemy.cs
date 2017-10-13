/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour {
    public List<EnemyActionInfo<float>> Actions = new List<EnemyActionInfo<float>>();

    public float Life = 100;

    //States
    [HideInInspector] public IState CurrentState;
    [HideInInspector] public SearchState EnemySearchState;
    [HideInInspector] public AttackState EnemyAttackState;
    [HideInInspector] public IdleState EnemyIdleState;

    // Use this for initialization
    void Awake()
    {
        EnemySearchState = new SearchState();
        EnemyAttackState = new AttackState();
        EnemyIdleState = new IdleState();

        EnemySearchState.StateMachineController = this;
        EnemyAttackState.StateMachineController = this;
        EnemyIdleState.StateMachineController = this;

        //Enquanto não estou conseguindo colocar as Actions para aparecer no Inspector
        Actions.Add(new EnemyActionInfo<float>(new PushPullAction(), 0.5f, -1, "push pull action negativa"));
        Actions.Add(new EnemyActionInfo<float>(new PushPullAction(), 0.5f, 1, "push pull action positiva"));
    }

    void Start () {
        CurrentState = EnemyIdleState;
	}
	
	// Update is called once per frame
	void Update () {
        //Checa se inimigo está vivo
		if(Life <= 0)
        {
            OnDeath();
        }

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

    /// <summary>
    /// Diz se o enemy consegue enxergar o player
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerVisible()
    {
        //Somente para teste, depois será implementado algo para checar se o player está próximo
        if (Input.GetKey("1"))
        {
            return true;
        }

        return false;
    }
}
