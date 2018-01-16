/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {
    public AffectedByPressurePlate ObjectAffected;

    [Header("Pressure Plate")]
    [Tooltip("Guarda a massa mínima que o pressure plate necessita para ser ativado")]public float MinMass = 2;

    [Header("Lever")]
    [Tooltip("Indica se é uma alavanca, ou seja, se uma vez pressionada não volta ao estado original")]public bool IsLever = false;

    private bool _isActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float objectMass;
        bool objectIsValid = collision.collider.gameObject == PlayerInfo.PlayerInstance.gameObject 
            || collision.otherCollider.gameObject == PlayerInfo.PlayerInstance.gameObject
            || collision.collider.tag == "Box"
            || collision.otherCollider.tag == "Box";

        //Objeto não é um player ou caixa (ou já está ativo), nada acontece
        if(!objectIsValid || _isActive){
            return;
        }
        
        objectMass = collision.collider.gameObject.GetComponent<PhysicsObject>().physicsData.mass;

        //Não faz o efeito da pressure plate a menos que o peso seja maior que o mínimo necessário (ou o objeto seja uma lever)
        if ((MinMass > objectMass) && !IsLever)
        {
            return;
        }

        Debug.Log("Porta aberta");
        ObjectAffected.OnPressed();
        _isActive = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        bool objectIsValid = collision.collider.gameObject == PlayerInfo.PlayerInstance.gameObject
            || collision.otherCollider.gameObject == PlayerInfo.PlayerInstance.gameObject
            || collision.collider.tag == "Box"
            || collision.otherCollider.tag == "Box";

        if (objectIsValid && !IsLever && _isActive)
        {
            Debug.Log("Porta fechada");
            ObjectAffected.OnUnpressed();
            _isActive = false;
        }
    }
}
