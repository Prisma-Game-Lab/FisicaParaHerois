/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    static public GameObject PlayerGameObject;
    public float Life;
    public List<IAction<float>> Actions;

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        //Seta a referência do gameObject do player
        PlayerGameObject = gameObject;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Move(Vector2 dir)
    {

    }
}
