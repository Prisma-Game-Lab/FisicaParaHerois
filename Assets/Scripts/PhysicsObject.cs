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

    private bool _pushPullAction = false;
    public float _timeLeftToDeactivatePushPullAction = 1;
    [HideInInspector] public bool _hasChain = false;

	// Use this for initialization
	void Start () {
        if(gameObject.GetComponent<HingeJoint2D>() != null)
        {
            _hasChain = true;
        }
    }

    void Awake()
    {
        physicsData = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        if (_pushPullAction)
        {
            if (_timeLeftToDeactivatePushPullAction > 0)
            {
                _timeLeftToDeactivatePushPullAction -= Time.deltaTime;
            }

            else
            {
                _timeLeftToDeactivatePushPullAction = 1;
                _pushPullAction = false;
            }
        }
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

    void OnCollisionStay2D(Collision2D collision)
    {
        //Se player não estiver envolvido na colisão, não faça nada
        if (collision.collider.gameObject != PlayerInfo.PlayerInstance.gameObject)
        {
            return;
        }

        if (!_pushPullAction)
        {
            physicsData.velocity = new Vector2(0, 0);
        }

        else
        {
            physicsData.AddForce(new Vector2(PlayerInfo.PlayerInstance.ForceToApplyOnObject,0));
        }
    }

    public void OnPushPullActionUsed()
    {
        if (_hasChain)
        {
            return;
        }

        _pushPullAction = true;
    }
}
