/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    private bool _isLocked;
    public bool BeginLocked;
    public string NextScene;

	// Use this for initialization
	void Start () {
        _isLocked = BeginLocked;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider == PlayerInfo.PlayerInstance.gameObject || collision.otherCollider == PlayerInfo.PlayerInstance.gameObject)
        {
            if(_isLocked == false)
            {
                OnEnter();
            }
        }
    }

    public void OnEnter()
    {
        Application.LoadLevel(NextScene);
    }

    public void Unlock() {
        _isLocked = false;
    }
}
