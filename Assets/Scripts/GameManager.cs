/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.SceneManagement;
=======
using UnityEngine.UI;
>>>>>>> ba5cb325c7503e84d3418f283386c1cdfecf09b0

public class GameManager : MonoBehaviour {
    static public GameManager Instance;

    public static bool IsPaused;
    public bool IsEnded;
<<<<<<< HEAD
    public float TimeInGame;

=======
>>>>>>> ba5cb325c7503e84d3418f283386c1cdfecf09b0
    public PlayerData Data;

    public Text TimeText;

    private float _time;

	// Use this for initialization
	void Start () {
<<<<<<< HEAD
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
=======
        //Zera o tempo
        _time = 0.0f;
        IsPaused = false;
        IsEnded = false;
	}

    void Awake()
    {
        //Se registra como instância de GameManager
        Instance = this;
    }

    // Update is called once per frame
    void Update () {
        //Se o level já acabou ou o jogo está pausado, não faz nada
        if (IsEnded || IsPaused)
        {
            return;
        }

        //Incrementa o tempo
        _time += Time.deltaTime;
        TimeText.text = _time.ToString("0.00");
	}

    /// <summary>
    /// Define o que ocorre quando a fase termina
    /// </summary>
    void OnLevelEnd()
    {
        IsEnded = true;

        //Registra dados do level no playerData
        if(Data != null)
        {
            Data.Time = _time;
        }
    }

    /// <summary>
    /// Define o que ocorre quando o jogo é pausado ou despausado
    /// </summary>
    public void OnPause()
    {
        if (IsPaused)
        {
            Time.timeScale = 1f;
            Debug.Log("Jogo despausado");
        }

        else
        {
            Time.timeScale = 0f;
            Debug.Log("Jogo pausado");
        }

        IsPaused = !IsPaused;
>>>>>>> ba5cb325c7503e84d3418f283386c1cdfecf09b0
    }
}
