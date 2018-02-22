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
	public const int StartCountingFrom = 112;
    public const int TILED_FLOOR_ID = 1;

    //Player Layer
    public const int TILED_PLAYER_ID = 4;

    //Objects Layer
    public const int TILED_BOX_ID = 3;
    public const int TILED_SEESAW_ID = 2;
    public const int TILED_DOOR_ID = 5;
    public const int TILED_PRESSUREPLATE_ID = 6;

    //Enemy Layer
    public const int TILED_ENEMY_ID = 188;
    public const int TILED_BOSS_ID = 189;


    //Outras infos
    [Tooltip("Tamanho do tile no Unity em rela��o ao Tiled")] public int TileSize;

    [Header("Floor Tiles")]
    public Sprite top_left;
    public Sprite top_right;
    public Sprite top;
    public Sprite middle_right;
    public Sprite single;
    public Sprite single_line_left;
    public Sprite single_line_right;

    [Header("Load File")]
    public string fileName; // The name of the file that will be loaded 

    [Header("Generated Prefabs")] // The prefabs that the script will Instantiate
    [Tooltip("Inclua aqui todos os prefabs presentes em qualquer fase, por exemplo, background e canvas. Se houver uma ordem necess�ria de inicializa��o, respeite essa ordem ao incluir os itens")] public GameObject[] EssentialPrefabs;
    public GameObject FloorPrefab;
	public GameObject FloorWithEdgePrefab;
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
                //Criando e inicializando matriz de collider
                bool[,] colliderMatrix = new bool[layer.width, layer.height];
                for (int i = 0; i < layer.width; i++)
                {
                    for (int j = 0; j < layer.height; j++)
                    {
                        colliderMatrix[i, j] = false;
                    }
                }

                c = 0;
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
					posZ = 0;
					posY = (-1.1f * c);

					if (layer.name == "Player")
					{
                        GameObject instantiatedPrefab;

                        switch (layer.data[i])
                        {
							case TILED_FLOOR_ID:
							case TILED_FLOOR_ID + StartCountingFrom:
								if (GameObject.Find ("FloorTiles") == null) {
									var floorEmptyObj = new GameObject ("FloorTiles");
									floorEmptyObj.transform.parent = GameObject.Find ("GeneratedTiles").transform;
									floorEmptyObj.transform.position = Vector3.zero;
									floorEmptyObj.transform.localPosition = Vector3.zero;
									floorEmptyObj.transform.localRotation = Quaternion.identity;
								}

								bool shouldSpawnFloorWithEdge = CheckFloorWithEdge (layer, i); 
								GameObject prefabToInstantiate = shouldSpawnFloorWithEdge ? FloorWithEdgePrefab : FloorPrefab;
								Vector2 correctPosition = shouldSpawnFloorWithEdge ? new Vector2(0,-0.023f) : new Vector2(0,0);

								instantiatedPrefab = Instantiate(prefabToInstantiate, Vector3.zero, Quaternion.identity, GameObject.Find("FloorTiles").transform);
                            	instantiatedPrefab.transform.position = new Vector3(posX * TileSize - correctPosition.x, posY * TileSize - correctPosition.y, posZ);
                                //instantiatedPrefab.transform.localScale = new Vector3(0.23f, 0.23f, 1);

                                if (shouldSpawnFloorWithEdge)
                                {
                                    Debug.Log(CheckFloorDirection(layer, i));
                                    ChangeFloorTile(instantiatedPrefab, CheckFloorDirection(layer, i));
                                }

                                //Collider unificado
                                int coluna = i % layer.width; //i
                                int linha = i/layer.width; //j;
                                //CreateFloorCollider(layer, coluna, linha, instantiatedPrefab, colliderMatrix);

                                //Collider separado
                                GameObject colliderEmptyObj = GameObject.Find("Colliders");
                                if (colliderEmptyObj == null)
                                {
                                    colliderEmptyObj = new GameObject("Colliders");
                                    colliderEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
                                    colliderEmptyObj.transform.position = Vector3.zero;
                                    colliderEmptyObj.transform.localPosition = Vector3.zero;
                                    colliderEmptyObj.transform.localRotation = Quaternion.identity;
                                }

                                Transform floor_Collider = instantiatedPrefab.transform.Find("Floor_Collider");
                                floor_Collider.parent = colliderEmptyObj.transform;

                            	break;

							case TILED_PLAYER_ID:
							case TILED_PLAYER_ID + StartCountingFrom:
                             	instantiatedPrefab = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("GeneratedTiles").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                instantiatedPrefab.name = "Player";

                                //Seta ActionMenu
                                GameObject ActionMenu = GameObject.Find("ActionPanel");
                                instantiatedPrefab.GetComponent<PlayerInput>().ActionMenu = ActionMenu;
                                break;

							case TILED_BOX_ID:
							case TILED_BOX_ID + StartCountingFrom:
                                if (GameObject.Find("PhysicsObjects") == null)
                                {
                                    var objectsEmptyObj = new GameObject("PhysicsObjects");
                                    objectsEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
                                    objectsEmptyObj.transform.position = Vector3.zero;
                                    objectsEmptyObj.transform.localPosition = Vector3.zero;
                                    objectsEmptyObj.transform.localRotation = Quaternion.identity;
                                }

                                instantiatedPrefab = Instantiate(BoxPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("PhysicsObjects").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                break;

							case TILED_DOOR_ID:
							case TILED_DOOR_ID + StartCountingFrom:
                                if (GameObject.Find("Objects") == null)
                                {
                                    var objectsEmptyObj = new GameObject("Objects");
                                    objectsEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
                                    objectsEmptyObj.transform.position = Vector3.zero;
                                    objectsEmptyObj.transform.localPosition = Vector3.zero;
                                    objectsEmptyObj.transform.localRotation = Quaternion.identity;
                                }

                                //Tile n�o deve ser instanciado
                                if (!CheckRepeatedTiles(layer, i, 1, 2))
                                {
                                    break;
                                }

                                instantiatedPrefab = Instantiate(DoorPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Objects").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize + 0.22f, posY * TileSize + 0.53f, posZ);
                                break;

							case TILED_PRESSUREPLATE_ID:
							case TILED_PRESSUREPLATE_ID + StartCountingFrom:
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
							case TILED_SEESAW_ID + StartCountingFrom:
                                if (GameObject.Find("PhysicsObjects") == null)
                                {
                                    var objectsEmptyObj = new GameObject("PhysicsObjects");
                                    objectsEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
                                    objectsEmptyObj.transform.position = Vector3.zero;
                                    objectsEmptyObj.transform.localPosition = Vector3.zero;
                                    objectsEmptyObj.transform.localRotation = Quaternion.identity;
                                }

                                //Tile n�o deve ser instanciado
                                if (!CheckRepeatedTiles(layer, i, 4, 1))
                                {
                                    break;
                                }

                                instantiatedPrefab = Instantiate(SeesawPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("PhysicsObjects").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize - 1.18f, posY * TileSize + 0.29f, posZ);
                                instantiatedPrefab.name = "Gangorra"; //Corre��o de bug no PlayerInput
                                break;

							case TILED_BOSS_ID:
							case TILED_BOSS_ID + StartCountingFrom:
                                instantiatedPrefab = Instantiate(BossPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("GeneratedTiles").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                break;
							case TILED_ENEMY_ID:
							case TILED_ENEMY_ID + StartCountingFrom:
                                instantiatedPrefab = Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("GeneratedTiles").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize, posY * TileSize, posZ);
                                break;                        }
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
	/// Checa se o ch�o deve ter borda
	/// </summary>
	/// <returns><c>true</c>, se deve ter borda, <c>false</c> caso contr�rio.</returns>
	/// <param name="layer">Layer atual</param>
	/// <param name="i">Posi��o atual do layer que est� sendo percorrido</param>
	public bool CheckFloorWithEdge(Layer layer, int i){
		if (i - layer.width < 0) {
			return true;
		}

        if (layer.data[i + 1] != layer.data[i])
        {
            return true;
        }

		if (layer.data [i - layer.width] == layer.data [i]) {
			return false;
		}

		return true;
	}

    /// <summary>
    /// Checa se h� um determinado n�mero de tiles repetidos na vertical ou na horizontal
    /// </summary>
    /// <param name="layer">Layer atual</param>
    /// <param name="i">Posi��o atual do layer que est� sendo percorrido</param>
    /// <param name="horizontal">N�mero de tiles repetidos horizontalmente para o objeto. 1 se n�o houver repeti��o nesta dire��o.</param>
    /// <param name="vertical">N�mero de tiles repetidos verticalmente para o objeto. 1 se n�o houver repeti��o nesta dire��o.</param>
    /// <returns>True, se o objeto deve ser instanciado e false, caso contr�rio.</returns>
    public bool CheckRepeatedTiles(Layer layer, int i, int horizontal, int vertical)
    {
        if(horizontal == 0 || vertical == 0)
        {
            Debug.LogError("A fun��o CheckRepeatedTiles n�o deve receber 0");
        }

        int repetidosHorizontal = 0, repetidosVertical = 0;

        //Checa repeti��o horizontal
        for(int j = 0; j < horizontal || (layer.data[i - j] == layer.data[i]); j++)
        {
            if(layer.data[i-j] != layer.data[i])
            {
                //Tile atual ainda faz parte da repeti��o de um tile, n�o spawnar
                return false;
            }
            repetidosHorizontal++;
        }

        //Checa repeti��o vertical
        for (int j = 0; j < vertical || (layer.data[i - layer.width*j] == layer.data[i]); j++)
        {
            if (layer.data[i - layer.width*j] != layer.data[i])
            {
                //Tile atual ainda faz parte da repeti��o de um tile, n�o spawnar
                return false;
            }
            repetidosVertical++;
        }

        /*Retorna o fato de o n�mero de tiles repetidos ser o m�ltiplo de tiles 
         * que devem ser repetidos em uma das dire��es, ou seja, diz se o tile atual est� no meio
         * das repeti��es de um tile anterior e, portanto, n�o deve ser instanciado*/
        return ((repetidosVertical % vertical == 0) && (repetidosHorizontal % horizontal == 0));
    }

    /// <summary>
    /// Checa se o ch�o atual � uma borda
    /// </summary>
    /// <returns>null se n�o for borda, e o nome da dire��o a ser passada para o ChangeFloorTile caso contr�rio.</returns>
    public string CheckFloorDirection(Layer layer, int i)
    {
        //Checa se n�o � a primeira ou a �ltima linha
        if (((i - layer.width) >= 0) && ((i + layer.width) < layer.data.Length))
        {    
            //Checa se n�o h� tiles em cima ou embaixo
            if ((layer.data[i - layer.width] != layer.data[i]) && (layer.data[i + layer.width] != layer.data[i]))
            {
                //Checa se n�o h� tiles nas laterais  
                if ((layer.data[i + 1] != layer.data[i]) && (layer.data[i - 1] != layer.data[i]))
                {
                    return "single";
                }

                //Checa se n�o h� tiles em cima ou embaixo e � o �ltimo tile da linha
                if ((i % layer.width == (layer.width - 1)) || (layer.data[i + 1] != layer.data[i]))
                {
                    return "single_line_right";
                }

                //Checa se n�o h� tiles em cima ou embaixo e � o primeiro tile da linha
                if (i % layer.width == 0 || (layer.data[i - 1] != layer.data[i]))
                {
                    return "single_line_left";
                }

                else
                {
                    return "top";
                }
            }
        }

        //Checa se � um tile do topo
        if ((i - layer.width < 0) || (layer.data[i - layer.width] != layer.data[i]))
        {
            //Checa se � o primeiro tile da linha
            if (i % layer.width == 0)
            {
                return "top_left";
            }

            //Checa se � o �ltimo tile da linha
            if (i % layer.width == (layer.width - 1))
            {
                return "top_right";
            }

            //Checa se o tile a esquerda � diferente
            if ((layer.data[i - 1] != layer.data[i]) && (layer.data[i + 1] == layer.data[i]))
            {
                return "top_left";
            }

            //Checa se o tile a direita � diferente
            if ((layer.data[i + 1] != layer.data[i]) && (layer.data[i - 1] == layer.data[i]))
            {
                return "top_right";
            }

            else
            {
                return "top";
            }
        }

        else
        {
            return "middle_right";
        }
    }

    /// <summary>
    /// Modifica o sprite do ch�o
    /// </summary>
    /// <param name="floor">O objeto que ter� o sprite modificado</param>
    /// <param name="direction">A dire��o do ch�o. Valores v�lidos s�o top_right e top_left.</param>
    public void ChangeFloorTile(GameObject floor, string direction)
    {
        //Pega sprite renderer do floor
        SpriteRenderer rend = floor.GetComponent<Floor>().rend;

        //Muda ele para o sprite desejado
        switch (direction)
        {
            case "single":
                rend.sprite = single;
                break;
            case "single_line_right":
                rend.sprite = single_line_right;
                break;
            case "single_line_left":
                rend.sprite = single_line_left;
                break;
            case "top_right":
                rend.sprite = top_right;
                break;
            case "top_left":
                rend.sprite = top_left;
                break;
            case "top":
                rend.sprite = top;
                break;
            case "middle_right":
                rend.sprite = middle_right;
                break;
            default:
                Debug.LogError("Valor inv�lido passado para o m�todo ChangeFloorTile");
                break;
        }
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
