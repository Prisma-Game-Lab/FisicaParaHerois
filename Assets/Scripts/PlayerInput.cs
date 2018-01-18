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
    [Tooltip("Tempo mínimo para identificar um toque longo")]
    public float HoldTime = 0.8f;
    public bool realJump;
    public float jumpCheckDistance;
    public float CameraTouchSpeed = 0.01f;

    private Vector3 _cameraOrigin;
    private Vector3 _mouseOrigin;

    private GameObject _directionBeforeJump;
   // private float _friction;

    private Rigidbody2D rb;

    //máscara usada para ignorar o player
    private int _layerMask;

    // Use this for initialization
    void Start () {
        Input.multiTouchEnabled = true;

        //_friction = Player.GetComponent<BoxCollider2D>().sharedMaterial.friction;

        _cameraOrigin = Camera.main.transform.position; 
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        int layerIndex = LayerMask.NameToLayer("Player");
        if(layerIndex == -1)
        {
            Debug.LogWarning("Player layer mask does not exists");
            layerIndex = 0;
        }

        //máscara só colide com player
        _layerMask = (1 << layerIndex);

        //máscara colide com tudo menos o player
        _layerMask = ~_layerMask;
    }


    // Update is called once per frame
    void Update () {

        // Fazer algo pra melhorar o movimento emcima da gangorra aqui!
        if (PlayerTouchingSeesaw())
        {
           // Player.GetComponent<BoxCollider2D>().sharedMaterial.friction = 0.1f;
            //print(Player.GetComponent<BoxCollider2D>().sharedMaterial.friction);
        } else
        {
            //Player.GetComponent<BoxCollider2D>().sharedMaterial.friction = _friction;
        }

        // Verifica se está rodando o jogo no unity caso contrário será em algum mobile
#if UNITY_STANDALONE || UNITY_WEBPLAYER

		// Não permite que o player clique no botão enquanto tiver no pause
		if (!GameManager.IsPaused && !ActionPanel.Instance.isActiveAndEnabled) {
			
			if (Input.GetKey(KeyCode.A))
			{
				if (realJump)
				{
					if (!IsJumping()) Player.Move(true);
				} else
				{
					Player.Move(true);
				}

			} else if (Input.GetKey(KeyCode.D))
			{
				if (realJump)
				{
					if (!IsJumping()) Player.Move(false);
				} else
				{
					Player.Move(false);
				}

			}

			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
			{
				if(!IsJumping()) Player.Jump();
			}
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			GameManager.ReloadScene();

		}

        if (Input.GetKey(KeyCode.E))
        {
            ActionButton();
        }

		if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.OnPause();
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
       
        // Verifica se está jogando com iOS ou Android
#elif UNITY_IOS || UNITY_ANDROID

        CheckInput();
        //CheckJump();

 #endif

    }

    void FixedUpdate()
    {

    }

    public void ActionButton()
    {
        IAction<float> action = Player.Actions.Find(x => x.GetActionName().Equals("Push/Pull"));
        if (action != null)
        {
            PhysicsObject target = Player.FindNearestPhysicsObject();
            action.SetTarget(target);
            if (target != null)
            {
                PlayerInfo.PlayerInstance.ForceToApplyOnObject = Mathf.Sign(target.transform.position.x - Player.transform.position.x) * 100;
                action.OnActionUse(Mathf.Sign(target.transform.position.x - Player.transform.position.x)); //O ARGUMENTO NÃO AFETA MAIS! old: O argumento será 1 ou -1, dependendo de se o player está antes ou depois do target.
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

                    // Verifica se o toque foi em algum item da UI
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Debug.Log("UI is touched");
                    UITouch(touch);
                }
                else
                {
                    Debug.Log("UI is not touched");
                    ObjectsTouch(touch);
                }
            }

        }
    }

    private void UITouch(Touch touch)
    {
        GameObject HUDbnt = EventSystem.current.currentSelectedGameObject;
        //GameObject HUDbntLast = EventSystem.current.lastSelectedGameObject;
        TouchPhase phase = touch.phase;

		// Não permite que o player clique no botão enquanto tiver no pause
		if (!GameManager.IsPaused && !ActionPanel.Instance.isActiveAndEnabled) {
			
			if (HUDbnt.name == "LeftDir")
			{
				if (realJump)
				{
					if (!IsJumping()) Player.Move(true);
				}
				else
				{
					Player.Move(true);
				}

			} else if (HUDbnt.name == "RightDir")
			{
				if (realJump)
				{
					if (!IsJumping()) Player.Move(false);
				}
				else
				{
					Player.Move(false);
				}
			}

			if (HUDbnt.name == "Jump")
			{
				if (touch.phase == TouchPhase.Began)
				{
					if(!IsJumping()) Player.Jump();
				}
			} else if (HUDbnt.name == "Action")
			{
				if (touch.phase == TouchPhase.Began)
				{
					print("Action");

                    ActionButton();
				}
				else if (touch.phase == TouchPhase.Stationary)
				{
					print("Change Objects Properties");
				}
			}

		}

        if (HUDbnt.name == "Restart")
        {
            if (touch.phase == TouchPhase.Began)
            {
                GameManager.ReloadScene();
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

    private void ObjectsTouch(Touch touch)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), -Vector2.up);
        TouchPhase phase = touch.phase;

        if (hit.collider != null)
        {
            print(hit.collider.name);

            if (hit.transform.name == "physicsObject")
            {
                Debug.Log(hit.transform.name);

                if(touch.phase == TouchPhase.Began)
                {

                } else if (touch.phase == TouchPhase.Stationary)
                {
                    if (touch.deltaTime == HoldTime)
                    {
                        // Long tap
                        Debug.Log("Change Objects Properties");
                    }

                } else if (touch.phase == TouchPhase.Moved)
                {
                    Debug.Log("Drag Object");
                    hit.transform.position = touch.position;

                } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    // Velocidade do drag do objeto
                   // float touchSpeed = touch.deltaPosition.magnitude / touch.deltaTime;
                    Debug.Log("Throw Object");
                }

            } else if (hit.collider.name == "slider")
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    Debug.Log("Move Slider");
                    float y = touch.deltaPosition.y;
                    hit.transform.position = touch.position;
                }

            } else if (hit.collider.name == "physicsProperty")
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (hit.transform.name == "gravity")
                    {
                        Debug.Log("Change gravity");
                    } else if (hit.transform.name == "mass")
                    {
                        Debug.Log("Change mass");
                    }
                    else if (hit.transform.name == "heat")
                    {
                        Debug.Log("Change heat");
                    }
                }
            }
            else
            {
               Debug.Log("Move Camera");
               MoveCamera(new Vector2(-touch.deltaPosition.x * CameraTouchSpeed, -touch.deltaPosition.y * CameraTouchSpeed));
            }
        }
    }

    private bool IsJumping()
    {
        //usa raycast pra ver se há algum objeto abaixo do player, até certa distância
        //1.0f temp para distancia mínima
        Vector2 posMin = new Vector2(transform.position.x - Player.GetComponent<BoxCollider2D>().size.x/2 - 0.1f, transform.position.y);
        Vector2 posMid = new Vector2(transform.position.x, transform.position.y);
        Vector2 posMax = new Vector2(transform.position.x + Player.GetComponent<BoxCollider2D>().size.x/2 + 0.1f, transform.position.y);

		float alturaPlayer = Player.GetComponent<BoxCollider2D> ().bounds.size.y;
		//jumpCheckDistance
		RaycastHit2D hitMin = Physics2D.Raycast(posMin, Vector2.down, alturaPlayer/2 + 0.1f, _layerMask);
		RaycastHit2D hitMid = Physics2D.Raycast(posMid, Vector2.down, alturaPlayer/2 + 0.1f, _layerMask);
		RaycastHit2D hitMax = Physics2D.Raycast(posMax, Vector2.down, alturaPlayer/2 + 0.1f, _layerMask);

        if (hitMin.collider == null && hitMid.collider == null && hitMax.collider == null)
        {
            return true;
        }
        else return false;
    }

    // verifica se está tocando a gangorra e diminui o atrito
    private bool PlayerTouchingSeesaw()
    {

        BoxCollider2D playerCollider = Player.GetComponent<BoxCollider2D>();
        Transform seesaw;

        if (GameObject.Find("Gangorra") == null)
        {
            return false;
        }

        seesaw = GameObject.Find("PhysicsObjects").transform.Find("Gangorra").transform.Find("Gangorra");


        // Verifica somente a barra da gangorra
        Collider2D physicsSeesawCollider = seesaw.GetComponent<Collider2D>();


        if (playerCollider.IsTouching(physicsSeesawCollider))
        {
             return true;
        }

        return false;
    }

    private void MoveCamera(Vector2 translation)
    {
        if (ActionMenu.activeInHierarchy)
        {
            return;
        }

        Camera.main.transform.Translate(translation.x, translation.y, 0);
    }


    //    private void CheckJump()
    //    {
    //        BoxCollider2D playerCollider = Player.GetComponent<BoxCollider2D>();
    //        Transform scene = GameObject.Find("SceneObjects").transform;
    //        Transform objects = GameObject.Find("PhysicsObjects").transform;
    //        bool touchingScene = false;
    //        bool touchingObject = false;

    //        for (int i = 0; i < scene.childCount; i += 1)
    //        {
    //            Collider2D sceneObjectCollider = scene.GetChild(i).GetComponent<Collider2D>();

    //            if (playerCollider.IsTouching(sceneObjectCollider))
    //            {
    //                touchingScene = true;
    //            }
    //        }

    //        for (int i = 0; i < objects.childCount; i += 1)
    //        {
    //            Collider2D physicsObjectCollider = objects.GetChild(i).GetComponent<Collider2D>();

    //            if (playerCollider.IsTouching(physicsObjectCollider))
    //            {
    //                touchingObject = true;
    //            }
    //        }

    //        if ((touchingObject || touchingScene))
    //        {
    //            _isJumping = false;
    //            // verificar aqui se continua a andar ou não 

    //        } else
    //        {
    //            _isJumping = true;
    //        }
    //    }

}
