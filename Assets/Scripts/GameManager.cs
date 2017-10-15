/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    static public GameManager Instance;

    public bool IsPaused;
    public bool IsEnded;
    public PlayerData Data;

    public Text TimeText;

    private float _time;

	// Use this for initialization
	void Start () {
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
    }
}
