/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : AffectedByPressurePlate {
    private bool _isLocked;
    private int _buttonsPressed;
    public bool BeginLocked;
    public string NextScene;
    public int ButtonsNeededToUnlock = 1;
	public GameObject WinCanvas;

	// Use this for initialization
	void Start () {
        _isLocked = BeginLocked;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == PlayerInfo.PlayerInstance.gameObject || collision.otherCollider.gameObject == PlayerInfo.PlayerInstance.gameObject)
        {
            if(_isLocked == false)
            {
                OnEnter();
            }
        }
    }

    public void OnEnter()
    {
		if (WinCanvas != null) {
			WinCanvas.SetActive (true);
		} else {
			SceneManager.LoadScene (NextScene);
		}
    }

    public override void OnPressed()
    {
        _buttonsPressed++;

        if (_buttonsPressed >= ButtonsNeededToUnlock)
        {
            Debug.Log("Porta destrancada");
            _isLocked = false;
        }
    }

    public override void OnUnpressed()
    {
        _buttonsPressed--;


        if (_buttonsPressed < ButtonsNeededToUnlock)
        {
            Debug.Log("Porta trancada");
            _isLocked = true;
        }
    }
}
