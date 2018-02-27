
/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour {
	public PlayerInfo Player;
	public GameObject ActionMenu;
	public GameObject ActionMenuGangorra;
	[Tooltip("Tempo mínimo para identificar um toque longo")]
	public float HoldTime = 0.8f;
	public bool realJump;
	public float jumpCheckDistance;
	public float CameraTouchSpeed = 0.01f;
    public float MinDistanceToMoveCamera = 0.3f;

	public Vector3 vel;

	public Button Jump;
	public Button Action;

	private Vector3 _cameraOrigin;
	private Vector3 _mouseOrigin;

	private GameObject _directionBeforeJump;
	// private float _friction;

	private Rigidbody2D rb;
	private Animator _playerAnim;

	//mácara usada para ignorar o player
	private int _layerMask;

	public bool _isJumping;

	private string _lastBntSelected;


	private PlayerInfo _info;

    [Header("DEBUG")]
    public PhysicsObject ObjectToReset;

	// Use this for initialization
	void Start () {
		Input.multiTouchEnabled = true;

		_info = Player.GetComponent<PlayerInfo> ();

		#if UNITY_IOS || UNITY_ANDROID

		//Debug.Log(gameObject.transform.parent.Find("Canvas").Find("Jump").GetComponent<Button>().name + " AAAA");
//		Debug.Log(gameObject.transform.parent.name + " AAAA");
		Jump.onClick.AddListener (JumpFunction);
		//Action.onClick.AddListener (ActionFunction);

		#endif

		_cameraOrigin = Camera.main.transform.position; 
	}

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		_playerAnim = this.GetComponent<Animator>();

		int layerIndex = LayerMask.NameToLayer("Player");
		if(layerIndex == -1)
		{
			Debug.LogWarning("Player layer mask does not exists");
			layerIndex = 0;
		}

        //tenta achar action menu se não estiver setado
        if (ActionMenu == null)
        {
            ActionMenu = GameObject.Find("Canvas").transform.Find("ActionPanel").gameObject;

            if (ActionMenu == null)
            {
                Debug.LogError("ActionMenu não foi setado no PlayerInput");
            }
        }

		//mácara sóolide com player
		_layerMask = (1 << layerIndex);

		//mácara colide com tudo menos o player
		_layerMask = ~_layerMask;

        //corrige instância do ActionPanel se não estiver setada
        if (ActionPanel.Instance == null)
        {
            ActionPanel.Instance = ActionMenu.GetComponent<ActionPanel>();
        }
	}


	// Update is called once per frame
	void Update () {

		vel = Player.GetComponent<Rigidbody2D> ().velocity;

		if (_isJumping) {
			_playerAnim.SetBool ("onFloor", false);

		} else {
			_playerAnim.SetBool("onFloor", true);

		}

		// Verifica se estáodando o jogo no unity caso contráio serám algum mobile
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if(TutorialDialog.IsCanvasOn){
		return;
		}

		// Nã permite que o player clique no botã enquanto tiver no pause
		if (!GameManager.IsPaused && !ActionPanel.Instance.isActiveAndEnabled) {

			if (Input.GetKey(KeyCode.A))
			{
				if (realJump)
				{
					if (!_isJumping){

						Player.Move(true, MinDistanceToMoveCamera);
						_info.CheckInputFlip("A");
					}
				} else
				{
					Player.Move(true, MinDistanceToMoveCamera);
					_info.CheckInputFlip("A");
				}

			}  else if (Input.GetKey(KeyCode.D))
			{
				if (realJump)
				{
					if (!_isJumping) {
						Player.Move(false, MinDistanceToMoveCamera);
						_info.CheckInputFlip("D");
					}
				}  else
				{
					Player.Move(false, MinDistanceToMoveCamera);
					_info.CheckInputFlip("D");
				} 
			}

			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
			{
				if (!_isJumping) Player.Jump();
			}
		}

		if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.LoadLastCheckpoint();
		}

		if (Input.GetKey(KeyCode.E))
		{
			ActionButton();
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			GameManager.Instance.OnPause();
		}

		/*
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("VISÃO FÍSICA EXPERIMENTAL");
            PlayerInfo.PlayerInstance.ChangePhysicsVisionStatus();
        }
        */

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DebugResetObj();
        }

        if(Input.GetKeyDown(KeyCode.Comma)){
            GameManager.Instance.CreateNewCheckpoint();
        }

        if(Input.GetKeyDown(KeyCode.Period)){
            GameManager.Instance.LoadLastCheckpoint();
        }

		if (Input.GetMouseButtonDown(0))
		{
			_mouseOrigin = Input.mousePosition;
		}

		else if(Input.GetMouseButton(0))
		{
			Debug.Log("MoveCamera");
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin);
			Vector2 move = new Vector2(pos.x, pos.y);
			MoveCamera(move);
		}

		#endif



	}

	void FixedUpdate()
	{
		#if UNITY_IOS || UNITY_ANDROID
		if(TutorialDialog.IsCanvasOn){
			return;
		}
		CheckInput();
   		#endif
    }

	public void JumpFunction() {
		if (TutorialDialog.IsCanvasOn) {
			return;
		}

		if (!_isJumping) Player.Jump ();
	}

	public void ActionButton()
	{
		if (TutorialDialog.IsCanvasOn) {
			return;
		}

		IAction<float> action = Player.Actions.Find(x => x.GetActionName().Equals("Push/Pull"));
		if (action != null)
		{
			PhysicsObject target = Player.FindNearestPhysicsObject();
			action.SetTarget(target);
			if (target != null && !_isJumping)
			{
				action.OnActionUse(Mathf.Sign(target.transform.position.x - Player.transform.position.x)); //O ARGUMENTO NÃ AFETA MAIS! old: O argumento será ou -1, dependendo de se o player estántes ou depois do target.
			}
		}
	}

	public void CheckInput()
	{
		int touches = Input.touchCount;

		if (touches > 0)
		{
			for (int i = 0; i < touches; i++)
			{
				Touch touch = Input.GetTouch(i);

				// Verifica se o toque foi em algum item da UI (IsPointerOver pega apenas o toque no UI, 
				// mas precisa do outro código para não perder uma referencia)
				if (EventSystem.current.IsPointerOverGameObject (touch.fingerId) && IsPointerOverUIObject()) {
					Debug.Log ("UI is touched");
					UITouch (touch);
				} else {
					Debug.Log ("UI is not touched");
					ObjectsTouch (touch);
				}
			}
		}
	}
		
	private bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

		return results.Count > 0;
	}

	private void UITouch(Touch touch)
	{
		GameObject HUDbnt = EventSystem.current.currentSelectedGameObject;
		TouchPhase phase = touch.phase;

		if (HUDbnt != null) {
			_lastBntSelected = HUDbnt.name;

			// Nã permite que o player clique no botã enquanto tiver no pause
			if (!GameManager.IsPaused && !ActionPanel.Instance.isActiveAndEnabled) {

				if (HUDbnt.name == "Action") {
					if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
					{
						//if (touch.phase == TouchPhase.Stationary) {
						print("Action");
						ActionButton();	
						//}
					}
				}
			}

			if (HUDbnt.name == "Restart")
			{
				if (touch.phase == TouchPhase.Began)
				{
					GameManager.Instance.LoadLastCheckpoint();
				}
			}

			if (HUDbnt.name == "Menu")
			{
				// Por enquanto só pausa o jogo
				if (touch.phase == TouchPhase.Began)
				{
					GameManager.Instance.OnPause();
					print("Menu");
				}
			}
		}

		else {
			ScreenTouch ();
		}
	}

	private void ObjectsTouch(Touch touch)
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), -Vector2.up);
		TouchPhase phase = touch.phase;

		if (hit.collider != null) {

			if (hit.transform.name == "physicsObject") {
				Debug.Log (hit.transform.name);

				if (touch.phase == TouchPhase.Began) {

				} else if (touch.phase == TouchPhase.Stationary) {
					if (touch.deltaTime == HoldTime) {
						// Long tap
						Debug.Log ("Change Objects Properties");
					}

				} else if (touch.phase == TouchPhase.Moved) {
					Debug.Log ("Drag Object");
					hit.transform.position = touch.position;

				} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
					// Velocidade do drag do objeto
					// float touchSpeed = touch.deltaPosition.magnitude / touch.deltaTime;
					Debug.Log ("Throw Object");
				}

			} else if (hit.collider.name == "slider") {
				if (touch.phase == TouchPhase.Moved) {
					Debug.Log ("Move Slider");
					float y = touch.deltaPosition.y;
					hit.transform.position = touch.position;
				}

			} else if (hit.collider.name == "physicsProperty") {
				if (touch.phase == TouchPhase.Began) {
					if (hit.transform.name == "gravity") {
						Debug.Log ("Change gravity");
					} else if (hit.transform.name == "mass") {
						Debug.Log ("Change mass");
					} else if (hit.transform.name == "heat") {
						Debug.Log ("Change heat");
					}
				}
			} else {
				ScreenTouch ();
			}
		} 
	}

	public void ScreenTouch(){
		Touch _touch = Input.GetTouch (0);

		Debug.Log("Move Camera");
		CameraController.Instance.CameraScroll (_touch.deltaPosition * CameraTouchSpeed);
	}

	void OnTriggerEnter2D(Collider2D other){


		_isJumping = false;
	}

	void OnTriggerStay2D(Collider2D other){

		_isJumping = false;

	}

	void OnTriggerExit2D(Collider2D other){


		_isJumping = true;
	}

	// verifica se estáocando a gangorra e diminui o atrito
	private bool PlayerTouchingSeesaw()
	{

		BoxCollider2D playerCollider = Player.GetComponent<BoxCollider2D>();
		Transform seesaw;

		if (GameObject.FindWithTag("Gangorra") == null)
		{
			return false;
		}

		//seesaw = GameObject.Find("PhysicsObjects").transform.Find("Gangorra").transform.Find("Gangorra");
		seesaw = GameObject.FindWithTag("Gangorra").transform.Find("Gangorra");

		// Verifica somente a barra da gangorra
		Collider2D physicsSeesawCollider = seesaw.GetComponent<Collider2D>();


		if (playerCollider.IsTouching(physicsSeesawCollider))
		{
			return true;
		}

		return false;
	}

	public void MoveCamera(Vector2 offset)
	{
		if (ActionMenu.activeInHierarchy || (ActionMenuGangorra != null && ActionMenuGangorra.activeInHierarchy))
		{
			return;
		}

		PlayerInfo.PlayerInstance.MoveCamera(offset, MinDistanceToMoveCamera, true);
	}

    #region DEBUG
    private void DebugResetObj()
    {
        if (ObjectToReset != null)
        {
            ObjectToReset.ResetObj();
        }
    }
    #endregion

}