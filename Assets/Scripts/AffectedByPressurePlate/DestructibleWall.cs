using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : AffectedByPressurePlate {
    public List<GameObject> CollidersToDisable;

	public Animator _Anim;
	public int ButtonsNeededToUnlock = 1;

	private int _buttonsPressed = 0;

	public void Start(){
		_Anim.SetBool ("open", false);
	}

	public void Update(){
	}

	public override void OnPressed () 
    {
		_buttonsPressed++;

		if (_buttonsPressed >= ButtonsNeededToUnlock)
		{
			Debug.Log("Porta destrancada");
			_Anim.SetBool ("open", true);
			SetActive(false);
		}
	}
	
    public override void OnUnpressed()
	{       
		_buttonsPressed--;

		if (_buttonsPressed < ButtonsNeededToUnlock)
		{
			Debug.Log("Porta trancada");
			_Anim.SetBool ("open", false);
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
}
