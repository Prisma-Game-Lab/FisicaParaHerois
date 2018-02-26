using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : AffectedByPressurePlate {
    public List<GameObject> CollidersToDisable;
	[Tooltip("Posição para onde a porta deve ir")] public Vector3 TargetPosition;
	public float Speed = 10f;

	public Vector3 _initialPosition;
	public bool _goingUp;
	public int ButtonsNeededToUnlock = 1;

	private int _buttonsPressed = 0;

	public void Start(){
		_initialPosition = transform.position;
		_goingUp = false;
	}

	public void Update(){
		switch (_goingUp) {
		case true:
			transform.Translate ((TargetPosition - transform.position) * (1 / Speed));
			break;
		case false:
			transform.Translate ((_initialPosition - transform.position) * (1 / Speed));
			break;
		}
	}

	public override void OnPressed () 
    {
		_buttonsPressed++;

		if (_buttonsPressed >= ButtonsNeededToUnlock)
		{
			Debug.Log("Porta destrancada");
			_goingUp = true;
			SetActive(false);
		}
	}
	
    public override void OnUnpressed()
	{       
		_buttonsPressed--;

		if (_buttonsPressed < ButtonsNeededToUnlock)
		{
			Debug.Log("Porta trancada");
			_goingUp = false;
			SetActive(true);
		}
	}

    void SetActive(bool status)
    {
        foreach (GameObject collider in CollidersToDisable)
        {
            collider.SetActive(status);
        }
    }

	public void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(TargetPosition, 0.25f);
	}
}
