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
    public static List<PhysicsObject> PhysicsObjectList;

    [HideInInspector]
    public Rigidbody2D physicsData;

    public Sprite ObjectSprite;
    public GameObject HaloPrefab;
    public AvailableActionsData AvailableActions;
    public bool CanPlayerInteract = true; //Define se o player pode interagir com esse objeto

    private Vector3 _oldPosition = Vector3.negativeInfinity;
    private Vector3 _playerOldPosition;

    private bool _pushPullAction = false;
    private float _realMass;
    public float _timeLeftToDeactivatePushPullAction = 0.5f;
    public Behaviour Halo;
    [HideInInspector] public bool _hasChain = false;

    private RigidbodyConstraints2D _defaultConstraints;
    private bool _physicsVisionIsReady = false;

    private Vector3 _initialPos;
    private float _initialGravity, _initialMass;

    private Vector3 _lastCheckpointPos;
    private Quaternion _lastCheckpointRotation;
    private float _lastCheckpointGravity, _lastCheckpointMass;

    [Header("On Reset")]
    public bool ShouldResetPosition = true;
    public bool ShouldResetRotation = true;
    public bool ShouldResetGravity = true;
    public bool ShouldResetMass = true;

    void OnValidate()
    {
        if (gameObject.CompareTag("Box"))
        {
            if (gameObject.GetComponent<FreezeBox>() == null)
            {
                gameObject.AddComponent<FreezeBox>();
            }
        }

        if (!_physicsVisionIsReady)
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
                Transform HaloInstance = transform.Find(HaloPrefab.name + "(Clone)");

                if (HaloInstance == null)
                {
                    HaloInstance = Instantiate(HaloPrefab, transform.position, Quaternion.identity, transform).transform;
                }

                Halo = HaloInstance.GetComponent<Behaviour>();
            }
        }

        Halo.enabled = false;
    }

	// Use this for initialization
	void Start () {
        PhysicsObjectList.Add(this); //Se adiciona à lista de PhysicsObjects

        //Pega a referência do halo
        _physicsVisionIsReady = PlayerInfo.PlayerInstance.PhysicsVisionIsReady;
        if (_physicsVisionIsReady)
        {
            OnValidate();
        }

        if(gameObject.GetComponent<HingeJoint2D>() != null)
        {
            _hasChain = true;
        }

        _defaultConstraints = physicsData.constraints;

        //Guarda as informações iniciais (para qnd precisar resetar o objeto)
        _initialPos = transform.position;
        _initialMass = physicsData.mass;
        _initialGravity = physicsData.gravityScale;
        _lastCheckpointPos = _initialPos;
        _lastCheckpointMass = _initialMass;
        _lastCheckpointGravity = _initialGravity;
        _lastCheckpointRotation = transform.rotation;
    }

    void Awake()
    {
        physicsData = gameObject.GetComponent<Rigidbody2D>();

        PhysicsObjectList = new List<PhysicsObject>(); //reseta lista de PhysicsObjects (para evitar problemas, por exemplo, em transição de scenes)
    }

    // Update is called once per frame
    void Update () {
        _playerOldPosition = PlayerInfo.PlayerInstance.transform.position;

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
		if (this.CompareTag ("Gangorra")) 
		{
			//faz o mesmo que foi explicado abaixo, mas para o painel específico da gangorra

			//Checa se o painel já está ativo
			if (ActionPanelGangorra.Instance.gameObject.activeInHierarchy)
			{
				return;
			}
			//Senão, ativa com o objeto selecionado
			if (CanPlayerInteract) {
				ActionPanelGangorra.Instance.OnPanelActivated(this);
			}
			return;
		}

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

    public void OnPhysicsVisionDeactivated()
    {
        if (Halo == null)
        {
            return;
        }
        Halo.enabled = false;
    }

    public void ResetObj() { 
        if (ShouldResetPosition) transform.position = _initialPos; //resetar posição
        if (ShouldResetRotation) transform.rotation = Quaternion.Euler(Vector3.zero); //resetar rotação

        if (ShouldResetMass) physicsData.mass = _initialMass; //resetar massa
        if (ShouldResetGravity) physicsData.gravityScale = _initialGravity; //resetar gravidade
    }

    public void NewCheckpoint()
    {
        _lastCheckpointPos = transform.position;
        _lastCheckpointRotation = transform.rotation;
        _lastCheckpointMass = physicsData.mass;
        _lastCheckpointGravity = physicsData.gravityScale;
    }

    public void LoadLastCheckpoint()
    {
        transform.position = _lastCheckpointPos;
        transform.rotation = _lastCheckpointRotation;
        physicsData.mass = _lastCheckpointMass;
        physicsData.gravityScale = _lastCheckpointGravity;       
    }
}
