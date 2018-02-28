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
    public Animator Animator;
	public AudioClip Press;
	public AudioClip Solto;



    [Header("Pressure Plate")]
    [Tooltip("Guarda a massa mínima que o pressure plate necessita para ser ativado")]public float MinMass = 2;

    [Header("Lever")]
    [Tooltip("Indica se é uma alavanca, ou seja, se uma vez pressionada não volta ao estado original")]public bool IsLever = false;

    [Header("FacingUP")]
    [Tooltip("Determina se o sprite da pressure plate está voltado para cima ou para baixo")]
    public bool FaceUP = true;

    private bool _isActive = false;


	// Use this for initialization
	void Start () {
        if(Animator == null)
        {
            Animator = GetComponent<Animator>();
        }

        Animator.SetBool("faceUp", FaceUP);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collider)
    {
        float objectMass;
        
		bool objectIsValid = collider.gameObject == PlayerInfo.PlayerInstance.gameObject
		                     || collider.tag == "Box";

        //Objeto não é um player ou caixa (ou já está ativo), nada acontece
        if (!objectIsValid || _isActive)
        {
            return;
        }

        Animator.SetBool("on", true);

        if (IsLever)
        {
            return;
        }

        objectMass = collider.gameObject.GetComponent<PhysicsObject>().physicsData.mass;

        //Não faz o efeito da pressure plate a menos que o peso seja maior que o mínimo necessário (ou o objeto seja uma lever)
        if (MinMass > objectMass)
        {
            return;
        }

        Debug.Log("Botão pressionado");
        ObjectAffected.OnPressed();
        _isActive = true;
		AudioSource.PlayClipAtPoint (Press, new Vector3 (5, 1, 2));

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
		bool objectIsValid = collider.gameObject == PlayerInfo.PlayerInstance.gameObject
		                           || collider.tag == "Box";

        Animator.SetBool("on", false);

        if (objectIsValid && !IsLever && _isActive)
        {
            Debug.Log("Botão solto");
            
            ObjectAffected.OnUnpressed();
            _isActive = false;
			AudioSource.PlayClipAtPoint (Solto, new Vector3 (5, 1, 2));

        }
    }

    public void OnPushPullActionUsed()
    {
        if (!IsLever || _isActive)
        {
            return;
        }

        Debug.Log("Botão pressionado");

        if (ObjectAffected != null)
        {
            ObjectAffected.OnPressed();
        }

        _isActive = true;
    }


}
