/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapData : MonoBehaviour {

    #region Public Variables
    #endregion

    #region Private Variables
    private List<string> _maps;
    private string _nextMap;
    #endregion

    #region Start and Awake
    void Awake()
    {
        _maps = new List<string>();

        StartMapsToLoad();
    }

    /// <summary>
    /// Pega arquivos na pasta de mapas 
    /// </summary>
    void StartMapsToLoad()
    {
        DirectoryInfo mapFolder = new DirectoryInfo(Path.Combine(Application.dataPath, MapGenerator.MapsFolder));
        FileInfo[] mapFiles = mapFolder.GetFiles("*.json");

        foreach (FileInfo f in mapFiles)
        {
            _maps.Add(f.Name);
        }

        for (int i = 0; i < Count(); i++)
        {
            //Remove a extensão .json
            int dotIndex = _maps[i].IndexOf("."); //índice onde começa a extensão
            if (dotIndex > 0)
            {
                _maps[i] = _maps[i].Substring(0, dotIndex);
            }

            //Remove mapas que não são do modo
            string s = _maps[i];
        }
    }
    #endregion

    public string this[int i]
    {
        get { return _maps[i]; }
        set { _maps[i] = value; }
    }

    public void Add(string map)
    {
        if(_maps == null)
        {
            _maps = new List<string>();
        }

        _maps.Add(map);
    }

    public void Remove(string map)
    {
        _maps.Remove(map);
    }

    public int Count()
    {
        if(_maps == null)
        {
            return 0;
        }

        return _maps.Count;
    }


    public string GetRandomMap()
    {
        if(_maps != null)
        {
            return _maps[Random.Range(0, _maps.Count)];
        }
        Debug.Log("_maps nulo (provavelmente pelo fato de estar no editor do unity, sem estar rodando o jogo, portanto não passou pelo Awake())");
        return "";
    }
}
