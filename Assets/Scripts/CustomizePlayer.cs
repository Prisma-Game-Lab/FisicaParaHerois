using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CustomizePlayer : MonoBehaviour {

	public Button Up;
	public Button Down;
	public Button Left;
	public Button Right;
	public Button[] Categories;
	public Button[] Color;

	public GameObject Estampa;
	public GameObject CiliosItem;
	public GameObject BarbaItem;

	public GameObject PlayerCustomizable;
	public GameObject PlayerInGame;

	[Header ("Camiseta")]
	public Sprite[] Camisetas;
	public Sprite[] Estampas;
	public Color[] CorCamisetas;

    [Header("Olho")]
    public Sprite[] Olhos;
	public Color[] CorOlhos;
    public Sprite[] Cilios;

    [Header("Cor de Pele")]
    public Sprite[] Corpo;
    public Color[] CorPele;

    [Header("Nariz")]
    public Sprite[] Narizes;
    public Color[] CorNariz;

    [Header("Boca")]
    public Sprite[] Bocas;
    public Color[] CorBoca;

    [Header("Cabelo")]
    public Sprite[] Cabelos;
    public Color[] CorCabelo;
	public Sprite[] Barba;

    [Header("Calça")]
    public Sprite[] Calcas;
    public Color[] CorCalca;

    [Header("Sapato")]
    public Sprite[] Sapatos;
    public Color[] CorSapato;

    [Header("Acessorio")]
    public Sprite[] Acessorios;
    public Color[] CorAcessorio;

	public GameObject Sombrancelha;

	public Sprite spritePack;

	public GameObject camera;

	private Transform[] bigPlayerObjs;

	private ScrollRect _CategoryScroll;
	private RectTransform _ColorScroll;

	private int _numberColorsActive = 5;
	private string _currentCategory;
	private int _indexItemCategoria = 0;
	private int _indexCategoriaAcessorio = 0;

	static public string _camisaCategory = "Camisas";
	static public string _olhosCategory = "Olhos";
	static public string _cabeloCategory = "Cabelo";
	static public string _narizCategory = "Nariz";
	static public string _bocaCategory = "Boca";
	static public string _calcaCategory = "Calca";
	static public string _sapatoCategory = "Sapato";
	static public string _acessorioCategory = "Acessorios";
	static public string _corDePeleCategory = "Cor de pele";

	public struct Camisa {

		public Sprite camiseta;
		public Sprite estampa;
		public Color cor;
		public int index;
		public int indexEstampa;
		public int indexCor;

		public Camisa (Sprite camiseta, Sprite estampa, Color cor, int index, int indexEstampa, int indexCor){
			this.camiseta = Sprite.Create(camiseta.texture,camiseta.rect, camiseta.pivot);
			this.camiseta.name = camiseta.name;
			this.estampa = Sprite.Create(estampa.texture,estampa.rect, estampa.pivot);
			this.estampa.name = estampa.name;
			this.cor = cor;
			this.index = index;
			this.indexEstampa = indexEstampa;
			this.indexCor = indexCor;
		}
	}

	public struct Olho {

		public Sprite olhos;
		public Sprite cilios;
		public Color cor;
		public int index;
		public int indexCilios;
		public int indexCor;

		public Olho (Sprite olhos, Sprite cilios, Color cor, int index, int indexCilios, int indexCor){
			this.olhos = Sprite.Create(olhos.texture,olhos.rect, olhos.pivot);
			this.olhos.name = olhos.name;
			this.cilios = Sprite.Create(cilios.texture,cilios.rect, cilios.pivot);
			this.cilios.name = cilios.name;
			this.cor = cor;
			this.index = index;
			this.indexCilios = indexCilios;
			this.indexCor = indexCor;
		}
	}

	public struct Cabelo {

		public Sprite cabelo;
		public Sprite barba;
		public Color cor;
		public int index;
		public int indexBarba;
		public int indexCor;

		public Cabelo (Sprite cabelo, Sprite barba, Color cor, int index, int indexBarba, int indexCor){
			this.cabelo = Sprite.Create(cabelo.texture,cabelo.rect, cabelo.pivot);
			this.cabelo.name = cabelo.name;
			this.barba = Sprite.Create(barba.texture,barba.rect, barba.pivot);
			this.barba.name = barba.name;
			this.cor = cor;
			this.index = index;
			this.indexBarba = indexBarba;
			this.indexCor = indexCor;
		}
	}

	public struct CorDePele {

		public Sprite corpo;
		public Color cor;
		public int indexCor;

		public CorDePele (Sprite corpo, Color cor, int indexCor){
			this.corpo = Sprite.Create(corpo.texture,corpo.rect, corpo.pivot);
			this.corpo.name = corpo.name;
			this.cor = cor;
			this.indexCor = indexCor;
		}
	}

	public struct Nariz {

		public Sprite nariz;
		public Color cor;
		public int index;
		public int indexCor;

		public Nariz (Sprite nariz, Color cor, int index, int indexCor){
			this.nariz = Sprite.Create(nariz.texture,nariz.rect, nariz.pivot);
			this.nariz.name = nariz.name;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	public struct Boca {

		public Sprite boca;
		public Color cor;
		public int index;
		public int indexCor;

		public Boca (Sprite boca, Color cor, int index, int indexCor){
			this.boca = Sprite.Create(boca.texture,boca.rect, boca.pivot);
			this.boca.name = boca.name;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	public struct Calca {

		public Sprite calca;
		public Color cor;
		public int index;
		public int indexCor;

		public Calca (Sprite calca, Color cor, int index, int indexCor){
			this.calca = Sprite.Create(calca.texture,calca.rect, calca.pivot);
			this.calca.name = calca.name;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	public struct Sapato {

		public Sprite sapato;
		public Color cor;
		public int index;
		public int indexCor;

		public Sapato (Sprite sapato, Color cor, int index, int indexCor){
			this.sapato = Sprite.Create(sapato.texture,sapato.rect, sapato.pivot);
			this.sapato.name = sapato.name;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	public struct Acessorio {

		public Sprite acessorio;
		public Color cor;
		public int index;
		public int indexCor;

		public Acessorio (Sprite acessorio, Color cor, int index, int indexCor){
			this.acessorio = Sprite.Create(acessorio.texture,acessorio.rect, acessorio.pivot);
			this.acessorio.name = acessorio.name;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	public struct Customize {

		public Camisa camisa;
		public Olho olhos;
		public Cabelo cabelo;
		public CorDePele corDePele;
		public Nariz nariz;
		public Boca boca;
		public Calca calca;
		public Sapato sapato;
		public Acessorio acessorio;

		public Customize (Camisa camisa, Olho olhos, Cabelo cabelo, CorDePele corDePele, Nariz nariz,
			Boca boca, Calca calca, Sapato sapato, Acessorio acessorio){
			this.camisa = camisa;
			this.olhos = olhos;
			this.cabelo = cabelo;
			this.corDePele = corDePele;
			this.nariz = nariz;
			this.boca = boca;
			this.calca = calca;
			this.sapato = sapato;
			this.acessorio = acessorio;
		}
	}

	static public Customize _changePlayer;
	#if UNITY_EDITOR
	private SpriteMetaData[] sliceMetaData;
	#endif

	// Use this for initialization
	void Start () {
		
		PlayerInGame.transform.parent.parent.GetComponent<Rigidbody2D> ().isKinematic = true;

		//AdjustSprites ();

		Up.onClick.AddListener (UpButton);
		Down.onClick.AddListener (DownButton);
		Left.onClick.AddListener (LeftButton);
		Right.onClick.AddListener (RightButton);

		for (int i = 0; i < Categories.Length; i++) {
			int index = i;
			Categories [i].onClick.AddListener (() => CategoryButton(index));
		}	

		for (int i = 0; i < Color.Length; i++) {
			int index = i;
			Color [i].onClick.AddListener (() => ColorButton(index));
		}

		_ColorScroll = Color [0].transform.parent.transform.parent.GetComponent<RectTransform> ();

		gameObject.GetComponent<Image> ().sprite = Corpo [0];
		gameObject.GetComponent<Image> ().color = CorPele [0];
		ChangePalette (CorPele);
		ActivatePattern ();
		_currentCategory = _corDePeleCategory;

		bigPlayerObjs = PlayerCustomizable.transform.GetComponentsInChildren<Transform> (true);

		_changePlayer = new Customize (
			new Camisa (Camisetas [0], Estampas [0], CorCamisetas [0], 0, 0, 0), 
			new Olho (Olhos [0], Cilios [0], CorOlhos [0], 0, 0, 0), 
			new Cabelo (Cabelos [0], Barba [0], CorCabelo [0], 0, 0, 0), 
			new CorDePele (Corpo [0], CorPele [0], 0), 
			new Nariz (Narizes [0], CorNariz [0], 0 , 0), 
			new Boca (Bocas [0], CorBoca [0], 0 , 0),
			new Calca (Calcas [0], CorCalca [0], 0 , 0), 
			new Sapato (Sapatos [0], CorSapato [0], 0 , 0), 
			new Acessorio (Acessorios [0], CorAcessorio [0], 0, 0)
		);

		ColorButton (0);
		Sombrancelha.GetComponent<Anima2D.SpriteMeshInstance> ().color = _changePlayer.cabelo.cor;


		ActivateItemBigPlayer (_changePlayer.camisa.camiseta.name, _camisaCategory, _changePlayer.camisa.cor, true);
		//ActivateItemBigPlayer (_changePlayer.camisa.estampa.name, "Estampa", _changePlayer.camisa.cor, true);
		ActivateItemBigPlayer (_changePlayer.olhos.olhos.name, _olhosCategory, _changePlayer.olhos.cor, true);
		//ActivateItemBigPlayer (_changePlayer.olhos.cilios.name);
		ActivateItemBigPlayer (_changePlayer.cabelo.cabelo.name, _cabeloCategory, _changePlayer.cabelo.cor, true);
		//ActivateItemBigPlayer (_changePlayer.cabelo.barba.name);
		ActivateItemBigPlayer (_changePlayer.nariz.nariz.name, _narizCategory, _changePlayer.nariz.cor, true);
		ActivateItemBigPlayer (_changePlayer.boca.boca.name, _bocaCategory, _changePlayer.boca.cor, true);
		ActivateItemBigPlayer (_changePlayer.calca.calca.name, _calcaCategory, _changePlayer.calca.cor, true);
		ActivateItemBigPlayer (_changePlayer.sapato.sapato.name, _sapatoCategory, _changePlayer.sapato.cor, true);
		ActivateItemBigPlayer (_changePlayer.acessorio.acessorio.name, _acessorioCategory, _changePlayer.acessorio.cor, true);


		ActivateItemLittlePlayer(_camisaCategory, _changePlayer.camisa.cor, PlayerInGame);
		ActivateItemLittlePlayer(_olhosCategory, _changePlayer.olhos.cor, PlayerInGame);
		ActivateItemLittlePlayer(_cabeloCategory, _changePlayer.cabelo.cor, PlayerInGame);
		ActivateItemLittlePlayer(_narizCategory, _changePlayer.nariz.cor, PlayerInGame);
		ActivateItemLittlePlayer(_bocaCategory, _changePlayer.boca.cor, PlayerInGame);
		ActivateItemLittlePlayer(_calcaCategory, _changePlayer.calca.cor, PlayerInGame);
		ActivateItemLittlePlayer(_sapatoCategory, _changePlayer.sapato.cor, PlayerInGame);
		ActivateItemLittlePlayer(_acessorioCategory, _changePlayer.acessorio.cor, PlayerInGame);
		ActivateItemLittlePlayer(_corDePeleCategory, _changePlayer.corDePele.cor, PlayerInGame);

	}

#if UNITY_EDITOR
	void AdjustSprites(){

		string path = AssetDatabase.GetAssetPath(spritePack);
		TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
		sliceMetaData = textureImporter.spritesheet;

		foreach (SpriteMetaData item in sliceMetaData){

			// Camisetas
			if (item.name == "cust1_0" || item.name == "cust1_1" || item.name == "cust1_21"){
				
//				if (Camisetas.Length != 0) Camisetas [Camisetas.Length - 1] = item;
//				else Camisetas [0] = item;

			// Estampas
			} else if (item.name == "cust1_26" || item.name == "cust1_27" || item.name == "cust1_29" || 
				item.name == "cust1_31" || item.name == "cust1_32" || item.name == "cust1_35" || 
				item.name == "cust1_37" || item.name == "cust1_38" || item.name == "cust4_0"){

//				if (Camisetas.Length != 0) Estampas [Camisetas.Length - 1] = item;
//				else Estampas [0] = item;

			
			}/* else if (item.name == "cust1_26" || item.name == "cust1_27" || item.name == "cust1_29" || 
				item.name == "cust1_31" || item.name == "cust1_32" || item.name == "cust1_35" || 
				item.name == "cust1_37" || item.name == "cust1_38" || item.name == "cust4_0"){

				if (Camisetas.Length != 0) Estampas [Camisetas.Length - 1] = item;
				else Estampas [0] = item;
			}*/
			
		}
//		for (i...)
//		{
//			string eachName = ...;
//				...
//				sliceMetaData[i].name = eachName; 
//		}

		// Save settings.
		textureImporter.spritesheet = sliceMetaData;
		EditorUtility.SetDirty(textureImporter);
		textureImporter.SaveAndReimport();

		// Reimport/refresh asset.
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

	}
#endif
	
	// Update is called once per frame
	void Update () {
		
		//gameObject.GetComponent<Image> ().preserveAspect = true;
		//if (_currentCategory == _cabeloCategory)
			gameObject.GetComponent<Image> ().SetNativeSize ();

		//Estampa.GetComponent<Image> ().SetNativeSize ();
		//Estampa.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100, 100);
		Estampa.GetComponent<Image> ().preserveAspect = true;
		BarbaItem.GetComponent<Image> ().SetNativeSize ();
		//BarbaItem.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100, 100);
		BarbaItem.GetComponent<Image> ().preserveAspect = true;
		//BarbaItem.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100, 100);
		CiliosItem.GetComponent<Image> ().SetNativeSize ();
		//CiliosItem.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100, 100);
		CiliosItem.GetComponent<Image> ().preserveAspect = true;
	
	}

	// Muda a estampa/cilios
    void UpButton() {

		if (_currentCategory == "Camisas") {
			if (_indexCategoriaAcessorio < Estampas.Length - 1) {
				_indexCategoriaAcessorio += 1;
				//Estampa.GetComponent<Image> ().sprite = Estampas [_indexCategoriaAcessorio];
				ActivateItemBigPlayer (Estampas [_indexItemCategoria].name, "Estampa", _changePlayer.camisa.cor, false);
				_changePlayer.camisa.indexEstampa = _indexCategoriaAcessorio;
			}
		} else if (_currentCategory == "Olhos") {
			if (_indexCategoriaAcessorio < Cilios.Length - 1) {
				_indexCategoriaAcessorio += 1;
				CiliosItem.GetComponent<Image> ().sprite = Cilios [_indexCategoriaAcessorio];
				_changePlayer.olhos.indexCilios = _indexCategoriaAcessorio;
			}
		} else if (_currentCategory == "Cabelo") {
			if (_indexCategoriaAcessorio < Barba.Length - 1) {
				_indexCategoriaAcessorio += 1;
				BarbaItem.GetComponent<Image> ().sprite = Barba [_indexCategoriaAcessorio];
				_changePlayer.cabelo.indexBarba = _indexCategoriaAcessorio;
			}
		}

	}

	// Muda a estampa/cilios
	void DownButton() {

		if (_currentCategory == "Camisas") {
			if (_indexCategoriaAcessorio > 0) {
				_indexCategoriaAcessorio -= 1;
				Estampa.GetComponent<Image> ().sprite = Estampas [_indexCategoriaAcessorio];
				//ActivateItemBigPlayer (Estampas [_indexItemCategoria].name, "Estampa", _changePlayer.camisa.cor, false);
				_changePlayer.camisa.indexEstampa = _indexCategoriaAcessorio;
			}
		} else if (_currentCategory == "Olhos") {
			if (_indexCategoriaAcessorio > 0) {
				_indexCategoriaAcessorio -= 1;
				CiliosItem.GetComponent<Image> ().sprite = Cilios [_indexCategoriaAcessorio];
				_changePlayer.olhos.indexCilios = _indexCategoriaAcessorio;
			}
		} else if (_currentCategory == "Cabelo") {
			if (_indexCategoriaAcessorio > 0) {
				_indexCategoriaAcessorio -= 1;
				BarbaItem.GetComponent<Image> ().sprite = Barba [_indexCategoriaAcessorio];
				_changePlayer.cabelo.indexBarba = _indexCategoriaAcessorio;
			}
		} 
	}

	// Muda o item da categoria
	void LeftButton() {

		if (_indexItemCategoria > 0) {

			_indexItemCategoria -= 1;

			switch (_currentCategory) {

			case "Camisas": 
				gameObject.GetComponent<Image> ().sprite = Camisetas [_indexItemCategoria];
				ActivateItemBigPlayer (Camisetas [_indexItemCategoria].name, _camisaCategory, _changePlayer.camisa.cor, false);
				ActivateItemLittlePlayer(_camisaCategory, _changePlayer.camisa.cor, PlayerInGame);
				_changePlayer.camisa.index = _indexItemCategoria;

				break;
			case "Nariz":
				gameObject.GetComponent<Image> ().sprite = Narizes [_indexItemCategoria];
				ActivateItemBigPlayer (Narizes [_indexItemCategoria].name, _narizCategory, _changePlayer.nariz.cor, false);
				ActivateItemLittlePlayer(_narizCategory, _changePlayer.nariz.cor, PlayerInGame);
				_changePlayer.nariz.index = _indexItemCategoria;

				break;
			case "Boca":
				gameObject.GetComponent<Image> ().sprite = Bocas [_indexItemCategoria];
				ActivateItemBigPlayer (Bocas [_indexItemCategoria].name, _bocaCategory, _changePlayer.boca.cor, false);
				ActivateItemLittlePlayer(_bocaCategory, _changePlayer.boca.cor, PlayerInGame);
				_changePlayer.boca.index = _indexItemCategoria;
				if (_changePlayer.boca.boca.name == "cust1_9" && _changePlayer.boca.boca.name == "cust1_10") {
					ColorButton (0);
				}

				break;
			case "Cabelo":
				gameObject.GetComponent<Image> ().sprite = Cabelos [_indexItemCategoria];
				ActivateItemBigPlayer (Cabelos [_indexItemCategoria].name, _cabeloCategory, _changePlayer.cabelo.cor, false);
				ActivateItemLittlePlayer(_cabeloCategory, _changePlayer.cabelo.cor, PlayerInGame);
				_changePlayer.cabelo.index = _indexItemCategoria;
				ColorButton (_changePlayer.cabelo.indexCor);

				break;
			case "Calca":
				gameObject.GetComponent<Image> ().sprite = Calcas [_indexItemCategoria];
				ActivateItemBigPlayer (Calcas [_indexItemCategoria].name, _calcaCategory, _changePlayer.calca.cor, false);
				ActivateItemLittlePlayer(_calcaCategory, _changePlayer.calca.cor, PlayerInGame);
				_changePlayer.calca.index = _indexItemCategoria;

				break;
			case "Sapato":
				gameObject.GetComponent<Image> ().sprite = Sapatos [_indexItemCategoria];
				ActivateItemBigPlayer (Sapatos [_indexItemCategoria].name, _sapatoCategory, _changePlayer.sapato.cor, false);
				ActivateItemLittlePlayer(_sapatoCategory, _changePlayer.sapato.cor, PlayerInGame);
				_changePlayer.sapato.index = _indexItemCategoria;

				break;
			case "Acessorios":
				gameObject.GetComponent<Image> ().sprite = Acessorios [_indexItemCategoria];
				ActivateItemBigPlayer (Acessorios [_indexItemCategoria].name, _acessorioCategory, _changePlayer.acessorio.cor, false);
				ActivateItemLittlePlayer(_acessorioCategory, _changePlayer.acessorio.cor, PlayerInGame);
				_changePlayer.acessorio.index = _indexItemCategoria;

				break;
			case "Olhos":
				gameObject.GetComponent<Image> ().sprite = Olhos [_indexItemCategoria];
				ActivateItemBigPlayer (Olhos [_indexItemCategoria].name, _olhosCategory, _changePlayer.olhos.cor, false);
				ActivateItemLittlePlayer(_olhosCategory, _changePlayer.olhos.cor, PlayerInGame);
				_changePlayer.olhos.index = _indexItemCategoria;

				break;
			default:
				break;
			}
		}
	}

	// Muda o item da categoria
	void RightButton() {
		
		switch (_currentCategory) {

		case "Camisas":
			if (_indexItemCategoria < Camisetas.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Camisetas [_indexItemCategoria];
				ActivateItemBigPlayer (Camisetas [_indexItemCategoria].name, _camisaCategory, _changePlayer.camisa.cor, false);
				ActivateItemLittlePlayer(_camisaCategory, _changePlayer.camisa.cor, PlayerInGame);
				_changePlayer.camisa.index = _indexItemCategoria;
			}
			break;
		case "Nariz":
			if (_indexItemCategoria < Narizes.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Narizes [_indexItemCategoria];
				ActivateItemBigPlayer (Narizes [_indexItemCategoria].name, _narizCategory, _changePlayer.nariz.cor, false);
				ActivateItemLittlePlayer(_narizCategory, _changePlayer.nariz.cor, PlayerInGame);
				_changePlayer.nariz.index = _indexItemCategoria;
			}
			break;
		case "Boca":
			if (_indexItemCategoria < Bocas.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Bocas [_indexItemCategoria];
				ActivateItemBigPlayer (Bocas [_indexItemCategoria].name, _bocaCategory, _changePlayer.boca.cor, false);
				ActivateItemLittlePlayer(_bocaCategory, _changePlayer.boca.cor, PlayerInGame);
				_changePlayer.boca.index = _indexItemCategoria;
				if (_changePlayer.boca.boca.name == "cust1_9" && _changePlayer.boca.boca.name == "cust1_10") {
					ColorButton (0);
				}
			}
			break;
		case "Cabelo":
			if (_indexItemCategoria < Cabelos.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Cabelos [_indexItemCategoria];
				ActivateItemBigPlayer (Cabelos [_indexItemCategoria].name, _cabeloCategory, _changePlayer.cabelo.cor, false);
				ActivateItemLittlePlayer(_cabeloCategory, _changePlayer.cabelo.cor, PlayerInGame);
				_changePlayer.cabelo.index = _indexItemCategoria;
				ColorButton (_changePlayer.cabelo.indexCor);
			}
			break;
		case "Calca":
			if (_indexItemCategoria < Calcas.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Calcas [_indexItemCategoria];
				ActivateItemBigPlayer (Calcas [_indexItemCategoria].name, _calcaCategory, _changePlayer.calca.cor, false);
				ActivateItemLittlePlayer(_calcaCategory, _changePlayer.calca.cor, PlayerInGame);
				_changePlayer.calca.index = _indexItemCategoria;
			}
			break;
		case "Sapato":
			if (_indexItemCategoria < Sapatos.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Sapatos [_indexItemCategoria];
				ActivateItemBigPlayer (Sapatos [_indexItemCategoria].name, _sapatoCategory, _changePlayer.sapato.cor, false);
				ActivateItemLittlePlayer(_sapatoCategory, _changePlayer.sapato.cor, PlayerInGame);
				_changePlayer.sapato.index = _indexItemCategoria;
			}
			break;
		case "Acessorios":
			if (_indexItemCategoria < Acessorios.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Acessorios [_indexItemCategoria];
				ActivateItemBigPlayer (Acessorios [_indexItemCategoria].name, _acessorioCategory, _changePlayer.acessorio.cor, false);
				ActivateItemLittlePlayer(_acessorioCategory, _changePlayer.acessorio.cor, PlayerInGame);
				_changePlayer.acessorio.index = _indexItemCategoria;
			}
			break;
		case "Olhos":
			if (_indexItemCategoria < Olhos.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Olhos [_indexItemCategoria];
				ActivateItemBigPlayer (Olhos [_indexItemCategoria].name, _olhosCategory, _changePlayer.olhos.cor, false);
				ActivateItemLittlePlayer(_olhosCategory, _changePlayer.olhos.cor, PlayerInGame);
				_changePlayer.olhos.index = _indexItemCategoria;
			}
			break;
		default:
			break;
		}
	}

	void CategoryButton(int index) {

        switch (Categories [index].name) {

		case "Camisas":
			
			_currentCategory = "Camisas";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.camisa.camiseta;
			//if (Estampas.Length != 0)
				//Estampa.GetComponent<Image> ().sprite = _changePlayer.camisa.estampa;
			if (CorCamisetas.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.camisa.cor;
				ChangePalette (CorCamisetas);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.camisa.camiseta.name, _camisaCategory, _changePlayer.camisa.cor, true);
			//ActivateItemBigPlayer (_changePlayer.camisa.estampa.name, "Estampa", _changePlayer.camisa.cor, true);
			ActivateItemLittlePlayer(_camisaCategory, _changePlayer.camisa.cor, PlayerInGame);
			_indexItemCategoria = _changePlayer.camisa.index;
			_indexCategoriaAcessorio = _changePlayer.camisa.indexEstampa;

                break;

		case "Cor de pele":
			
			_currentCategory = "Cor de pele";
			gameObject.GetComponent<Image> ().sprite = Corpo[0];
			if (CorPele.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.corDePele.cor;
				ChangePalette (CorPele);
			}
			ActivatePattern ();
			ActivateItemLittlePlayer(_corDePeleCategory, _changePlayer.corDePele.cor, PlayerInGame);

            break;

		case "Nariz":
			
			_currentCategory = "Nariz";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.nariz.nariz;
			if (CorNariz.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.nariz.cor;
				ChangePalette (CorNariz);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.nariz.nariz.name, _narizCategory, _changePlayer.nariz.cor, true);
			ActivateItemLittlePlayer(_narizCategory, _changePlayer.nariz.cor, PlayerInGame);
			_indexItemCategoria = _changePlayer.nariz.index;

				break;

		case "Boca":
			
			_currentCategory = "Boca";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.boca.boca;
			if (CorBoca.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.boca.cor;
				ChangePalette (CorBoca);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.boca.boca.name, _bocaCategory, _changePlayer.boca.cor, true);
			ActivateItemLittlePlayer(_bocaCategory, _changePlayer.boca.cor, PlayerInGame);
			_indexItemCategoria = _changePlayer.boca.index;

                break;

		case "Cabelo":
			
			_currentCategory = "Cabelo";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.cabelo.cabelo;
			//if (Barba.Length != 0)
				//BarbaItem.GetComponent<Image> ().sprite = _changePlayer.cabelo.barba;
			if (CorCabelo.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.cabelo.cor;
				ChangePalette (CorCabelo);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.cabelo.cabelo.name, _cabeloCategory, _changePlayer.cabelo.cor, true);
			ActivateItemLittlePlayer (_cabeloCategory, _changePlayer.cabelo.cor, PlayerInGame);
			ColorButton (_changePlayer.cabelo.indexCor);
			_indexItemCategoria = _changePlayer.cabelo.index;
			_indexCategoriaAcessorio = _changePlayer.cabelo.indexBarba;

                break;

		case "Calca":

			_currentCategory = "Calca";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.calca.calca;
			if (CorCalca.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.calca.cor;
				ChangePalette (CorCalca);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.calca.calca.name, _calcaCategory, _changePlayer.calca.cor, true);
			ActivateItemLittlePlayer(_calcaCategory, _changePlayer.calca.cor, PlayerInGame);
			_indexItemCategoria = _changePlayer.calca.index;

                break;

		case "Sapato":
			
			_currentCategory = "Sapato";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.sapato.sapato;
			if (CorSapato.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.sapato.cor;
				ChangePalette (CorSapato);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.sapato.sapato.name, _sapatoCategory, _changePlayer.sapato.cor, true);
			ActivateItemLittlePlayer(_sapatoCategory, _changePlayer.sapato.cor, PlayerInGame);
			_indexItemCategoria = _changePlayer.sapato.index;

                break;

		case "Acessorios":
			
			_currentCategory = "Acessorios";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.acessorio.acessorio;
			if (CorAcessorio.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.acessorio.cor;
				ChangePalette (CorAcessorio);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.acessorio.acessorio.name, _acessorioCategory, _changePlayer.acessorio.cor, true);
			ActivateItemLittlePlayer(_acessorioCategory, _changePlayer.acessorio.cor, PlayerInGame);
			_indexItemCategoria = _changePlayer.acessorio.index;

                break;

		case "Olhos":
			
			_currentCategory = "Olhos";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.olhos.olhos;
			//if (Cilios.Length != 0)
				//CiliosItem.GetComponent<Image> ().sprite = _changePlayer.olhos.cilios;
			if (CorOlhos.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.olhos.cor;
				ChangePalette (CorOlhos);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.olhos.olhos.name, _olhosCategory, _changePlayer.olhos.cor, true);
			ActivateItemLittlePlayer(_olhosCategory, _changePlayer.olhos.cor, PlayerInGame);
			_indexItemCategoria = _changePlayer.olhos.index;
			_indexCategoriaAcessorio = _changePlayer.olhos.indexCilios;

                break;

		default:
                break;
        }
			
	}
		
	// Ativa os assets do player grande
	void ActivateItemBigPlayer(string item, string category, Color cor, bool firstTime){

		string lastItem;

		switch (category) {

		case "Camisas": 

			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == item) {

					foreach (Transform asset in bigPlayerObjs) {
						if (asset.transform.name == _changePlayer.camisa.camiseta.name) {
							asset.gameObject.SetActive (false);
						}
					}

					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
					_changePlayer.camisa.camiseta.name = obj.name;
				}
			}
				
			break;
		case "Nariz":

			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == item) {

					foreach (Transform asset in bigPlayerObjs) {
						if (asset.transform.name == _changePlayer.nariz.nariz.name) {
							asset.gameObject.SetActive (false);
						}
					}

					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
					_changePlayer.nariz.nariz.name = obj.name;
				}
			}

			break;
		case "Boca":

			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == item) {

					foreach (Transform asset in bigPlayerObjs) {
						if (asset.transform.name == _changePlayer.boca.boca.name) {
							asset.gameObject.SetActive (false);
						}
					}

					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
					_changePlayer.boca.boca.name = obj.name;
				}
			}

			break;
		case "Cor de pele":
			PlayerCustomizable.transform.Find ("cust2_0").Find (item).gameObject.SetActive (true);
			PlayerCustomizable.transform.Find ("arms").Find (item).gameObject.SetActive (true);
			PlayerCustomizable.transform.Find ("cust1_4").Find (item).gameObject.SetActive (true);

			break;
		case "Cabelo":

			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == item) {

					foreach (Transform asset in bigPlayerObjs) {
						if (asset.transform.name == _changePlayer.cabelo.cabelo.name) {
							asset.gameObject.SetActive (false);
						}
					}

					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
					_changePlayer.cabelo.cabelo.name = obj.name;
				}
			}

			break;
		case "Calca":

			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == item) {

					foreach (Transform asset in bigPlayerObjs) {
						if (asset.transform.name == _changePlayer.calca.calca.name) {
							asset.gameObject.SetActive (false);
						}
					}

					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
					_changePlayer.calca.calca.name = obj.name;
				}
			}

			break;
		case "Sapato":

			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == item) {

					foreach (Transform asset in bigPlayerObjs) {
						if (asset.transform.name == _changePlayer.sapato.sapato.name) {
							asset.gameObject.SetActive (false);
						}
					}

					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
					_changePlayer.sapato.sapato.name = obj.name;
				}
			}

			break;
		case "Acessorios":

			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == item) {

					foreach (Transform asset in bigPlayerObjs) {
						if (asset.transform.name == _changePlayer.acessorio.acessorio.name) {
							asset.gameObject.SetActive (false);
						}
					}

					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
					_changePlayer.acessorio.acessorio.name = obj.name;
				}
			}

			break;
		case "Olhos":
			
			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == item) {

					foreach (Transform asset in bigPlayerObjs) {
						if (asset.transform.name == _changePlayer.olhos.olhos.name) {
							asset.gameObject.SetActive (false);
						}
					}

					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
					_changePlayer.olhos.olhos.name = obj.name;
				}
			}

			break;

		case "Estampa":
			
			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == item) {



					foreach (Transform asset in bigPlayerObjs) {
						if (asset.transform.name == _changePlayer.camisa.estampa.name) {
							asset.gameObject.SetActive (false);
						}
					}

					obj.gameObject.SetActive (true);
					_changePlayer.camisa.estampa.name = obj.name;
				}
			}

			break;
		default:
			break;
		}
	}

	static public void SetPlayer(GameObject player){

		ActivateItemLittlePlayer(_camisaCategory, _changePlayer.camisa.cor , player.transform.Find("Mesh").gameObject);
		ActivateItemLittlePlayer(_corDePeleCategory, _changePlayer.corDePele.cor , player.transform.Find("Mesh").gameObject);
		ActivateItemLittlePlayer(_cabeloCategory, _changePlayer.cabelo.cor , player.transform.Find("Mesh").gameObject);
		ActivateItemLittlePlayer(_calcaCategory, _changePlayer.calca.cor , player.transform.Find("Mesh").gameObject);
		ActivateItemLittlePlayer(_sapatoCategory, _changePlayer.sapato.cor , player.transform.Find("Mesh").gameObject);

	}

	// Ativa os assets do player pequeno
	static public void ActivateItemLittlePlayer(string category, Color cor, GameObject PlayerMesh){

		switch (category) {

		case "Camisas": 

			foreach (Transform child in PlayerMesh.GetComponentsInChildren<Transform>(true)){

				if (child.name == "blusas"){


					if (_changePlayer.camisa.camiseta.name == "cust1_0"){

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)){

							if (item.name == "curta1" || item.name == "curta2" || 
								item.name == "longa1" || item.name == "longa2") {

								item.gameObject.SetActive(false);

							} else if (item.name == "base"){
								item.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
							}
						}

					} else if (_changePlayer.camisa.camiseta.name == "cust1_1") {

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)){

							if (item.name == "longa1" || item.name == "longa2") {

								item.gameObject.SetActive(false);

							} else if (item.name == "base" || item.name == "curta1" || item.name == "curta2"){
								item.gameObject.SetActive(true);
								item.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
							}
						}

					} else if (_changePlayer.camisa.camiseta.name == "cust1_21") {

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)){

							if (item.name == "curta1" || item.name == "curta2") {

								item.gameObject.SetActive(false);

							} else if (item.name == "base" || item.name == "longa1" || item.name == "longa2"){
								item.gameObject.SetActive(true);
								item.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
							}
						}

					}

				}
			}


			break;
		case "Cor de pele":


			foreach (Transform child in PlayerMesh.GetComponentsInChildren<Transform>(true)){

				if (child.name == "corpo"){
					child.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
				} else if (child.name == "blusas") {

					foreach (Transform item in child.GetComponentInChildren<Transform>(true)){

						if (item.name == "braco1" || item.name == "braco2") {
							item.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
						} 
					}
				}
			}

			break;
		case "Cabelo":

			foreach (Transform child in PlayerMesh.GetComponentsInChildren<Transform>(true)){

				if (child.name == "cabelo"){

					if (_changePlayer.camisa.camiseta.name == "thumb_hairs_0" ||
					    _changePlayer.camisa.camiseta.name == "thumb_hairs2_1_3") {

						child.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)) {

							if (item.name == "miniCabelos_0" || item.name == "miniCabelos_2") {

								item.gameObject.SetActive (false);

							} else if (item.name == "miniCabelos_1") {
								item.gameObject.SetActive (true);
								item.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
							}
						}

					} else if (_changePlayer.camisa.camiseta.name == "thumb_hairs_1" ||
					           _changePlayer.camisa.camiseta.name == "thumb_hairs2_1_2") {

						child.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)) {

							if (item.name == "miniCabelos_1" || item.name == "miniCabelos_2") {

								item.gameObject.SetActive (false);

							} else if (item.name == "miniCabelos_0") {
								item.gameObject.SetActive (true);
								item.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
							}
						}

					} else if (_changePlayer.camisa.camiseta.name == "thumb_hairs_3") {

						child.GetComponent<Anima2D.SpriteMeshInstance> ().color = _changePlayer.corDePele.cor;

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)) {

							if (item.name == "miniCabelos_0" || item.name == "miniCabelos_1" ||
								item.name == "miniCabelos_2") {

								item.gameObject.SetActive (false);
							} 
						}

					} else if (_changePlayer.camisa.camiseta.name == "thumb_hairs_2" ||
					           _changePlayer.camisa.camiseta.name == "thumb_hairs2_1_0") {

						child.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)) {

							if (item.name == "miniCabelos_0" || item.name == "miniCabelos_1" ||
								item.name == "miniCabelos_2") {

								item.gameObject.SetActive (false);
							}
						}

					} else if (_changePlayer.cabelo.cabelo.name == "thumb_hairs2_1_4" ||
						_changePlayer.camisa.camiseta.name == "thumb_hairs2_1_5") {

						child.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)) {

							if (item.name == "miniCabelos_0" || item.name == "miniCabelos_1") {

								item.gameObject.SetActive (false);

							} else if (item.name == "miniCabelos_2") {
								item.gameObject.SetActive (true);
								item.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
							}
						}
					}
				}
			}

			break;
		case "Calca":

			foreach (Transform child in PlayerMesh.GetComponentsInChildren<Transform>(true)){

				if (child.name == "calca"){

					if (_changePlayer.calca.calca.name == "cust1_3"){

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)){

							if (item.name == "calca1") {
								item.gameObject.SetActive(false);
							} else if (item.name == "calca3"){
								item.gameObject.SetActive(true);
								item.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
							}
						}

					} else if (_changePlayer.calca.calca.name == "cust4_3"){

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)) {

							if (item.name == "calca3") {
								item.gameObject.SetActive (false);
							} else if (item.name == "calca1") {
								item.gameObject.SetActive(true);
								item.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
							}
						}
					}
				}
			}

			break;
		case "Sapato":

			foreach (Transform child in PlayerMesh.GetComponentsInChildren<Transform>(true)){

				if (child.name == "sapato"){
					foreach (Transform item in child.GetComponentInChildren<Transform>(true)){

						item.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
					}
				}
			}

			break;
		default:
			break;
		}
	}


	// Muda os cor do sprite
	void ColorButton(int index) {

		if (_currentCategory != _bocaCategory && _currentCategory != _cabeloCategory) {
			gameObject.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
		} else if (_currentCategory == _bocaCategory) {
			if (_changePlayer.boca.boca.name != "cust1_9" && _changePlayer.boca.boca.name != "cust1_10") {
				gameObject.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
			} else {
				gameObject.GetComponent<Image> ().color = Color [0].GetComponent<Image> ().color;
			}
		} else if (_currentCategory == _cabeloCategory) {
			if (_changePlayer.boca.boca.name != "thumb_hairs_3") {
				gameObject.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
			} 
			BarbaItem.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
		}

		switch (_currentCategory) {

		case "Camisas":
			_changePlayer.camisa.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.camisa.indexCor = index;

			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.camisa.camiseta.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}

			ActivateItemLittlePlayer(_camisaCategory, _changePlayer.camisa.cor, PlayerInGame);

			break;
		case "Nariz":
			_changePlayer.nariz.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.nariz.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.nariz.nariz.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}

			ActivateItemLittlePlayer(_narizCategory, _changePlayer.nariz.cor, PlayerInGame);

			break;
		case "Acessorios":
			_changePlayer.acessorio.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.acessorio.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.acessorio.acessorio.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}

			ActivateItemLittlePlayer(_acessorioCategory, _changePlayer.acessorio.cor, PlayerInGame);

			break;
		case "Boca":
			
			if (_changePlayer.boca.boca.name != "cust1_9" && _changePlayer.boca.boca.name != "cust1_10") {
				_changePlayer.boca.cor = Color [index].GetComponent<Image> ().color;
				_changePlayer.boca.indexCor = index;
			} else {
				_changePlayer.boca.cor = Color [0].GetComponent<Image> ().color;
				_changePlayer.boca.indexCor = 0;
			}
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.boca.boca.name) {

					if (_changePlayer.boca.boca.name != "cust1_9" && _changePlayer.boca.boca.name != "cust1_10") {
						obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
					} else {
						obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [0].GetComponent<Image> ().color;
					}
				}
			}

			ActivateItemLittlePlayer(_bocaCategory, _changePlayer.boca.cor, PlayerInGame);

			break;
		case "Cabelo":
			BarbaItem.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
			Sombrancelha.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
			_changePlayer.cabelo.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.cabelo.indexCor = index;

			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.cabelo.cabelo.name) {

					if (_changePlayer.cabelo.cabelo.name == "thumb_hairs_3") {
						obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = _changePlayer.corDePele.cor;
					} else {
						obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
						if (obj.childCount > 0) {
							obj.GetChild (0).GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
						}
					}
				}
				if (obj.transform.name == _changePlayer.cabelo.barba.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
				if (obj.transform.name == "cust4_1") {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}

			ActivateItemLittlePlayer(_cabeloCategory, _changePlayer.cabelo.cor, PlayerInGame);

			break;
		case "Calca":
			_changePlayer.calca.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.calca.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.calca.calca.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}
			ActivateItemLittlePlayer(_calcaCategory, _changePlayer.calca.cor, PlayerInGame);

			break;
		case "Olhos":
			_changePlayer.olhos.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.olhos.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.olhos.olhos.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}
			ActivateItemLittlePlayer(_olhosCategory, _changePlayer.olhos.cor, PlayerInGame);

			break;
		case "Sapato":
			_changePlayer.sapato.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.sapato.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.sapato.sapato.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}
			ActivateItemLittlePlayer(_sapatoCategory, _changePlayer.sapato.cor, PlayerInGame);

			break;
		case "Cor de pele":
			_changePlayer.corDePele.cor = Color [index].GetComponent<Image> ().color;

			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == "cust2_0") {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
				if (obj.transform.name == "arms") {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
				if (obj.transform.name == "cust1_4") {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
				if (obj.transform.name == "thumb_hairs_3") {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}

			ActivateItemLittlePlayer(_corDePeleCategory, _changePlayer.corDePele.cor, PlayerInGame);

			break;
		default:
			break;
		}
	}
		
	// ajeita o numero de cores que aparecem no display de cores e o tamanho do scroll
	void ChangePalette(Color[] colors){

		// se o numero de cores for menor que o numero de botoes
		if (colors.Length < _numberColorsActive) {
			for (int i = colors.Length; i < Color.Length; i++)
				Color [i].gameObject.SetActive (false);
			
		// se o numero de cores for maior que o numero de botoes
		} else if (colors.Length > _numberColorsActive) {
			for (int i = 0 ; i < colors.Length; i++)
				Color [i].gameObject.SetActive (true);
		}

		Vector2 ColorPosIni = _ColorScroll.GetChild (0).transform.position;

		float altura = Mathf.Abs(Color [colors.Length - 1].transform.localPosition.y - 202);
		_ColorScroll.sizeDelta = new Vector2 (_ColorScroll.sizeDelta.x, altura);

		_ColorScroll.GetChild (0).position = ColorPosIni;

		_numberColorsActive = colors.Length;

		for (int i = 0; i < colors.Length; i++) {
			Color [i].GetComponent<Image> ().color = colors [i];
		}

	}

	// tira o alpha da estampa e ou cílio
	void ActivatePattern(){
		
		if (_currentCategory == "Camisas") {

			if (Barba.Length != 0) BarbaItem.SetActive (false);
			if (Cilios.Length != 0) CiliosItem.SetActive (false);
			//if (Estampas.Length != 0) Estampa.SetActive (true);

		} else if (_currentCategory == "Olhos") {

			if (Estampas.Length != 0) Estampa.SetActive (false);
			if (Barba.Length != 0) BarbaItem.SetActive (false);
			//if (Cilios.Length != 0) CiliosItem.SetActive (true);

		} else if (_currentCategory == "Cabelo") {

			if (Estampas.Length != 0) Estampa.SetActive (false);
			if (Cilios.Length != 0) CiliosItem.SetActive (false);
			//if (Barba.Length != 0) BarbaItem.SetActive (true);


		} else {
			
			if (Estampas.Length != 0) Estampa.SetActive (false);
			if (Cilios.Length != 0) CiliosItem.SetActive (false);
			if (Barba.Length != 0) BarbaItem.SetActive (false);

		}

	}

}
