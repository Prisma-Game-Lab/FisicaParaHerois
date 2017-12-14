/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    static public PlayerInfo PlayerInstance;
    public float Life;
    public List<IAction<float>> Actions;
    [Tooltip("Velocidade com que o player se move")]
    public float PaceSpeed = 20.0f;
    [Tooltip("Velocidade com que o player realiza o pulo")]
    public float JumpSpeed = 60.0f;
    [Tooltip("Distância máxima para o player poder usar ações como Push/Pull em um objeto")]
    public float MaxDistanceToNearbyObject = 1.5f;
    public float MaxVelocity = 5.0f;

    private bool _receiveDamage;
    private float _damageNumber;
    private Vector2 _movement;
    private Rigidbody2D _rb;

    void Awake()
    {
        //Seta a referência do player
        PlayerInstance = this;
        Actions = new List<IAction<float>>();

        IAction<float>[] playerActions = gameObject.GetComponents<IAction<float>>();

        foreach(IAction<float> action in playerActions)
        {
            Actions.Add(action);
        }
    }

    // Use this for initialization
    void Start () {

        _damageNumber = 0.0f;
        _rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {

        if (_receiveDamage)
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
        Debug.Log("Player morreu");
        GameManager.IsEnded = true;
        Time.timeScale = 0;
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
        if (_movement.x > -2)
        {
            _movement.x = this.transform.position.x - 1 * (Time.deltaTime / 10f);
        }

        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();

        if (walkLeft)
        {
            print(rb.velocity);
            if (rb.velocity.x >= -MaxVelocity)
            {
                rb.AddForce(_movement * PaceSpeed);
            }

        } else
        {
            if (rb.velocity.x <= MaxVelocity)
            {
                rb.AddForce(-_movement * PaceSpeed);
            }
        }
    }

    public void Jump()
    {
        _rb.AddForce(Vector2.up * JumpSpeed);
    }

    // Métodos Auxiliares
    public PhysicsObject FindNearestPhysicsObject()
    {
        Collider2D[] objectsFound = Physics2D.OverlapCircleAll(transform.position, MaxDistanceToNearbyObject);

        PhysicsObject nearestPhysicsObj = null;
        float distanceToNearestPhysicsObj = Mathf.Infinity;
        PhysicsObject playerPhysicsObject = GetComponent<PhysicsObject>(); //usado para não retornar o próprio player

        //Busca o PhysicsObject mais próximo
        foreach(Collider2D objFound in objectsFound)
        {
            PhysicsObject curObj = objFound.gameObject.GetComponent<PhysicsObject>();
            
            if(curObj != null && curObj != playerPhysicsObject)
            {
                float distanceToCurObj = Vector3.Distance(objFound.transform.position, transform.position);

                if(distanceToCurObj < distanceToNearestPhysicsObj)
                {
                    distanceToNearestPhysicsObj = distanceToCurObj;
                    nearestPhysicsObj = curObj;
                }
            }
        }

        return nearestPhysicsObj;
    }

    // Métodos do Editor
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MaxDistanceToNearbyObject);
    }

}
