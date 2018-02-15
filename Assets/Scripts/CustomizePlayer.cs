using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	private ScrollRect _CategoryScroll;
	private RectTransform _ColorScroll;

	private int _numberColorsActive = 5;
	private string _currentCategory;
	private int _indexItemCategoria = 0;
	private int _indexCategoriaAcessorio = 0;

	private struct Camisa {

		public Sprite camiseta;
		public Sprite estampa;
		public Color cor;

		public Camisa (Sprite camiseta, Sprite estampa, Color cor){
			this.camiseta = camiseta;
			this.estampa = estampa;
			this.cor = cor;
		}
	}

	private struct Olho {

		public Sprite olhos;
		public Sprite cilios;
		public Color cor;

		public Olho (Sprite olhos, Sprite cilios, Color cor){
			this.olhos = olhos;
			this.cilios = cilios;
			this.cor = cor;
		}
	}

	private struct Cabelo {

		public Sprite cabelo;
		public Sprite barba;
		public Color cor;

		public Cabelo (Sprite cabelo, Sprite barba, Color cor){
			this.cabelo = cabelo;
			this.barba = barba;
			this.cor = cor;
		}
	}

	private struct CorDePele {

		public Sprite corpo;
		public Color cor;

		public CorDePele (Sprite corpo, Color cor){
			this.corpo = corpo;
			this.cor = cor;
		}
	}

	private struct Nariz {

		public Sprite nariz;
		public Color cor;

		public Nariz (Sprite nariz, Color cor){
			this.nariz = nariz;
			this.cor = cor;
		}
	}

	private struct Boca {

		public Sprite boca;
		public Color cor;

		public Boca (Sprite boca, Color cor){
			this.boca = boca;
			this.cor = cor;
		}
	}

	private struct Calca {

		public Sprite calca;
		public Color cor;

		public Calca (Sprite calca, Color cor){
			this.calca = calca;
			this.cor = cor;
		}
	}

	private struct Sapato {

		public Sprite sapato;
		public Color cor;

		public Sapato (Sprite sapato, Color cor){
			this.sapato = sapato;
			this.cor = cor;
		}
	}

	private struct Acessorio {

		public Sprite acessorio;
		public Color cor;

		public Acessorio (Sprite acessorio, Color cor){
			this.acessorio = acessorio;
			this.cor = cor;
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

		//PlayerCustomizable

		_changePlayer = new Customize (
			new Camisa (Camisetas [0], Estampas [0], CorCamisetas [0]), 
			new Olho (Olhos [0], Cilios [0], CorOlhos [0]), 
			new Cabelo (Cabelos [0], Barba [0], CorCabelo [0]), 
			new CorDePele (Corpo [0], CorPele [0]), 
			new Nariz (Narizes [0], CorNariz [0]), 
			new Boca (Bocas [0], CorBoca [0]),
			new Calca (Calcas [0], CorCalca [0]), 
			new Sapato (Sapatos [0], CorSapato [0]), 
			new Acessorio (Acessorios [0], CorAcessorio [0])
		);

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

		PlayerCustomizable.transform.Find ("blusas").Find (_changePlayer.camisa.camiseta.name).gameObject.SetActive (true);


		Debug.Log (Camisetas[0].name);

		Debug.Log(PlayerCustomizable.transform.Find ("blusas").Find(Camisetas[0].name));

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
			}
		} else if (_currentCategory == "Olhos") {
			if (_indexCategoriaAcessorio < Cilios.Length - 1) {
				_indexCategoriaAcessorio += 1;
				CiliosItem.GetComponent<Image> ().sprite = Cilios [_indexCategoriaAcessorio];
			}
		} else if (_currentCategory == "Cabelo") {
			if (_indexCategoriaAcessorio < Barba.Length - 1) {
				_indexCategoriaAcessorio += 1;
				BarbaItem.GetComponent<Image> ().sprite = Barba [_indexCategoriaAcessorio];
			}
		}

	}

	// Muda a estampa/cilios
	void DownButton() {

		if (_currentCategory == "Camisas") {
			if (_indexCategoriaAcessorio > 0) {
				_indexCategoriaAcessorio -= 1;
				Estampa.GetComponent<Image> ().sprite = Estampas [_indexCategoriaAcessorio];
			}
		} else if (_currentCategory == "Olhos") {
			if (_indexCategoriaAcessorio > 0) {
				_indexCategoriaAcessorio -= 1;
				CiliosItem.GetComponent<Image> ().sprite = Cilios [_indexCategoriaAcessorio];
			}
		} else if (_currentCategory == "Cabelo") {
			if (_indexCategoriaAcessorio > 0) {
				_indexCategoriaAcessorio -= 1;
				BarbaItem.GetComponent<Image> ().sprite = Barba [_indexCategoriaAcessorio];
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
				break;
			case "Nariz":
				gameObject.GetComponent<Image> ().sprite = Narizes [_indexItemCategoria];
				break;
			case "Boca":
				gameObject.GetComponent<Image> ().sprite = Bocas [_indexItemCategoria];
				break;
			case "Cor de pele":
				gameObject.GetComponent<Image> ().sprite = Corpo [_indexItemCategoria];
				break;
			case "Cabelo":
				gameObject.GetComponent<Image> ().sprite = Cabelos [_indexItemCategoria];
				break;
			case "Calca":
				gameObject.GetComponent<Image> ().sprite = Calcas [_indexItemCategoria];
				break;
			case "Sapato":
				gameObject.GetComponent<Image> ().sprite = Sapatos [_indexItemCategoria];
				break;
			case "Acessorios":
				gameObject.GetComponent<Image> ().sprite = Acessorios [_indexItemCategoria];
				break;
			case "Olhos":
				gameObject.GetComponent<Image> ().sprite = Olhos [_indexItemCategoria];
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
			}
			break;
		case "Nariz":
			if (_indexItemCategoria < Narizes.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Narizes [_indexItemCategoria];
			}
			break;
		case "Boca":
			if (_indexItemCategoria < Bocas.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Bocas [_indexItemCategoria];
			}
			break;
		case "Cor de pele":
			if (_indexItemCategoria < Corpo.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Corpo [_indexItemCategoria];
			}
			break;
		case "Cabelo":
			if (_indexItemCategoria < Cabelos.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Cabelos [_indexItemCategoria];
			}
			break;
		case "Calca":
			if (_indexItemCategoria < Calcas.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Calcas [_indexItemCategoria];
			}
			break;
		case "Sapato":
			if (_indexItemCategoria < Sapatos.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Sapatos [_indexItemCategoria];
			}
			break;
		case "Acessorios":
			if (_indexItemCategoria < Acessorios.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Acessorios [_indexItemCategoria];
			}
			break;
		case "Olhos":
			if (_indexItemCategoria < Olhos.Length - 1) {
				_indexItemCategoria += 1;
				gameObject.GetComponent<Image> ().sprite = Olhos [_indexItemCategoria];
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
			gameObject.GetComponent<Image> ().sprite = Camisetas [0];
			if (Estampas.Length != 0)
				Estampa.GetComponent<Image> ().sprite = Estampas [0];
                // dar um jeito de adicionar a estampa ao sprite da camiseta
			if (CorCamisetas.Length != 0) {
				gameObject.GetComponent<Image> ().color = CorCamisetas [0];
				ChangePalette (CorCamisetas);
			}
			ActivatePattern ();
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

				// setar a paleta de cores com as cores disponíveis da camisa
				// ativar apenas a camisa no personagem
                break;

		case "Cor de pele":
			
			_currentCategory = "Cor de pele";
			gameObject.GetComponent<Image> ().sprite = Corpo [0];
			if (CorPele.Length != 0) {
				gameObject.GetComponent<Image> ().color = CorPele [0];
				ChangePalette (CorPele);
			}
			ActivatePattern ();
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

		case "Nariz":
			
			_currentCategory = "Nariz";
			gameObject.GetComponent<Image> ().sprite = Narizes [0];
			if (CorNariz.Length != 0) {
				gameObject.GetComponent<Image> ().color = CorNariz [0];
				ChangePalette (CorNariz);
			}
			ActivatePattern ();
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

				break;

		case "Boca":
			
			_currentCategory = "Boca";
			gameObject.GetComponent<Image> ().sprite = Bocas [0];
			if (CorBoca.Length != 0) {
				gameObject.GetComponent<Image> ().color = CorBoca [0];
				ChangePalette (CorBoca);
			}
			ActivatePattern ();
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

		case "Cabelo":
			
			_currentCategory = "Cabelo";
			gameObject.GetComponent<Image> ().sprite = Cabelos [0];
			if (Barba.Length != 0)
				BarbaItem.GetComponent<Image> ().sprite = Barba [0];
			if (CorCabelo.Length != 0) {
				gameObject.GetComponent<Image> ().color = CorCabelo [0];
				ChangePalette (CorCabelo);
			}
			ActivatePattern ();
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

		case "Calca":
			
			_currentCategory = "Calca";
			gameObject.GetComponent<Image> ().sprite = Calcas [0];
			if (CorCalca.Length != 0) {
				gameObject.GetComponent<Image> ().color = CorCalca [0];
				ChangePalette (CorCalca);
			}
			ActivatePattern ();
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

		case "Sapato":
			
			_currentCategory = "Sapato";
			gameObject.GetComponent<Image> ().sprite = Sapatos [0];
			if (CorSapato.Length != 0) {
				gameObject.GetComponent<Image> ().color = CorSapato [0];
				ChangePalette (CorSapato);
			}
			ActivatePattern ();
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

		case "Acessorios":
			
			_currentCategory = "Acessorios";
			gameObject.GetComponent<Image> ().sprite = Acessorios [0];
			if (CorAcessorio.Length != 0) {
				gameObject.GetComponent<Image> ().color = CorAcessorio [0];
				ChangePalette (CorAcessorio);
			}
			ActivatePattern ();
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

		case "Olhos":
			
			_currentCategory = "Olhos";
			gameObject.GetComponent<Image> ().sprite = Olhos [0];
			if (Cilios.Length != 0)
				CiliosItem.GetComponent<Image> ().sprite = Cilios [0];
			if (CorOlhos.Length != 0) {
				gameObject.GetComponent<Image> ().color = CorOlhos [0];
				ChangePalette (CorOlhos);
			}
			ActivatePattern ();
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

            default:
			
                break;
        }
			
	}

	// Muda os cor do texto
	void ColorButton(int index) {
		
		gameObject.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
		if (_currentCategory == "Cabelo") {
			BarbaItem.GetComponent<Image>().color = Color [index].GetComponent<Image> ().color;
			Sombrancelha.GetComponent<Image>().color = Color [index].GetComponent<Image> ().color;
		}
		// muda a cor do asset
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
