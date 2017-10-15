/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {
    public PlayerInfo Player;

    private float holdTime = 0.8f;          // tempo para ativar a ação do hold
    private float speed = 2.0f;

    //Chamar ações que os botão irão realizar

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

        // Verifica se está rodando o jogo no unity caso contrário será em algum mobile
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        // Controles do teclado

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        //Player.transform.position += move * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            Vector2 movement = Vector2.zero;
            movement.x = Player.transform.position.x - 1 * (Time.deltaTime / 0.5f);
            //movement.x = Mathf.Max(-1, Player.transform.position.x - 1 * (Time.deltaTime / 0.5f));

            Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
            rb.AddForce(movement * speed);

            //float moveHorizontal = Input.GetAxis("Horizontal");

            // Vector3 movement = new Vector3(moveHorizontal, 0.0f, Player.transform.position.y);

            //Rigidbody rb = Player.GetComponent<Rigidbody>();
            //rb.AddForce(movement * speed);
            //Player.transform.position = new Vector3(Player.transform.position.x + 5, Player.transform.position.y);
        } else if (Input.GetKey(KeyCode.D))
        {
            Vector2 movement = Vector2.zero;
            movement.x = Player.transform.position.x - 1 * (Time.deltaTime / 0.5f);
           // movement.x = Mathf.Max(-1, Player.transform.position.x - 1 * (Time.deltaTime / 0.5f));

            Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
            rb.AddForce(-movement * speed);
        } else if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.OnPause();
        }

        // Verifica se está jogando com iOS ou Android
        #elif UNITY_IOS || UNITY_ANDROID 

        int touches = Input.touchCount;

        if (touches > 0)
        {
            for (int i = 0; i < touches; i++)
            {
                Touch touch = Input.GetTouch(i);
                TouchPhase phase = touch.phase;

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), -Vector2.up);

                print("Touch index " + touch.fingerId + " detected at position " + touch.position);

                if (touch.phase == TouchPhase.Began)
                {

                    if (hit.collider != null)
                    {

                        if (hit.collider.tag == "rightBnt")
                        {
                            Debug.Log("right button pressed");
                        } else if (hit.collider.tag == "leftBnt") {
                            Debug.Log("Left Button pressed");
                        } else if (hit.collider.tag == "jumpBnt") {
                            Debug.Log("JUMP!");
                        } else if (hit.collider.tag == "interactionBnt") {
                            Debug.Log("Change Obj Physics");
                        } else if (hit.collider.tag == "box") {
                            Debug.Log("Tap on box");
                        } else {
                            Debug.Log("Not an valid touch");
                        }
                    }
                } else if (touch.phase == TouchPhase.Stationary)
                {

                    if (touch.deltaTime == holdTime && hit.collider.name == "box")
                    {
                        // Long tap
                        Debug.Log("Change Objects Properties");
                    }

                } else if (touch.phase == TouchPhase.Moved)
                {

                    if (hit.collider.name == "box")
                    {
                        Debug.Log("Drag Object");
                        hit.transform.position = touch.position;
                    } else if (hit.collider.name == "slider")
                    {
                        Debug.Log("Move Slider");
                        float y = touch.deltaPosition.y;
                        hit.transform.position = touch.position;
                    }


                } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    float touchSpeed = touch.deltaPosition.magnitude / touch.deltaTime;         // Velocidade do drag do objeto
                    Debug.Log("Throw Object");

                }
            }


        }

 #endif

    }

    void FixedUpdate()
    {

    }

    public void CheckInput()
    {

    }
}
