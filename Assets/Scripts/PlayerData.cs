/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    public int Level;
    public float Time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Chamado quando o jogo acaba (Game Manager?)
    public void StoreData(int lvl, float time)
    {
        Level = lvl;
        Time = time;
    }
}
