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
    [Tooltip("Antigo JumpSpeed. Influencia em velocidade e altura com que o player realiza o pulo")]
    public float JumpForce = 60.0f;
    [Tooltip("Distância máxima para o player poder usar ações como Push/Pull em um objeto")]
    public float MaxDistanceToNearbyObject = 1.5f;
    public float MaxVelocity = 5.0f;
	public AudioClip jump;
    public bool facingRight = true;
    public FixedJoint2D PushPullJoint;

    private bool _receiveDamage;
    private float _damageNumber;
    private Rigidbody2D _rb;
    private Animator _playerAnim;

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
        _playerAnim = this.GetComponent<Animator>();

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

    //Checa se o player está olhando para a esquerda ou direita
    void FixedUpdate()
    {
       
    }

	public void CheckInputFlip(string btn, float velocity) {

		_playerAnim.SetInteger("velocity", (int)(_rb.velocity.x));

		if ((btn == "D" || btn == "RightDir") && !facingRight)
			Flip();
		else if ((btn == "A" || btn == "LeftDir") && facingRight)
			Flip();

	}

    //Flip no player
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
    public void Move(bool walkLeft, float minDistanceToMoveCamera)
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();

        //Reseta a posição da câmera
        Vector3 cameraDistToPlayer = transform.position - Camera.main.transform.position;
        MoveCamera(new Vector2(cameraDistToPlayer.x, cameraDistToPlayer.y), minDistanceToMoveCamera);

        if (walkLeft)
        {
            _playerAnim.SetBool("mirror", true);
            if (rb.velocity.x >= -MaxVelocity)
            {
                Vector2 movement = Vector2.left * PaceSpeed * 8.47f;
                rb.AddForce(movement);
            }

        } else
        {
            _playerAnim.SetBool("mirror", false);
            if (rb.velocity.x <= MaxVelocity)
            {
                Vector2 movement = Vector2.right * PaceSpeed * 8.47f;
                rb.AddForce(movement);
            }
        }




    }

    public void MoveCamera(Vector2 offset, float minDistanceToMoveCamera, bool forceCamera = false)
    {
        if (!forceCamera && (minDistanceToMoveCamera > offset.magnitude))
        {
            return;
        }

        CameraController camera = GetComponent<CameraController>();
        if (camera == null)
        {
            Debug.LogError("Player está sem CameraController");
            return;
        }
        camera.Move(new Vector3(offset.x, offset.y, 0));
    }

    public void Jump()
    {
        _rb.AddForce(Vector2.up * JumpForce);
		AudioSource.PlayClipAtPoint (jump, transform.position);
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
