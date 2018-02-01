﻿/* 
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
    public GameObject HaloPrefab;
    public AvailableActionsData AvailableActions;
    public bool CanPlayerInteract = true; //Define se o player pode interagir com esse objeto
    [HideInInspector] public Vector3 OldPosition = Vector3.negativeInfinity;

    private bool _pushPullAction = false;
    private float _realMass;
    public float _timeLeftToDeactivatePushPullAction = 0.5f;
    public Behaviour Halo;
    [HideInInspector] public bool _hasChain = false;

    private RigidbodyConstraints2D _defaultConstraints;

    [Header("DEBUG")]
    public bool PhysicsVisionIsReady = false;

    void OnValidate()
    {
        if (!PhysicsVisionIsReady)
        {
            return;
        }

        //Usado para a visão física
        if (Halo == null)
        {
            Halo = (Behaviour)GetComponent("Halo");
            if (HaloPrefab == null)
            {
                Debug.LogError("Halo não está setado no objeto " + gameObject.name);
                return;
            }

            else
            {
                Halo = Instantiate(HaloPrefab, transform.position, Quaternion.identity, transform).GetComponent<Behaviour>();
            }
        }

        Halo.enabled = false;
    }

	// Use this for initialization
	void Start () {
        //Pega a referência do halo
        if (PhysicsVisionIsReady)
        {
            OnValidate();
        }

        if(gameObject.GetComponent<HingeJoint2D>() != null)
        {
            _hasChain = true;
        }

        _defaultConstraints = physicsData.constraints;
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
                _pushPullAction = false;

                //if(PlayerInfo.PlayerInstance.ObjectColliding == this)
                if (PlayerInfo.PlayerInstance.PushPullJoint.connectedBody == physicsData)
                {
                    Debug.Log("Desativando");
                    PlayerInfo.PlayerInstance.PushPullJoint.connectedBody = null;
                    PlayerInfo.PlayerInstance.PushPullJoint.enabled = false;
                    //PlayerInfo.PlayerInstance.ObjectColliding = null;
                    //physicsData.mass = _realMass;
                }
                 
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
		if (CanPlayerInteract) {
			ActionPanel.Instance.OnPanelActivated(this);
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Se player não estiver envolvido na colisão, não faça nada
		if (((collision.collider.gameObject != PlayerInfo.PlayerInstance.gameObject) &&  /*player não está envolvido na colisão*/
            (collision.otherCollider.gameObject != PlayerInfo.PlayerInstance.gameObject)) || /*player não está envolvido na colisão*/
            (this.gameObject == PlayerInfo.PlayerInstance.gameObject) /*é o player*/) {
			return;
		} else {
			//Physics2D.GetIgnoreCollision(collision.collider.gameObject, this.GetComponent<Collider2D>(),true)
			//Debug.Log ("TOCO NO PLAYER");
		}

        if (!_pushPullAction)
        {
            physicsData.constraints = RigidbodyConstraints2D.FreezePosition;
            return;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Se player não estiver envolvido na colisão, não faça nada
        if (((collision.collider.gameObject != PlayerInfo.PlayerInstance.gameObject && (collision.otherCollider.gameObject != PlayerInfo.PlayerInstance.gameObject)) || this.gameObject == PlayerInfo.PlayerInstance.gameObject))
        {
            return;
        }

        if (!_pushPullAction && gameObject != PlayerInfo.PlayerInstance.gameObject)
        {
            physicsData.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        else
        {
            physicsData.constraints = _defaultConstraints;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        OldPosition = physicsData.position;
        physicsData.constraints = _defaultConstraints;
    }

    public void OnPushPullActionUsed()
    {
        if (_hasChain)
        {
            return;
        }

        _pushPullAction = true;
        _timeLeftToDeactivatePushPullAction = 0.2f;
        PlayerInfo.PlayerInstance.PushPullJoint.enabled = true;
        PlayerInfo.PlayerInstance.PushPullJoint.connectedBody = physicsData;
    }

    public void OnPhysicsVisionActivated()
    {
        if (Halo == null)
        {
            return;
        }
        Halo.enabled = true;
    }

    public void OnPhysicsVisionDeActivated()
    {
        if (Halo == null)
        {
            return;
        }
        Halo.enabled = false;
    }
}
