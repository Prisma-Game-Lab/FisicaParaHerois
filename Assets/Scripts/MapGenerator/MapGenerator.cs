/* 
* Copyright (c) Bravarda Game Studio
* John K. Paul Project 2017
*
*/
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [Header("Tiled")]
    //Os valores colocados aqui devem ser const (nunca podem ser alterados pelo programa)

    //Base Layer
    public const int TILED_FLOOR_ID = 113;

    //Player Layer
    public const int TILED_PLAYER_ID = 116;

    //Objects Layer
    public const int TILED_BOX_ID = 115;
    public const int TILED_SEESAW_ID = 114;
    public const int TILED_DOOR_ID = 117;
    public const int TILED_PRESSUREPLATE_ID = 118;

    //Enemy Layer
    public const int TILED_ENEMY_ID = 300;
    public const int TILED_BOSS_ID = 301;


    //Outras infos
    [Tooltip("Tamanho do tile no Unity em relação ao Tiled")] public int TileSize;


    [Header("Load File")]
    public string fileName; // The name of the file that will be loaded 

    [Header("Generated Prefabs")] // The prefabs that the script will Instantiate
    [Tooltip("Inclua aqui todos os prefabs presentes em qualquer fase, por exemplo, background e canvas. Se houver uma ordem necessária de inicialização, respeite essa ordem ao incluir os itens")] public GameObject[] EssentialPrefabs;
    public GameObject FloorPrefab;
    public GameObject PlayerPrefab;
    public GameObject BoxPrefab;
    public GameObject SeesawPrefab;
    public GameObject DoorPrefab;
    public GameObject PressurePlatePrefab;
    public GameObject EnemyPrefab;
    public GameObject BossPrefab;

    [Space]
    
    private string jsonString;

	// The Position that the objects will be Instantiated
	private float posX;
	private float posY;
	private float posZ;

	private List<GameObject> TileList = new List<GameObject>();

	//Counters
    int c;
    int t;

    int maxSize = 0;
    
    Layers AllLayers;

    private void Awake()
    {
        //prepara novo mapa
        string oldName = fileName;
        //fileName = RandomMapData.GetRandomMap();
        Debug.Log("Nome do arquivo pego aleatoriamente: " + fileName);

        if (fileName.Length > 1)
        {
            DeleteMap();
            DeleteMap();
            LoadMap();
            GenerateMap();
        }
        else
        {
            fileName = oldName;
        }

        c = 0;
    }
    void Start()
    {
    }
	/// <summary>
	/// Function that generates the map, it goes through the All layers list and Instatiate every object
	/// </summary>
    public void GenerateMap()
    {
        if(GameObject.Find("GeneratedTiles") == null)
        {
            new GameObject("GeneratedTiles");
        }

        if (AllLayers != null)
		{
			foreach (Layer layer in AllLayers.layers) // For each layer of the AllLayers variable
			{
				c = 0;
				if (GameObject.Find(layer.name) == null) // Create a new Empty object to hold the objects of that layer
				{
					var layerEmptyObjt = new GameObject(layer.name);					
					layerEmptyObjt.transform.parent = GameObject.Find("GeneratedTiles").transform;					
					layerEmptyObjt.transform.position = Vector3.zero;
					layerEmptyObjt.transform.localPosition = Vector3.zero;
					layerEmptyObjt.transform.localRotation = Quaternion.identity;
				}
				for (int i = 0; i < layer.data.Length; i++)
                {
                    if (i % layer.width == 0 && i != 0)
					{
						c++;
					}
					t = (i % layer.width) - layer.height;

                    if(c > maxSize)
                    {
                        maxSize = c;
                    }

 					//This is the next position of the Tiles base on the counters, so the first will be (3,0,0) -> (6,0,-3) -> (9,0,-6),etc. So the tiles size will have to be changed for each game, in this case 3x3 square
					posX = (1.1f * t + 0.34f);
					posZ = -0.55f;
					posY = (-1.1f * c);

					if (layer.name == "Player")
					{
                        GameObject instantiatedPrefab;

                        switch (layer.data[i])
                        {
                            case TILED_FLOOR_ID:
                                if (GameObject.Find("FloorTiles") == null)
                                {
                                    var floorEmptyObj = new GameObject("FloorTiles");
                                    floorEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
                                    floorEmptyObj.transform.position = Vector3.zero;
                                    floorEmptyObj.transform.localPosition = Vector3.zero;
                                    floorEmptyObj.transform.localRotation = Quaternion.identity;
                                }

                                instantiatedPrefab = Instantiate(FloorPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("FloorTiles").transform);
                                instantiatedPrefab.transform.position = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                break;

                            case TILED_PLAYER_ID:
                                instantiatedPrefab = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("GeneratedTiles").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                break;

                            case TILED_BOX_ID:
                                if (GameObject.Find("Objects") == null)
                                {
                                    var objectsEmptyObj = new GameObject("Objects");
                                    objectsEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
                                    objectsEmptyObj.transform.position = Vector3.zero;
                                    objectsEmptyObj.transform.localPosition = Vector3.zero;
                                    objectsEmptyObj.transform.localRotation = Quaternion.identity;
                                }

                                instantiatedPrefab = Instantiate(BoxPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Objects").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                break;

                            case TILED_DOOR_ID:
                                if (GameObject.Find("Objects") == null)
                                {
                                    var objectsEmptyObj = new GameObject("Objects");
                                    objectsEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
                                    objectsEmptyObj.transform.position = Vector3.zero;
                                    objectsEmptyObj.transform.localPosition = Vector3.zero;
                                    objectsEmptyObj.transform.localRotation = Quaternion.identity;
                                }

                                //Tile não deve ser instanciado
                                if (!CheckRepeatedTiles(layer, i, 1, 2))
                                {
                                    break;
                                }

                                instantiatedPrefab = Instantiate(DoorPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Objects").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize + 0.22f, posY * TileSize + 0.53f, posZ);
                                break;

                            case TILED_PRESSUREPLATE_ID:
                                if (GameObject.Find("Objects") == null)
                                {
                                    var objectsEmptyObj = new GameObject("Objects");
                                    objectsEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
                                    objectsEmptyObj.transform.position = Vector3.zero;
                                    objectsEmptyObj.transform.localPosition = Vector3.zero;
                                    objectsEmptyObj.transform.localRotation = Quaternion.identity;
                                }

                                instantiatedPrefab = Instantiate(PressurePlatePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Objects").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                break;

                            case TILED_SEESAW_ID:
                                if (GameObject.Find("Objects") == null)
                                {
                                    var objectsEmptyObj = new GameObject("Objects");
                                    objectsEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
                                    objectsEmptyObj.transform.position = Vector3.zero;
                                    objectsEmptyObj.transform.localPosition = Vector3.zero;
                                    objectsEmptyObj.transform.localRotation = Quaternion.identity;
                                }

                                //Tile não deve ser instanciado
                                if (!CheckRepeatedTiles(layer, i, 4, 1))
                                {
                                    break;
                                }

                                instantiatedPrefab = Instantiate(SeesawPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Objects").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize - 1.18f, posY * TileSize + 0.29f, posZ);
                                break;

                            case TILED_BOSS_ID:
                                instantiatedPrefab = Instantiate(BossPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("GeneratedTiles").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                break;
                            case TILED_ENEMY_ID:
                                instantiatedPrefab = Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("GeneratedTiles").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                break;
                        }
                    }
                }
			}			
		}
		Debug.Log("Tile count" + TileList.Count);
	}
    public void DeleteMap() // Deletes the map that was loaded 
    {
		foreach (Transform child in GameObject.Find("GeneratedTiles").transform)
		{
			DestroyImmediate(child.gameObject);
		}
        TileList.Clear();
        /*
		CharDataReader.ResetStartCells();
        */
    }

	public bool CheckChildZero()
	{
		return (transform.childCount == 0);
	}

    /// <summary>
    /// Checa se há um determinado número de tiles repetidos na vertical ou na horizontal
    /// </summary>
    /// <param name="layer">Layer atual</param>
    /// <param name="i">Posição atual do layer que está sendo percorrido</param>
    /// <param name="horizontal">Número de tiles repetidos horizontalmente para o objeto. 1 se não houver repetição nesta direção.</param>
    /// <param name="vertical">Número de tiles repetidos verticalmente para o objeto. 1 se não houver repetição nesta direção.</param>
    /// <returns>True, se o objeto deve ser instanciado e false, caso contrário.</returns>
    public bool CheckRepeatedTiles(Layer layer, int i, int horizontal, int vertical)
    {
        if(horizontal == 0 || vertical == 0)
        {
            Debug.LogError("A função CheckRepeatedTiles não deve receber 0");
        }

        int repetidosHorizontal = 0, repetidosVertical = 0;

        //Checa repetição horizontal
        for(int j = 0; j < horizontal || (layer.data[i - j] == layer.data[i]); j++)
        {
            if(layer.data[i-j] != layer.data[i])
            {
                //Tile atual ainda faz parte da repetição de um tile, não spawnar
                return false;
            }
            repetidosHorizontal++;
        }

        //Checa repetição vertical
        for (int j = 0; j < vertical || (layer.data[i - layer.width*j] == layer.data[i]); j++)
        {
            if (layer.data[i - layer.width*j] != layer.data[i])
            {
                //Tile atual ainda faz parte da repetição de um tile, não spawnar
                return false;
            }
            repetidosVertical++;
        }

        /*Retorna o fato de o número de tiles repetidos ser o múltiplo de tiles 
         * que devem ser repetidos em uma das direções, ou seja, diz se o tile atual está no meio
         * das repetições de um tile anterior e, portanto, não deve ser instanciado*/
        return ((repetidosVertical % vertical == 0) && (repetidosHorizontal % horizontal == 0));
    }

	/// <summary>
	/// Function that loads the map file, and dumps it to the AllLayers variable
	/// </summary>
    public void LoadMap()
    {
        jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/jsonMaps/" + fileName + ".json");
        if (jsonString != null)
        {
            AllLayers = JsonUtility.FromJson<Layers>(jsonString);
            Debug.Log("Successfully Loaded: " + fileName);
        }
        else
        {
            Debug.Log("MAP FILE NOT FOUND, TRY CHANGING FILE NAME");
        }
    }

    [Serializable]
    public class Layers
    {
        public List<Layer> layers;
    }

	//Class that hold all the information of the json File
    [Serializable]
    public class Layer
    {
        public int[] data;
        public string name;
        public int opacity;
        public string type;
        public bool visible;
        public int width;
        public int height;
        public int x;
        public int y;
    }
}
