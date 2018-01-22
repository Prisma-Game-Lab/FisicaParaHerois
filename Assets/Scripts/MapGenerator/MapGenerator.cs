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
    [Tooltip("Tamanho do tile no Unity em relação ao Tiled")] public int TileSize;

    [Header("Floor Tiles")]
    public Sprite top_left;
    public Sprite top_right;

    [Header("Load File")]
    public string fileName; // The name of the file that will be loaded 

    [Header("Generated Prefabs")] // The prefabs that the script will Instantiate
    [Tooltip("Inclua aqui todos os prefabs presentes em qualquer fase, por exemplo, background e canvas. Se houver uma ordem necessária de inicialização, respeite essa ordem ao incluir os itens")] public GameObject[] EssentialPrefabs;
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
                    for (int j = 0; j < layer.width; j++)
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
					posZ = -0.55f;
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
                                    ChangeFloorTile(instantiatedPrefab, CheckFloorDirection(layer, i));
                                }

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

                                //Tile não deve ser instanciado
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

                                //Tile não deve ser instanciado
                                if (!CheckRepeatedTiles(layer, i, 4, 1))
                                {
                                    break;
                                }

                                instantiatedPrefab = Instantiate(SeesawPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("PhysicsObjects").transform);
                                instantiatedPrefab.transform.localPosition = new Vector3(posX * TileSize - 1.18f, posY * TileSize + 0.29f, posZ);
                                instantiatedPrefab.name = "Gangorra"; //Correção de bug no PlayerInput
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

    public void CreateFloorCollider(Layer layer, int i, int j, GameObject startFloor, bool[,] colliderMatrix)
    {
        //Checa matriz de collider
        if (colliderMatrix[i, j] == true)
        {
            return;
        }

        //Cria lista de pontos
        List<Vector2> edges = new List<Vector2>();

        //Cria emptyGameObj de colliders se ainda não houver
        var collidersEmptyObj = GameObject.Find("Colliders");
        if (collidersEmptyObj == null)
        {
            collidersEmptyObj = new GameObject("Colliders");
            collidersEmptyObj.transform.parent = GameObject.Find("GeneratedTiles").transform;
            collidersEmptyObj.transform.position = Vector3.zero;
            collidersEmptyObj.transform.localPosition = Vector3.zero;
            collidersEmptyObj.transform.localRotation = Quaternion.identity;
        }

        //Cria collider
        EdgeCollider2D collider = collidersEmptyObj.AddComponent<EdgeCollider2D>();
        Collider2D floorCollider = startFloor.GetComponent<Collider2D>();
        collider.bounds.SetMinMax(floorCollider.bounds.min, floorCollider.bounds.max);
        edges.Add(floorCollider.bounds.min);

        //Modifica colliderMatrix
        colliderMatrix[i, j] = true;
        

        //Checa blocos em volta
        for (int v = j; v < layer.height; v++)
        {
            //Checa blocos anteriores
            for (int h = i - 1; h >= 0; h--)
            {
                //Cria edge se não houver blocos em volta
                if (layer.data[v * layer.width + h] != layer.data[j * layer.width + i])
                {
                    Vector2 pos = floorCollider.bounds.min - (j - v + 1) * (floorCollider.bounds.max - floorCollider.bounds.min); //posição estimada
                    edges.Add(pos);
                    break;
                }

                //Modifica blocos afetados pelo collider na colliderMatrix
                colliderMatrix[v, h] = true;
            }

            //Checa blocos posteriores
            for (int h = i + 1; h < layer.width; h++)
            {
                //Cria edge se não houver blocos em volta
                if (layer.data[v * layer.width + h] != layer.data[j * layer.width + i])
                {
                    Vector2 pos = floorCollider.bounds.max + (v-1-j)*(floorCollider.bounds.max - floorCollider.bounds.min); //posição estimada
                    edges.Add(pos);
                    break;
                }

                //Modifica blocos afetados pelo collider na colliderMatrix
                colliderMatrix[v, h] = true;
            }
        }


        //Deleta collider original do chão
        //Destroy(floorCollider);

        //Passa lista de pontos para o EdgeCollider2D
        collider.points = edges.ToArray();
    }

	/// <summary>
	/// Checa se o chão deve ter borda
	/// </summary>
	/// <returns><c>true</c>, se deve ter borda, <c>false</c> caso contrário.</returns>
	/// <param name="layer">Layer atual</param>
	/// <param name="i">Posição atual do layer que está sendo percorrido</param>
	public bool CheckFloorWithEdge(Layer layer, int i){
		if (i - layer.width < 0) {
			return true;
		}

		if (layer.data [i - layer.width] == layer.data [i]) {
			return false;
		}

		return true;
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
    /// Checa se o chão atual é uma borda
    /// </summary>
    /// <returns>null se não for borda, e o nome da direção a ser passada para o ChangeFloorTile caso contrário.</returns>
    public string CheckFloorDirection(Layer layer, int i)
    {
        //Checa se é o primeiro tile da linha
        if (i % layer.width == 0)
        {
            return "top_left";
        }

        //Checa se é o último tile da linha
        if (i % layer.width == (layer.width - 1))
        {
            return "top_right";
        }

        //Checa se o tile a esquerda é diferente
        if ((layer.data[i - 1] != layer.data[i]) && (layer.data[i + 1] == layer.data[i]))
        {
            return "top_left";
        }

        //Checa se o tile a direita é diferente
        if ((layer.data[i + 1] != layer.data[i]) && (layer.data[i - 1] == layer.data[i]))
        {
            return "top_right";
        }

        return null;
    }

    /// <summary>
    /// Modifica o sprite do chão
    /// </summary>
    /// <param name="floor">O objeto que terá o sprite modificado</param>
    /// <param name="direction">A direção do chão. Valores válidos são top_right e top_left.</param>
    public void ChangeFloorTile(GameObject floor, string direction)
    {
        //Pega sprite renderer do floor
        SpriteRenderer rend = floor.transform.GetChild(1).GetComponent<SpriteRenderer>();

        //Muda ele para o sprite desejado
        switch (direction)
        {
            case "top_right":
                rend.sprite = top_right;
                break;
            case "top_left":
                rend.sprite = top_left;
                break;
            default:
                Debug.LogError("Valor inválido passado para o método ChangeFloorTile");
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
