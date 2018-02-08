using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gangorra : MonoBehaviour {

	public GameObject base_gangorra;
	public float visual_distance_connecting_point;

	private Collider2D _coll;
	private PhysicsObject _po;

	// Use this for initialization
	void Start () {
		_coll = this.GetComponent<Collider2D> ();
		if (_coll == null) {
			Debug.LogError ("Gangorra não possui um collider atrelado!");
		}

		_po = this.GetComponent<PhysicsObject> ();
		if (_po == null) {
			Debug.LogError ("Gangorra não possui componente PhysicsObject");
		}

		visual_distance_connecting_point = this.transform.position.y - base_gangorra.transform.position.y;

	}

	void OnCollisionStay2D(Collision2D coll)
	{
		//se a colisão é com chão, ignora
		if(coll.collider.CompareTag("Floor")) return;

		//Debug.Log ("false:" + coll.gameObject.ToString());
		//se há uma colisão, bloqueia a alteração sobre o eixo da gangorra
		_po.AvailableActions.ChangeAnchorPointAction = false;
	}

	void OnCollisionExit2D()
	{
		Debug.Log ("exit");
		_po.AvailableActions.ChangeAnchorPointAction = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
