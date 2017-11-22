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
	[Header("Load File")]
    public string fileName; // The name of the file that will be loaded 

	[Header("Generated Prefabs")] // The prefabs that the script will Instantiate
    public GameObject CellTiles;

    [Header("Map Randomization")]
    public static string MapsFolder = "StreamingAssets/jsonMaps";
    public MapData RandomMapData;

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
        fileName = RandomMapData.GetRandomMap();
        Debug.Log("Nome do arquivo pego aleatoriamente: " + fileName);

        if (fileName.Length > 1)
        {
            DeleteMap();
            DeleteMap();
            LoadMap();
            GenerateMap();

            /*
            //Usa OnValidate nas células
            Cell[] cells = transform.GetComponentsInChildren<Cell>();
            foreach (Cell cell in cells)
            {
                cell.GenerateCell();
            }
            */
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
		if (AllLayers != null)
		{
			foreach (Layer layer in AllLayers.layers) // For each layer of the AllLayers variable
			{
				c = 0;
				if (GameObject.Find(layer.name) == null) // Create a new Empity object to hold the objects of that layer
				{
					var layerEmptyObjt = new GameObject(layer.name);					
					layerEmptyObjt.transform.parent = GameObject.Find("GeneratedTiles").transform;					
					layerEmptyObjt.transform.position = Vector3.zero;
					layerEmptyObjt.transform.localPosition = Vector3.zero;
					layerEmptyObjt.transform.localRotation = Quaternion.identity;
				}
				for (int i = 0; i < layer.data.Length; i++)
				{
					if (i % layer.height == 0 && i != 0)
					{
						c++;
					}
					t = i - layer.width * c;

                    if(c > maxSize)
                    {
                        maxSize = c;
                    }

 					//This is the next position of the Tiles base on the counters, so the first will be (3,0,0) -> (6,0,-3) -> (9,0,-6),etc. So the tiles size will have to be changed for each game, in this case 3x3 square
					posX = (1.1f * t + 0.34f);
					posY = -0.55f;
					posZ = (-1.1f * c);

					if (layer.name == "Base") // The name of the layer inside the json file, and Tiled
					{
						switch (layer.data[i])
						{
							case 199:
                                /*
								if (GameObject.Find("FloorTiles") == null)
								{
									var floorEmpityObj = new GameObject("FloorTiles");
									floorEmpityObj.transform.parent = GameObject.Find(layer.name).transform;
									floorEmpityObj.transform.position = Vector3.zero;
									floorEmpityObj.transform.localPosition = Vector3.zero;
									floorEmpityObj.transform.localRotation = Quaternion.identity;
								}
								var UnavailableTile = Instantiate(CellTiles,Vector3.zero, Quaternion.identity, GameObject.Find("FloorTiles").transform) as GameObject;
								UnavailableTile.transform.localPosition = new Vector3 (posX, posY, posZ);
								UnavailableTile.GetComponentInChildren<Cell>().CurrentState = Cell.State.Unavailable;

								UnavailableTile.GetComponentInChildren<Cell>().Position[0] = t;
								UnavailableTile.GetComponentInChildren<Cell>().Position[1] = c;

								TileList.Add(UnavailableTile);
                                */
                                break;
							
							case 268:
                                /*
								if (GameObject.Find("FloorTiles") == null)
								{
									var floorEmpityObj = new GameObject("FloorTiles");
									floorEmpityObj.transform.parent = GameObject.Find(layer.name).transform;
									floorEmpityObj.transform.position = Vector3.zero;
									floorEmpityObj.transform.localPosition = Vector3.zero;
									floorEmpityObj.transform.localRotation = Quaternion.identity;
								}
								var DefaultTile = Instantiate(CellTiles, Vector3.zero, Quaternion.identity, GameObject.Find("FloorTiles").transform) as GameObject;
								DefaultTile.transform.localPosition = new Vector3(posX, posY, posZ);
								DefaultTile.GetComponentInChildren<Cell>().CurrentState = Cell.State.Default;

								DefaultTile.GetComponentInChildren<Cell>().Position[0] = t;
								DefaultTile.GetComponentInChildren<Cell>().Position[1] = c;

								TileList.Add(DefaultTile);
                                */

                                break;
                            case 267:
                                /*
                                if (GameObject.Find("FloorTiles") == null)
                                {
                                    var floorEmpityObj = new GameObject("FloorTiles");
                                    floorEmpityObj.transform.parent = GameObject.Find(layer.name).transform;
                                    floorEmpityObj.transform.position = Vector3.zero;
                                    floorEmpityObj.transform.localPosition = Vector3.zero;
                                    floorEmpityObj.transform.localRotation = Quaternion.identity;
                                }
                                var PowerUpTile = Instantiate(CellTiles, Vector3.zero, Quaternion.identity, GameObject.Find("FloorTiles").transform) as GameObject;
                                PowerUpTile.transform.localPosition = new Vector3(posX, posY, posZ);
                                PowerUpTile.GetComponentInChildren<Cell>().CurrentState = Cell.State.Default;

                                PowerUpTile.GetComponentInChildren<Cell>().SpawnPowerUp = true;
                                PowerUpTile.GetComponentInChildren<Cell>().Position[0] = t;
                                PowerUpTile.GetComponentInChildren<Cell>().Position[1] = c;

                                TileList.Add(PowerUpTile);
                                */

                                break;
                            case 269:
                                /*
                                if (GameObject.Find("FloorTiles") == null)
                                {
                                    var floorEmpityObj = new GameObject("FloorTiles");
                                    floorEmpityObj.transform.parent = GameObject.Find(layer.name).transform;
                                    floorEmpityObj.transform.position = Vector3.zero;
                                    floorEmpityObj.transform.localPosition = Vector3.zero;
                                    floorEmpityObj.transform.localRotation = Quaternion.identity;
                                }
                                var AltarTile = Instantiate(CellTiles, Vector3.zero, Quaternion.identity, GameObject.Find("FloorTiles").transform) as GameObject;
                                AltarTile.transform.localPosition = new Vector3(posX, posY, posZ);
                                AltarTile.GetComponentInChildren<Cell>().CurrentState = Cell.State.Altar;
                                AltarTile.GetComponentInChildren<Cell>().AltarPrefab.GetComponent<Altar>().Position = AltarTile.GetComponentInChildren<Cell>();

                                AltarTile.GetComponentInChildren<Cell>().Position[0] = t;
                                AltarTile.GetComponentInChildren<Cell>().Position[1] = c;

                                TileList.Add(AltarTile);
                                */
                                break;
                        }
						
					}
					else if (layer.name == "Player")
					{
                        switch (layer.data[i])
						{
							case 246:
                                /*
								CharDataReader.NewStartCell(TileList[i].GetComponentInChildren<Cell>());
                                */
                                break;
							case 247:
                                /*
								CharDataReader.NewStartCell(TileList[i].GetComponentInChildren<Cell>());
                                */
                                break;
							case 248:
                                /*
								CharDataReader.NewStartCell(TileList[i].GetComponentInChildren<Cell>());
                                */
                                break;
							case 249:
                                /*
								CharDataReader.NewStartCell(TileList[i].GetComponentInChildren<Cell>());
                                */
                                break;
						}
					}
				}

                /*
                GetComponent<Map>().CellPerSide = maxSize + 1;
                */
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
