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
    public Animator _playerAnim;
    public FixedJoint2D PushPullJoint;
	[Tooltip("Segundos desde que o player aperta o botão até impedir o movimento do player quando ele está em cima de uma gangorra")]public float MoveDuration = 0.5f;
	[HideInInspector] public bool IsConstrained = false;
	[HideInInspector] public bool IsJumping = false;
	public float JumpDuration = 1.5f;
	private float _timeSinceJumpStarted = 0.0f;

    private bool _receiveDamage;
    private float _damageNumber;
    private Rigidbody2D _rb;
    private bool _physicsVisionActivated = false;
    private PhysicsObject[] _physicsObjects;
    private CameraController _cameraController;
	private float _secondsSinceLastMove = 0;

    [Header("DEBUG")]
	public bool PhysicsVisionIsReady = false;

	[HideInInspector] public bool MayMoveCamera = true;

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

        _physicsObjects = FindObjectsOfType<PhysicsObject>();

		CustomizePlayer.SetPlayer(gameObject);
    }

    // Use this for initialization
    void Start () {

        _damageNumber = 0.0f;
        _rb = this.GetComponent<Rigidbody2D>();
        _playerAnim = this.GetComponent<Animator>();
        _cameraController = PlayerInstance.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update () {
		_secondsSinceLastMove += Time.deltaTime;

        if (_receiveDamage)
        {

        }


        if (Life <= 0)
        {
            OnDeath();
        }

		if (IsJumping) {
			_timeSinceJumpStarted += Time.deltaTime;
			if (_timeSinceJumpStarted >= JumpDuration) {
				IsJumping = false;
				_timeSinceJumpStarted = 0;
			}
		}

    }

    //Checa se o player está olhando para a esquerda ou direita
    void FixedUpdate()
    {
		_playerAnim.SetInteger("velocity", (int)(_rb.velocity.x));
        //if (_rb.velocity.x < 0 && )
    }

	public void CheckInputFlip(string btn) {

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
		_secondsSinceLastMove = 0;

        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();

        if (walkLeft)
        {
            if (rb.velocity.x >= -MaxVelocity)
            {
				Vector2 vel = new Vector2 (-1*PaceSpeed, _rb.velocity.y);
				rb.velocity = vel; 
            }

        } else
        {      
            if (rb.velocity.x <= MaxVelocity)
            {
				Vector2 vel = new Vector2 (PaceSpeed, _rb.velocity.y);

				rb.velocity = vel; 
            }
        }
    }

	public void Jump()
    {
		_secondsSinceLastMove = 0;
		IsJumping = true;
        _playerAnim.SetBool("onFloor", false);

        if (PushPullJoint.connectedBody != null)
        {
            //não pode pular enquanto segura uma caixa (argumento do Nelson: ela tem no minimo o peso do Player)
            return;
        }
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
            
            if(curObj != null && curObj != playerPhysicsObject && curObj.tag != "Gangorra")
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

    public void ChangePhysicsVisionStatus()
    {
        _physicsVisionActivated = !_physicsVisionActivated;

        switch (_physicsVisionActivated)
        {
            case true:
                foreach (PhysicsObject p in _physicsObjects)
                {
                    p.OnPhysicsVisionActivated();
                }
                _cameraController.OnPhysicsVisionActivated();
                break;
            default:
                foreach (PhysicsObject p in _physicsObjects)
                {
                    p.OnPhysicsVisionDeactivated();
                }
                _cameraController.OnPhysicsVisionDeactivated();
                break;
        }
    }

    // Métodos do Editor
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MaxDistanceToNearbyObject);
    }

}
