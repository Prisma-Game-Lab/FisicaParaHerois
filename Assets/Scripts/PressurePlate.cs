/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {
    public Door DoorToOpen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == PlayerInfo.PlayerInstance.gameObject 
            || collision.otherCollider.gameObject == PlayerInfo.PlayerInstance.gameObject
            || collision.collider.tag == "Box"
            || collision.otherCollider.tag == "Box")
        {
            Debug.Log("Porta aberta");
            DoorToOpen.Unlock();
        }
    }
}
