/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsObject : MonoBehaviour {

    [HideInInspector]
    public Rigidbody2D physicsData;

    public Sprite ObjectSprite;
    public AvailableActionsData AvailableActions;
    public bool CanPlayerInteract = true; //Define se o player pode interagir com esse objeto

	// Use this for initialization
	void Start () {
    }

    void Awake()
    {
        physicsData = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
    }

    void OnMouseDown()
    {
        //Checa se o painel já está ativo
        if (ActionPanel.Instance.gameObject.activeInHierarchy)
        {
            return;
        }

        //Senão, ativa com o objeto selecionado
        ActionPanel.Instance.OnPanelActivated(this);
    }
}
