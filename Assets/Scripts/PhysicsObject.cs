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

    private FreezeBox _freezeBox;

    private Vector3 _initialPos;
    private float _initialGravity, _initialMass;
    private Vector2 _initialVelocity;

    private Vector3 _lastCheckpointPos;
    private Quaternion _lastCheckpointRotation;
    private float _lastCheckpointGravity, _lastCheckpointMass;
    private Vector2 _lastCheckpointVelocity;

	private Vector3 _boxPositionBeforeCollision = Vector3.zero;
	private float _removePlayerFromWallCooldown = 0;

	private Gangorra _seesaw;
	private Vector2 _seesawInitalAnchor;
	private Vector3 _seesawInitialPosition;
	private Quaternion _seesawInitialRotation;
	private Vector2 _seesawLastCheckpointAnchor;
	private Vector3 _seesawLastCheckpointPosition;
	private Quaternion _seesawLastCheckpointRotation;

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

        if (tag == "Box")
        {
            _defaultConstraints = RigidbodyConstraints2D.None;
        }

        //Guarda as informações iniciais (para qnd precisar resetar o objeto)
        _initialPos = transform.position;
        _initialMass = physicsData.mass;
        _initialVelocity = physicsData.velocity;
        _initialGravity = physicsData.gravityScale;
        _lastCheckpointPos = _initialPos;
        _lastCheckpointMass = _initialMass;
        _lastCheckpointVelocity = _initialVelocity;
        _lastCheckpointGravity = _initialGravity;
        _lastCheckpointRotation = transform.rotation;

		//Guarda informações da gangorra
		if (tag == "Gangorra") {
			_seesaw = GetComponent<Gangorra>();
			_seesawInitalAnchor = _seesaw.Joint.anchor;
			_seesawInitialPosition = _seesaw.base_gangorra.transform.position;
			_seesawInitialRotation = _seesaw.base_gangorra.transform.rotation;
			_seesawLastCheckpointAnchor = _seesaw.Joint.anchor;
			_seesawLastCheckpointPosition = _seesaw.base_gangorra.transform.position;
			_seesawLastCheckpointRotation = _seesaw.base_gangorra.transform.rotation;
		}
    }

    void Awake()
    {
        physicsData = gameObject.GetComponent<Rigidbody2D>();

        PhysicsObjectList = new List<PhysicsObject>(); //reseta lista de PhysicsObjects (para evitar problemas, por exemplo, em transição de scenes)

        if (tag == "Box")
        {
            _freezeBox = GetComponent<FreezeBox>();
        }
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

		if(_removePlayerFromWallCooldown > 0){
			_removePlayerFromWallCooldown -= Time.deltaTime;
		}
    }

    void OnMouseDown()
    {
		//Checa se o painel já está ativo
		if (ActionPanel.Instance.gameObject.activeInHierarchy || (ActionPanelGangorra.Instance != null && ActionPanelGangorra.Instance.gameObject.activeInHierarchy))
		{
			return;
		}

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

        //Senão, ativa com o objeto selecionado
		if (CanPlayerInteract) {
			ActionPanel.Instance.OnPanelActivated(this);
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Se colisão for com gangorra, retira restrição de movimentação da caixa
		if ((collision.collider.transform.parent != null && collision.collider.transform.parent.tag == "Gangorra" && collision.otherCollider.tag == "Box") ||
			(collision.otherCollider.transform.parent != null && collision.otherCollider.transform.parent.tag == "Gangorra" && collision.collider.tag == "Box"))
        {
			if (tag == "Box" && !(IsCollisionFromAbove(collision) || IsCollisionFromBelow(collision)))
            {
                physicsData.constraints = _defaultConstraints;
            }
        }

        //Se player não estiver envolvido na colisão, não faça nada
		if (((collision.collider.gameObject != PlayerInfo.PlayerInstance.gameObject) &&  /*player não está envolvido na colisão*/
            (collision.otherCollider.gameObject != PlayerInfo.PlayerInstance.gameObject)) || /*player não está envolvido na colisão*/
            (this.gameObject == PlayerInfo.PlayerInstance.gameObject) /*é o player*/) {

			//Colisão entre player e wall
			if ((this.gameObject == PlayerInfo.PlayerInstance.gameObject) &&
			   ((collision.collider.name == "Floor_Collider") ||
			   (collision.otherCollider.name == "Floor_Collider")) && _removePlayerFromWallCooldown <= 0) {
				//checa direção
				foreach (ContactPoint2D pt in collision.contacts) {
					if(pt.collider.name != "Floor_Collider" &&
						pt.otherCollider.name != "Floor_Collider")
					{
						continue;
					}

					if (pt.point.x > transform.position.x + 2 || pt.point.x < transform.position.x - 2) {
						transform.Translate (new Vector3 (0, -1, 0));
						_removePlayerFromWallCooldown = 2f;
					}
				}

			}

			return;
		} else {
			//Physics2D.GetIgnoreCollision(collision.collider.gameObject, this.GetComponent<Collider2D>(),true)
			//Debug.Log ("TOCO NO PLAYER");
			if (tag == "Box") {
				if ((transform.position - _boxPositionBeforeCollision).magnitude >= 0.2) {
					_boxPositionBeforeCollision = transform.position;
				}
			}
		}

        if (!_pushPullAction)
        {
            //Checa se a colisão é por cima
            if (IsCollisionFromAbove(collision))
            {
                return;
            }

            //Congela posição se colisão não for por cima
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
            //Checa se a colisão é por cima
			if (IsCollisionFromAbove(collision) || IsCollisionFromBelow(collision))
			{
                physicsData.constraints = RigidbodyConstraints2D.FreezePositionX;
                return;
            }

            physicsData.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        else
		{
			_boxPositionBeforeCollision = transform.position;
            physicsData.constraints = _defaultConstraints;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        physicsData.constraints = _defaultConstraints;

		if (((collision.collider.gameObject != PlayerInfo.PlayerInstance.gameObject) &&  /*player não está envolvido na colisão*/
			(collision.otherCollider.gameObject != PlayerInfo.PlayerInstance.gameObject)) || /*player não está envolvido na colisão*/
			(this.gameObject == PlayerInfo.PlayerInstance.gameObject) /*é o player*/) {
			return;
		} else {
			if (tag == "Box" && !_pushPullAction && !(IsCollisionFromAbove(collision) || IsCollisionFromBelow(collision))) {
				transform.position = new Vector3(_boxPositionBeforeCollision.x, transform.position.y, transform.position.z);
			}
		}
    }

    bool IsCollisionFromAbove(Collision2D collision)
    {
		//* CONTACT POINT
        //Checa direção da colisão
        foreach (ContactPoint2D pt in collision.contacts)
        {
            //Checa se player está envolvido na colisão
            if (pt.collider.gameObject != PlayerInfo.PlayerInstance.gameObject &&
                pt.otherCollider.gameObject != PlayerInfo.PlayerInstance.gameObject)
            {
                continue;
            }

            //Checa se player está acima da caixa
            if (_freezeBox != null && pt.point.y >= _freezeBox.Collider.bounds.max.y)
            {
                return true;
            }
        }
        return false;
		//*/

		/* RAYCAST
		Debug.Log ("_freezeBox nula: " + (_freezeBox == null));
		int layerMask = LayerMask.GetMask("Player"); // Does the ray intersect any objects which are in the player layer.
		RaycastHit2D ray = Physics2D.Raycast(_freezeBox.Collider.offset, Vector2.up, Mathf.Infinity, layerMask);
		Debug.Log (ray.collider == null ? "null" : ray.collider.name);
		return ray.collider != null;
		*/
    }

	bool IsCollisionFromBelow(Collision2D collision)
	{
		//* CONTACT POINT
		//Checa direção da colisão
		foreach (ContactPoint2D pt in collision.contacts)
		{
			//Checa se player está envolvido na colisão
			if (pt.collider.gameObject != PlayerInfo.PlayerInstance.gameObject &&
				pt.otherCollider.gameObject != PlayerInfo.PlayerInstance.gameObject)
			{
				continue;
			}

			//Checa se player está acima da caixa
			if (_freezeBox != null && pt.point.y <= _freezeBox.Collider.bounds.min.y)
			{
				return true;
			}
		}
		return false;
		//*/

		/* RAYCAST
		Debug.Log ("_freezeBox nula: " + (_freezeBox == null));
		int layerMask = LayerMask.GetMask("Player"); // Does the ray intersect any objects which are in the player layer.
		RaycastHit2D ray = Physics2D.Raycast(_freezeBox.Collider.offset, Vector2.down, Mathf.Infinity, layerMask);
		Debug.Log (ray.collider == null ? "null" : ray.collider.name);
		return ray.collider != null;
		*/
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
        if (GameManager.Instance.ShouldResetPosition) transform.position = _initialPos; //resetar posição
        if (GameManager.Instance.ShouldResetRotation) transform.rotation = Quaternion.Euler(Vector3.zero); //resetar rotação
        if (GameManager.Instance.ShouldResetVelocity) physicsData.velocity = _initialVelocity; //resetar velocity
        if (GameManager.Instance.ShouldResetMass) physicsData.mass = _initialMass; //resetar massa
        if (GameManager.Instance.ShouldResetGravity) physicsData.gravityScale = _initialGravity; //resetar gravidade

		//resetar gangorra
		if (tag == "Gangorra" && GameManager.Instance.ShouldResetSeesaw) {
			_seesaw.Joint.anchor = _seesawInitalAnchor;
			_seesaw.base_gangorra.transform.position = _seesawInitialPosition;
			_seesaw.base_gangorra.transform.rotation = _seesawInitialRotation;
		}
    }

    public void NewCheckpoint()
    {
        _lastCheckpointPos = transform.position;
        _lastCheckpointRotation = transform.rotation;
        _lastCheckpointVelocity = physicsData.velocity;
        _lastCheckpointMass = physicsData.mass;
        _lastCheckpointGravity = physicsData.gravityScale;

		if (tag == "Gangorra") {
			_seesawLastCheckpointAnchor = _seesaw.Joint.anchor;
			_seesawLastCheckpointPosition = _seesaw.base_gangorra.transform.position;
			_seesawLastCheckpointRotation = _seesaw.base_gangorra.transform.rotation;
		}
    }

    public void LoadLastCheckpoint()
    {
        transform.position = _lastCheckpointPos;
        transform.rotation = _lastCheckpointRotation;
        physicsData.velocity = _lastCheckpointVelocity;
        physicsData.mass = _lastCheckpointMass;
        physicsData.gravityScale = _lastCheckpointGravity;
		if (tag == "Gangorra") {
			_seesaw.Joint.anchor = _seesawLastCheckpointAnchor;
			_seesaw.base_gangorra.transform.position = _seesawLastCheckpointPosition;
			_seesaw.base_gangorra.transform.rotation = _seesawLastCheckpointRotation;
		}
    }
}
