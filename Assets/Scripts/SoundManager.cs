/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    static public SoundManager Instance;

    public float MasterVolume = 1.0f; //define o volume principal do jogo

	// Use this for initialization
	void Start () {
		
	}

    void Awake() {
        //Se registra como instância de SoundManager
        Instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
