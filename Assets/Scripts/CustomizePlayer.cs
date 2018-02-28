using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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

	//public GameObject[] EyesFix;

	private Transform[] bigPlayerObjs;

	private ScrollRect _CategoryScroll;
	private RectTransform _ColorScroll;

	private int _numberColorsActive = 5;
	private string _currentCategory;
	private int _indexItemCategoria = 0;
	private int _indexCategoriaAcessorio = 0;

	private const string _camisaCategory = "Camisas";
	private const string _olhosCategory = "Olhos";
	private const string _cabeloCategory = "Cabelo";
	private const string _narizCategory = "Nariz";
	private const string _bocaCategory = "Boca";
	private const string _calcaCategory = "Calca";
	private const string _sapatoCategory = "Sapato";
	private const string _acessorioCategory = "Acessorios";
	private const string _corDePeleCategory = "Cor de pele";

	private struct Camisa {

		public Sprite camiseta;
		public Sprite estampa;
		public Color cor;
		public int index;
		public int indexEstampa;
		public int indexCor;

		public Camisa (Sprite camiseta, Sprite estampa, Color cor, int index, int indexEstampa, int indexCor){
			this.camiseta = camiseta;
			this.estampa = estampa;
			this.cor = cor;
			this.index = index;
			this.indexEstampa = indexEstampa;
			this.indexCor = indexCor;
		}
	}

	private struct Olho {

		public Sprite olhos;
		public Sprite cilios;
		public Color cor;
		public int index;
		public int indexCilios;
		public int indexCor;

		public Olho (Sprite olhos, Sprite cilios, Color cor, int index, int indexCilios, int indexCor){
			this.olhos = olhos;
			this.cilios = cilios;
			this.cor = cor;
			this.index = index;
			this.indexCilios = indexCilios;
			this.indexCor = indexCor;
		}
	}

	private struct Cabelo {

		public Sprite cabelo;
		public Sprite barba;
		public Color cor;
		public int index;
		public int indexBarba;
		public int indexCor;

		public Cabelo (Sprite cabelo, Sprite barba, Color cor, int index, int indexBarba, int indexCor){
			this.cabelo = cabelo;
			this.barba = barba;
			this.cor = cor;
			this.index = index;
			this.indexBarba = indexBarba;
			this.indexCor = indexCor;
		}
	}

	private struct CorDePele {

		public Sprite corpo;
		public Color cor;
		public int indexCor;

		public CorDePele (Sprite corpo, Color cor, int indexCor){
			this.corpo = corpo;
			this.cor = cor;
			this.indexCor = indexCor;
		}
	}

	private struct Nariz {

		public Sprite nariz;
		public Color cor;
		public int index;
		public int indexCor;

		public Nariz (Sprite nariz, Color cor, int index, int indexCor){
			this.nariz = nariz;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	private struct Boca {

		public Sprite boca;
		public Color cor;
		public int index;
		public int indexCor;

		public Boca (Sprite boca, Color cor, int index, int indexCor){
			this.boca = boca;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	private struct Calca {

		public Sprite calca;
		public Color cor;
		public int index;
		public int indexCor;

		public Calca (Sprite calca, Color cor, int index, int indexCor){
			this.calca = calca;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	private struct Sapato {

		public Sprite sapato;
		public Color cor;
		public int index;
		public int indexCor;

		public Sapato (Sprite sapato, Color cor, int index, int indexCor){
			this.sapato = sapato;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	private struct Acessorio {

		public Sprite acessorio;
		public Color cor;
		public int index;
		public int indexCor;

		public Acessorio (Sprite acessorio, Color cor, int index, int indexCor){
			this.acessorio = acessorio;
			this.cor = cor;
			this.index = index;
			this.indexCor = indexCor;
		}
	}

	private struct Customize {

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

	private Customize _changePlayer;
	private SpriteMetaData[] sliceMetaData;

	/*private Camisa _camisa;
	private Olho _olhos;
	private Cabelo _cabelo;
	private CorDePele _corDePele;
	private Nariz _nariz;
	private Boca _boca;
	private Calca _calca;
	private Sapato _sapato;
	private Acessorio _acessorio;*/


	// ativar toda a struct do player com os itens iniciais, (ativar somente eles e deixar desativado todos os outros)
	// quando for trocar o item, pegar o item original da struct do player, desativar ele, receber a nova imagem, 
	// ativar ela e setar a nova imagem como a magem do player na struct

	// Use this for initialization
	void Start () {

		AdjustSprites ();

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

		//_CategoryScroll = Categories [0].transform.parent.transform.parent.transform.parent.GetComponent<ScrollRect>();
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
		//ActivateItemBigPlayer (_changePlayer.camisa.estampa.name, "Camisas");
		ActivateItemBigPlayer (_changePlayer.olhos.olhos.name, _olhosCategory, _changePlayer.olhos.cor, true);
		//ActivateItemBigPlayer (_changePlayer.olhos.cilios.name);
		ActivateItemBigPlayer (_changePlayer.cabelo.cabelo.name, _cabeloCategory, _changePlayer.cabelo.cor, true);
		//ActivateItemBigPlayer (_changePlayer.cabelo.barba.name);
		ActivateItemBigPlayer (_changePlayer.nariz.nariz.name, _narizCategory, _changePlayer.nariz.cor, true);
		ActivateItemBigPlayer (_changePlayer.boca.boca.name, _bocaCategory, _changePlayer.boca.cor, true);
		ActivateItemBigPlayer (_changePlayer.calca.calca.name, _calcaCategory, _changePlayer.calca.cor, true);
		ActivateItemBigPlayer (_changePlayer.sapato.sapato.name, _sapatoCategory, _changePlayer.sapato.cor, true);
		ActivateItemBigPlayer (_changePlayer.acessorio.acessorio.name, _acessorioCategory, _changePlayer.acessorio.cor, true);

		ActivateItemLittlePlayer(_camisaCategory, _changePlayer.camisa.cor);
		ActivateItemLittlePlayer(_olhosCategory, _changePlayer.olhos.cor);
		ActivateItemLittlePlayer(_cabeloCategory, _changePlayer.cabelo.cor);
		ActivateItemLittlePlayer(_narizCategory, _changePlayer.nariz.cor);
		ActivateItemLittlePlayer(_bocaCategory, _changePlayer.boca.cor);
		ActivateItemLittlePlayer(_calcaCategory, _changePlayer.calca.cor);
		ActivateItemLittlePlayer(_sapatoCategory, _changePlayer.sapato.cor);
		ActivateItemLittlePlayer(_acessorioCategory, _changePlayer.acessorio.cor);


		// cust4_1 - sobrancelha
		// cust2_0 - cabeca careca
		// arms - bracos
		// barba
		// olhos
		// nariz
		// boca
		// cabelos
		// estampasDesenho
		// estampasCor
		// blusas
		// calcas
		// acessorios
		// sapato



		//Debug.Log (Camisetas[0].name);

		//Debug.Log(PlayerCustomizable.transform.Find ("blusas").Find(Camisetas[0].name));

	}


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
			
			print (item.name);
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
	
	// Update is called once per frame
	void Update () {
		
		// veficar se o retandogulo do botao inclui o centro da tela e executar a acao do botao

		gameObject.GetComponent<Image> ().SetNativeSize ();

	}

	// Muda a estampa/cilios
    void UpButton() {

		if (_currentCategory == "Camisas") {
			if (_indexCategoriaAcessorio < Estampas.Length - 1) {
				_indexCategoriaAcessorio += 1;
				Estampa.GetComponent<Image> ().sprite = Estampas [_indexCategoriaAcessorio];
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
				_changePlayer.camisa.index = _indexItemCategoria;

				break;
			case "Nariz":
				gameObject.GetComponent<Image> ().sprite = Narizes [_indexItemCategoria];
				ActivateItemBigPlayer (Narizes [_indexItemCategoria].name, _narizCategory, _changePlayer.nariz.cor, false);
				_changePlayer.nariz.index = _indexItemCategoria;

				break;
			case "Boca":
				gameObject.GetComponent<Image> ().sprite = Bocas [_indexItemCategoria];
				ActivateItemBigPlayer (Bocas [_indexItemCategoria].name, _bocaCategory, _changePlayer.boca.cor, false);
				_changePlayer.boca.index = _indexItemCategoria;

				break;
			case "Cabelo":
				gameObject.GetComponent<Image> ().sprite = Cabelos [_indexItemCategoria];
				ActivateItemBigPlayer (Cabelos [_indexItemCategoria].name, _cabeloCategory, _changePlayer.cabelo.cor, false);
				_changePlayer.cabelo.index = _indexItemCategoria;

				break;
			case "Calca":
				gameObject.GetComponent<Image> ().sprite = Calcas [_indexItemCategoria];
				ActivateItemBigPlayer (Calcas [_indexItemCategoria].name, _calcaCategory, _changePlayer.calca.cor, false);
				_changePlayer.calca.index = _indexItemCategoria;

				break;
			case "Sapato":
				gameObject.GetComponent<Image> ().sprite = Sapatos [_indexItemCategoria];
				ActivateItemBigPlayer (Sapatos [_indexItemCategoria].name, _sapatoCategory, _changePlayer.sapato.cor, false);
				_changePlayer.sapato.index = _indexItemCategoria;

				break;
			case "Acessorios":
				gameObject.GetComponent<Image> ().sprite = Acessorios [_indexItemCategoria];
				ActivateItemBigPlayer (Acessorios [_indexItemCategoria].name, _acessorioCategory, _changePlayer.acessorio.cor, false);
				_changePlayer.acessorio.index = _indexItemCategoria;

				break;
			case "Olhos":
				gameObject.GetComponent<Image> ().sprite = Olhos [_indexItemCategoria];
				ActivateItemBigPlayer (Olhos [_indexItemCategoria].name, _olhosCategory, _changePlayer.olhos.cor, false);
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
				_changePlayer.camisa.index = _indexItemCategoria;
			}
			break;
		case "Nariz":
			if (_indexItemCategoria < Narizes.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Narizes [_indexItemCategoria];
				ActivateItemBigPlayer (Narizes [_indexItemCategoria].name, _narizCategory, _changePlayer.nariz.cor, false);
				_changePlayer.nariz.index = _indexItemCategoria;
			}
			break;
		case "Boca":
			if (_indexItemCategoria < Bocas.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Bocas [_indexItemCategoria];
				ActivateItemBigPlayer (Bocas [_indexItemCategoria].name, _bocaCategory, _changePlayer.boca.cor, false);
				_changePlayer.boca.index = _indexItemCategoria;
			}
			break;
		case "Cabelo":
			if (_indexItemCategoria < Cabelos.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Cabelos [_indexItemCategoria];
				ActivateItemBigPlayer (Cabelos [_indexItemCategoria].name, _cabeloCategory, _changePlayer.cabelo.cor, false);
				_changePlayer.cabelo.index = _indexItemCategoria;
			}
			break;
		case "Calca":
			if (_indexItemCategoria < Calcas.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Calcas [_indexItemCategoria];
				ActivateItemBigPlayer (Calcas [_indexItemCategoria].name, _calcaCategory, _changePlayer.calca.cor, false);
				_changePlayer.calca.index = _indexItemCategoria;
			}
			break;
		case "Sapato":
			if (_indexItemCategoria < Sapatos.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Sapatos [_indexItemCategoria];
				ActivateItemBigPlayer (Sapatos [_indexItemCategoria].name, _sapatoCategory, _changePlayer.sapato.cor, false);
				_changePlayer.sapato.index = _indexItemCategoria;
			}
			break;
		case "Acessorios":
			if (_indexItemCategoria < Acessorios.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Acessorios [_indexItemCategoria];
				ActivateItemBigPlayer (Acessorios [_indexItemCategoria].name, _acessorioCategory, _changePlayer.acessorio.cor, false);
				_changePlayer.acessorio.index = _indexItemCategoria;
			}
			break;
		case "Olhos":
			if (_indexItemCategoria < Olhos.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Olhos [_indexItemCategoria];
				ActivateItemBigPlayer (Olhos [_indexItemCategoria].name, _olhosCategory, _changePlayer.olhos.cor, false);
				_changePlayer.olhos.index = _indexItemCategoria;
			}
			break;
		default:
			break;
		}
	}

	void CategoryButton(int index) {

        // pega o nome da categoria e muda os itens relacionados a ele

        switch (Categories [index].name) {

		case "Camisas":
			
			_currentCategory = "Camisas";
				// mudar sempre pra camisa e cor que estava antes
			gameObject.GetComponent<Image> ().sprite = _changePlayer.camisa.camiseta;
			if (Estampas.Length != 0)
				Estampa.GetComponent<Image> ().sprite = _changePlayer.camisa.estampa;
                // dar um jeito de adicionar a estampa ao sprite da camiseta
			if (CorCamisetas.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.camisa.cor;
				ChangePalette (CorCamisetas);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.camisa.camiseta.name, _camisaCategory, _changePlayer.camisa.cor, true);
			_indexItemCategoria = _changePlayer.camisa.index;
			_indexCategoriaAcessorio = _changePlayer.camisa.indexEstampa;

				// setar a paleta de cores com as cores disponíveis da camisa
				// ativar apenas a camisa no personagem
                break;

		case "Cor de pele":
			
			_currentCategory = "Cor de pele";
			gameObject.GetComponent<Image> ().sprite = Corpo[0];
			if (CorPele.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.corDePele.cor;
				ChangePalette (CorPele);
			}
			ActivatePattern ();
			//ActivateItemBigPlayer (_changePlayer.corDePele.cor);
			//_indexItemCategoria = _changePlayer.corDePele.indexCor;
			//_indexCategoriaAcessorio = 0;

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

			_indexItemCategoria = _changePlayer.nariz.index;
			//_indexCategoriaAcessorio = _changePlayer.nariz.indexCor;

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
			_indexItemCategoria = _changePlayer.boca.index;
			//_indexCategoriaAcessorio = 0;

                break;

		case "Cabelo":
			
			_currentCategory = "Cabelo";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.cabelo.cabelo;
			if (Barba.Length != 0)
				BarbaItem.GetComponent<Image> ().sprite = _changePlayer.cabelo.barba;
			if (CorCabelo.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.cabelo.cor;
				ChangePalette (CorCabelo);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.cabelo.cabelo.name, _cabeloCategory, _changePlayer.cabelo.cor, true);
			_indexItemCategoria = _changePlayer.cabelo.index;
			_indexCategoriaAcessorio = _changePlayer.cabelo.indexBarba;

                break;

		case _calcaCategory:

			_currentCategory = "Calca";
			print (_changePlayer.calca.calca);
			gameObject.GetComponent<Image> ().sprite = _changePlayer.calca.calca;
			if (CorCalca.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.calca.cor;
				ChangePalette (CorCalca);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.calca.calca.name, _calcaCategory, _changePlayer.calca.cor, true);

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
			_indexItemCategoria = _changePlayer.acessorio.index;

                break;

		case "Olhos":
			
			_currentCategory = "Olhos";
			gameObject.GetComponent<Image> ().sprite = _changePlayer.olhos.olhos;
			if (Cilios.Length != 0)
				CiliosItem.GetComponent<Image> ().sprite = _changePlayer.olhos.cilios;
			if (CorOlhos.Length != 0) {
				gameObject.GetComponent<Image> ().color = _changePlayer.olhos.cor;
				ChangePalette (CorOlhos);
			}
			ActivatePattern ();
			ActivateItemBigPlayer (_changePlayer.olhos.olhos.name, _olhosCategory, _changePlayer.olhos.cor, true);
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

			lastItem = _changePlayer.camisa.camiseta.name;

			foreach (Transform obj in bigPlayerObjs){
				if (obj.transform.name == lastItem) {

					obj.gameObject.SetActive (false);

				} 
				if (obj.transform.name == item) {

					_changePlayer.camisa.camiseta.name = item;
					obj.gameObject.SetActive (true);


					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
				}
			}

			//PlayerCustomizable.transform.Find ("estampasDesenhos").Find (_changePlayer.camisa.estampa.name).gameObject.SetActive (false);
			//PlayerCustomizable.transform.Find ("estampasDesenhos").Find (item).gameObject.SetActive (true);

			break;
		case "Nariz":

			lastItem = _changePlayer.nariz.nariz.name;

			foreach (Transform obj in bigPlayerObjs){
				if (obj.transform.name == lastItem) {

					obj.gameObject.SetActive (false);

				} 
				if (obj.transform.name == item) {

					_changePlayer.nariz.nariz.name = item;
					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
				}
			}

			break;
		case "Boca":

			lastItem = _changePlayer.boca.boca.name;

			foreach (Transform obj in bigPlayerObjs){
				if (obj.transform.name == lastItem) {

					obj.gameObject.SetActive (false);

				} 
				if (obj.transform.name == item) {

					_changePlayer.boca.boca.name = item;
					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
				}
			}

			break;
		case "Cor de pele":
			PlayerCustomizable.transform.Find ("cust2_0").Find (item).gameObject.SetActive (true);
			PlayerCustomizable.transform.Find ("arms").Find (item).gameObject.SetActive (true);
			PlayerCustomizable.transform.Find ("cust1_4").Find (item).gameObject.SetActive (true);
			//obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;

			//PlayerCustomizable.transform.Find ("cust2_0").Find (_changePlayer.camisa.camiseta.name).gameObject.SetActive (true);
			break;
		case "Cabelo":

			lastItem = _changePlayer.cabelo.cabelo.name;

			foreach (Transform obj in bigPlayerObjs){
				if (obj.transform.name == lastItem) {

					obj.gameObject.SetActive (false);

				} 
				if (obj.transform.name == item) {

					_changePlayer.cabelo.cabelo.name = item;
					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;

					if (obj.childCount > 0) {
						obj.GetChild (0).gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
					}
				}
			}

			break;
		case "Calca":

			lastItem = _changePlayer.calca.calca.name;

			foreach (Transform obj in bigPlayerObjs){
				if (obj.transform.name == lastItem) {

					obj.gameObject.SetActive (false);

				} 
				if (obj.transform.name == item) {

					_changePlayer.calca.calca.name = item;
					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
				}
			}

			break;
		case "Sapato":

			lastItem = _changePlayer.sapato.sapato.name;

			foreach (Transform obj in bigPlayerObjs){
				if (obj.transform.name == lastItem) {

					obj.gameObject.SetActive (false);

				} 
				if (obj.transform.name == item) {

					_changePlayer.sapato.sapato.name = item;
					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
				}
			}

			break;
		case "Acessorios":

			lastItem = _changePlayer.acessorio.acessorio.name;

			foreach (Transform obj in bigPlayerObjs){
				if (obj.transform.name == lastItem) {

					obj.gameObject.SetActive (false);

				} 
				if (obj.transform.name == item) {

					_changePlayer.acessorio.acessorio.name = item;
					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
				}
			}

			break;
		case "Olhos":

			print (_changePlayer.olhos.olhos.name + "OLHO");
			print (item + "ITEM OLHO");


			lastItem = _changePlayer.olhos.olhos.name;

			foreach (Transform obj in bigPlayerObjs) {
				if (obj.transform.name == _changePlayer.olhos.olhos.name)
					obj.gameObject.SetActive (false);
			}

			foreach (Transform obj in bigPlayerObjs){

				/*if (obj.transform.name == lastItem && !firstTime) {
					
						print("Desativa");

						obj.gameObject.SetActive (false);

				} else*/ if (obj.transform.name == item) {

					print("Troca");
					_changePlayer.olhos.olhos.name = item;
					obj.gameObject.SetActive (true);
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
				}
			}

			break;
		default:
			break;
		}
	}

	// Ativa os assets do player pequeno
	void ActivateItemLittlePlayer(string category, Color cor){

		switch (category) {

		case "Camisas": 

			foreach (Transform child in PlayerInGame.GetComponentsInChildren<Transform>(true)){

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
								item.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
							}
						}

					} else if (_changePlayer.camisa.camiseta.name == "cust1_21") {

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)){

							if (item.name == "curta1" || item.name == "curta2") {

								item.gameObject.SetActive(false);

							} else if (item.name == "base" || item.name == "longa1" || item.name == "longa2"){
								item.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
							}
						}

					}

				}
			}


			break;
		case "Cor de pele":

			foreach (Transform child in PlayerInGame.GetComponentsInChildren<Transform>(true)){

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

			foreach (Transform child in PlayerInGame.GetComponentsInChildren<Transform>(true)){

				if (child.name == "cabelo"){

				}
			}

			break;
		case "Calca":

			foreach (Transform child in PlayerInGame.GetComponentsInChildren<Transform>(true)){

				if (child.name == "calca"){

					if (_changePlayer.calca.calca.name == "cust1_3"){

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)){

							if (item.name == "calca1") {
								item.gameObject.SetActive(false);
							} else if (item.name == "calca3"){
								item.GetComponent<Anima2D.SpriteMeshInstance>().color = cor;
							}
						}

					} else if (_changePlayer.calca.calca.name == "cust4_3"){

						foreach (Transform item in child.GetComponentInChildren<Transform>(true)) {

							if (item.name == "calca3") {
								item.gameObject.SetActive (false);
							} else if (item.name == "calca1") {
								item.GetComponent<Anima2D.SpriteMeshInstance> ().color = cor;
							}
						}
					}
				}
			}

			break;
		case "Sapato":

			foreach (Transform child in PlayerInGame.GetComponentsInChildren<Transform>(true)){

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

		if (_currentCategory != _bocaCategory) {
			gameObject.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
		} else {
			if (_changePlayer.boca.boca.name != "cust1_9" && _changePlayer.boca.boca.name != "cust1_10") {
				gameObject.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
			} else {
				gameObject.GetComponent<Image> ().color = Color [0].GetComponent<Image> ().color;
			}
		}

		switch (_currentCategory) {

		case _camisaCategory:
			_changePlayer.camisa.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.camisa.indexCor = index;

			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.camisa.camiseta.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}

			break;
		case _narizCategory:
			_changePlayer.nariz.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.nariz.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.nariz.nariz.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}

			break;
		case _acessorioCategory:
			_changePlayer.acessorio.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.acessorio.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.acessorio.acessorio.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}

			break;
		case _bocaCategory:
			
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

			break;
		case _cabeloCategory:
			BarbaItem.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
			Sombrancelha.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
			_changePlayer.cabelo.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.cabelo.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.cabelo.cabelo.name && obj.transform.name != "thumb_hairs_3") {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
					if (obj.childCount > 0) {
						obj.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
					}
				}
				if (obj.transform.name == _changePlayer.cabelo.barba.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
				if (obj.transform.name == "cust4_1") {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}

			break;
		case _calcaCategory:
			_changePlayer.calca.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.calca.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.calca.calca.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}
			break;
		case _olhosCategory:
			_changePlayer.olhos.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.olhos.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.olhos.olhos.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}
			break;
		case _sapatoCategory:
			_changePlayer.sapato.cor = Color [index].GetComponent<Image> ().color;
			_changePlayer.sapato.indexCor = index;
			foreach (Transform obj in bigPlayerObjs){

				if (obj.transform.name == _changePlayer.sapato.sapato.name) {
					obj.gameObject.GetComponent<Anima2D.SpriteMeshInstance> ().color = Color [index].GetComponent<Image> ().color;
				}
			}
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

			break;
		default:
			break;
		}
	}
		
	// ajeita o numero de cores que aparecem no display de cores e o tamanho do scroll
	void ChangePalette(Color[] colors){

		// se o numero de cores for menor que o numero de botoes
		if (colors.Length < _numberColorsActive) {
			for (int i = colors.Length; i < Color.Length/*_numberColorsActive*/; i++)
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
			if (Estampas.Length != 0) Estampa.SetActive (true);

		} else if (_currentCategory == "Olhos") {

			if (Estampas.Length != 0) Estampa.SetActive (false);
			if (Barba.Length != 0) BarbaItem.SetActive (false);
			if (Cilios.Length != 0) CiliosItem.SetActive (true);

		} else if (_currentCategory == "Cabelo") {

			if (Estampas.Length != 0) Estampa.SetActive (false);
			if (Cilios.Length != 0) CiliosItem.SetActive (false);
			if (Barba.Length != 0) BarbaItem.SetActive (true);


		} else {
			
			if (Estampas.Length != 0) Estampa.SetActive (false);
			if (Cilios.Length != 0) CiliosItem.SetActive (false);
			if (Barba.Length != 0) BarbaItem.SetActive (false);

		}

	}

}
