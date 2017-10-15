/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static bool IsPaused;
    public bool IsEnded;
    public float TimeInGame;

    public PlayerData Data;

	// Use this for initialization
	void Start () {
        IsPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
        TimeInGame += Time.deltaTime;

        if (IsEnded)
        {
            Data.StoreData(1, TimeInGame);
        }
	}

    public static void PauseGame()
    {

        Canvas cnv = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject pause = cnv.transform.FindChild("Pause").GetComponent<GameObject>();

        if (Time.timeScale == 0)
        {
            cnv.transform.FindChild("Pause").gameObject.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
        } else
        {
            cnv.transform.FindChild("Pause").gameObject.SetActive(true);
            Time.timeScale = 0;
            IsPaused = true;
        }
        
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
