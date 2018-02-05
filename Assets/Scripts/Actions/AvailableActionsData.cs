using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class AvailableActionsData {
    //Acrescentar actions conforme forem acrescentadas (usar o nome da classe)
    public bool ChangeGravityAction = true;
	public float ChangeGravityActionMinValue = -5;
	public float ChangeGravityActionMaxValue = 5;
	public bool ChangeMassAction = true;
	public float ChangeMassActionMinValue = -5;
	public float ChangeMassActionMaxValue = 5;
    public bool PushPullAction = true;
	public float ChangeAnchorPointMinValue = -1.0f;
	public float ChangeAnchorPointMaxValue = 1.0f;
	public bool ChangeAnchorPointAction = true;

    /// <summary>
    /// Checa se a ação está disponível
    /// </summary>
    /// <param name="action">nome da classe da ação</param>
    /// <returns></returns>
    public bool CheckIfActionIsAvailable(string action)
    {
        switch (action)
        {
            case "ChangeGravityAction":
                return ChangeGravityAction;
            case "ChangeMassAction":
                return ChangeMassAction;
            case "PushPullAction":
                return PushPullAction;
			case "ChangeAnchorPointAction":
				return  ChangeAnchorPointAction;
			default:
                Debug.LogError("Action não encontrada no dicionário de ações. A ação foi adicionada no arquivo AvaliableActionsData.cs?");
                return false;
        }
    }

    /// <summary>
    /// Indica se uma determinada ação está ou não disponível
    /// </summary>
    /// <param name="action">nome da classe da ação</param>
    /// <param name="value">ação está disponível?</param>
    public void SetIfActionIsAvailable(string action, bool value)
    {
        switch (action)
        {
            case "ChangeGravityAction":
                ChangeGravityAction = value;
                break;
            case "ChangeMassAction":
                ChangeMassAction = value;
                break;
            case "PushPullAction":
                PushPullAction = value;
                break;
			case  "ChangeAnchorPointAction":
				ChangeAnchorPointAction = value;
				break;
	        default:
                Debug.LogError("Action não encontrada no dicionário de ações. A ação foi adicionada no arquivo AvaliableActionsData.cs?");
                return;
        }
    }
}
