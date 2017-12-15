using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

    [Header("Jump Atributes")]
    [Tooltip("Force Added at the start of a jump")]
    public float JumpForce;
    [Tooltip("Time elapsed for gravity to come back to normal")]
    public float GravityEffectTime;
    [Tooltip("Added gravity to end partial jump")]
    public float Add_Gravity_LowJump;
    [Tooltip("Added gravity to end a full jump")]
    public float Add_Gravity_Fall;

    //distância usada pelo raycast pra checar se o player está pulando
    public float jumpCheckDistance;

    //máscara usada para ignorar o player
    private int _layerMask;

    private Rigidbody2D _playerRB;
    // Use this for initialization
	void Awake () {
        _playerRB = Object.FindObjectOfType<PlayerInput>().gameObject.GetComponent<Rigidbody2D>();

        //seta máscara que não colide com a Layer player
        int layerIndex = LayerMask.NameToLayer("Player");
        if (layerIndex == -1)
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
	void FixedUpdate () {
		
	}

    public void Jump(int fingerId)
    {
        if(!IsJumping()) StartCoroutine(Jumping(fingerId));
    }

    //pulo inspirado pelo vídeo https://www.youtube.com/watch?v=7KiK0Aqtmzc
    //agradecimentos a Board to Bits Games
    IEnumerator Jumping(int fingerId)
    {

        _playerRB.AddForce(Vector2.up * JumpForce);

        yield return new WaitUntil(() => _playerRB.velocity.y < 0 || (Input.GetTouch(fingerId).phase == TouchPhase.Ended || Input.GetTouch(fingerId).phase == TouchPhase.Canceled));

        //se o pulo é parcial(ie o botão foi solto antes do player atingir o ápice), aplica a gravidade disso
        if(Input.GetTouch(fingerId).phase == TouchPhase.Ended || Input.GetTouch(fingerId).phase == TouchPhase.Canceled)
        {
            _playerRB.gravityScale += Add_Gravity_LowJump;
            //não acho bom usar tempo pra isso
            yield return new WaitWhile(() => IsJumping());
            _playerRB.gravityScale -= Add_Gravity_LowJump;
            yield return null;
        }
        //player alcançou o ápice e começa a descida
        else if(_playerRB.velocity.y < 0)
        {
            _playerRB.gravityScale += Add_Gravity_Fall;
            yield return new WaitWhile(() => IsJumping());
            _playerRB.gravityScale -= Add_Gravity_Fall;
            yield return null;
        }
    }

    private bool IsJumping()
    {
        //usa raycast pra ver se há algum objeto abaixo do player, até certa distância
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, jumpCheckDistance, _layerMask);
        if (hit.collider == null)
        {
            return true;
        }
        else
        {
            Debug.Log("HIT!" + hit);
            return false;
        }
    }
}
