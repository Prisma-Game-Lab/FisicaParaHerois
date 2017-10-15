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
    [Tooltip("Tempo mínimo para identificar um toque longo")]
    public float HoldTime = 0.8f;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

        // Verifica se está rodando o jogo no unity caso contrário será em algum mobile
#if UNITY_STANDALONE || UNITY_WEBPLAYER

        if (Input.GetKey(KeyCode.A))
        {
            Player.Move(true);
            
        } else if (Input.GetKey(KeyCode.D))
        {
            Player.Move(false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Player.Jump();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.ReloadScene();
           
        } else if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.OnPause();
        }

       
        // Verifica se está jogando com iOS ou Android
#elif UNITY_IOS || UNITY_ANDROID

        CheckInput();

 #endif

    }

    void FixedUpdate()
    {

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

        if(HUDbnt.name == "LeftDir")
        {
            Player.Move(true);

        } else if (HUDbnt.name == "RightDir")
        {
            Player.Move(false);
        }

        if (HUDbnt.name == "Jump")
        {
            if (touch.phase == TouchPhase.Began)
            {
                BoxCollider2D playerCollider = Player.GetComponent<BoxCollider2D>();
                BoxCollider2D floorCollider = GameObject.Find("Floor").GetComponent<BoxCollider2D>();

                if (playerCollider.IsTouching(floorCollider))
                {
                    Player.Jump();
                }
            }
        } else if (HUDbnt.name == "Action")
        {
            if (touch.phase == TouchPhase.Began)
            {
                print("Action");
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                print("Change Objects Properties");
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
            if(hit.collider.name == "physicsObject")
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
                    float touchSpeed = touch.deltaPosition.magnitude / touch.deltaTime;
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
        }
    }
    
}
