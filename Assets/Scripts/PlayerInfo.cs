/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    static public GameObject PlayerGameObject;
    public float Life;
    public List<IAction<float>> Actions;
    [Tooltip("Velocidade com que o player se move")]
    public float PaceSpeed = 2.0f;
    [Tooltip("Velocidade com que o player realiza o pulo")]
    public float JumpSpeed = 6.0f;

    private bool receiveDamage;
    private float damageNumber;
    private Vector2 movement;

    void Awake()
    {
        //Seta a referência do gameObject do player
        PlayerGameObject = gameObject;

        Actions.Add(new PushPullAction());
    }

    // Use this for initialization
    void Start () {

        damageNumber = 0.0f;

    }

    // Update is called once per frame
    void Update () {

        if (receiveDamage)
        {

        }


        if (Life <= 0)
        {
            OnDeath();
        }

    }

    /// <summary>
    /// Define o que ocorre quando o player morre
    /// </summary>
    void OnDeath()
    {
        Destroy(this);
    }

    /// <summary>
    /// Diminui a vida do player conforme o dano recebido
    /// </summary>
    void LoseLife(float dmg)
    {
        Life -= dmg;
    }

    // Movimentação
    public void Move(bool walkLeft)
    {
        if (movement.x < 2)
        {
            // movement.x = Mathf.Max(-1, this.transform.position.x - 1 * (Time.deltaTime / 0.5f));
            movement.x = this.transform.position.x - 1 * (Time.deltaTime / 0.5f);
        }

        print(movement);
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();

        if (walkLeft)
        {
            rb.AddForce(Vector2.left * PaceSpeed);
        } else
        {
            rb.AddForce(Vector2.right * PaceSpeed);
        }
    }

    public void Jump()
    {
        print("JUMP" + Vector2.up * JumpSpeed);
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * JumpSpeed);
    }

}
