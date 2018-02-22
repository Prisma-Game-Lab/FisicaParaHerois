/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    static public GameManager Instance;

    public static bool IsPaused;
    public static bool IsEnded;
    public float TimeInGame;
    public PlayerData Data;
    public Text TimeText;

    [Header("On Reset")]
    public bool ShouldResetPosition = true;
    public bool ShouldResetRotation = true;
    public bool ShouldResetGravity = true;
    public bool ShouldResetMass = true;
    public bool ShouldResetVelocity = true;
	public bool ShouldResetSeesaw = true;

    private static float _time;

	// Use this for initialization
	void Start () {
        IsPaused = false;
	}

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Zera o tempo
        _time = 0.0f;
        IsPaused = false;
        IsEnded = false;
	}

    public void Restart()
    {
        ReloadScene();
    }

    public void LoadLastCheckpoint()
    {
        foreach (PhysicsObject obj in PhysicsObject.PhysicsObjectList)
        {
			Debug.Log("Obj tag" + obj.tag + " name " + obj.name);
            obj.LoadLastCheckpoint();
        }
    }

    public void CreateNewCheckpoint()
    {
        foreach (PhysicsObject obj in PhysicsObject.PhysicsObjectList)
        {
            obj.NewCheckpoint();
        }
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
        Canvas cnv = (GameObject.Find("Canvas") != null) ? GameObject.Find("Canvas").GetComponent<Canvas>() : GameObject.Find("CanvasTemporaryFix").GetComponent<Canvas>();
        GameObject pause = cnv.transform.Find("Pause").GetComponent<GameObject>();

        if (IsPaused)
        {
            cnv.transform.Find("Pause").gameObject.SetActive(false);
            Time.timeScale = 1f;
            Debug.Log("Jogo despausado");
        }

        else
        {
            cnv.transform.Find("Pause").gameObject.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("Jogo pausado");
        }

        IsPaused = !IsPaused;
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
