using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Guarda todas as informações que o inimigo necessita para escolher uma ação
/// </summary>
[System.Serializable]
public class EnemyActionInfo<T>
{
    public IAction<T> Action;
    public string Description; //Descrição da ação (aparece no Debug.Log quando é usada a ação)
    public float Probability; //Probabilidade do inimigo escolher a ação
    public T Argument; //Argumento que a ação recebe

    public EnemyActionInfo(IAction<T> action, float probability, T argument, string description = ""){
        Action = action;
        Probability = probability;
        Argument = argument;
        Description = description;
    }
}
