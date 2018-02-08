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


	[Header ("Camiseta")]
	public Sprite[] Camisetas;
	public Sprite[] Estampas;
	public Color[] CorCamisetas;

    [Header("Olho")]
    public Sprite[] Olhos;
    public Sprite[] Cilios;
    public Color[] CorOlhos;

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

    [Header("Calça")]
    public Sprite[] Calcas;
    public Color[] CorCalca;

    [Header("Sapato")]
    public Sprite[] Sapatos;
    public Color[] CorSapato;

    [Header("Acessorio")]
    public Sprite[] Acessorios;
    public Color[] CorAcessorio;

    [Tooltip("Primeiro e único filho da imagem que é o texto genérico")] public Text TextoGenerico;
	private int _NumInSymbol = 0;
	private string[] _Symbols = {"", "!", "@", "#", "$", "%", "ˆ", "&", "*", "(", ")"};
	private string originalText = "CUSTOM TEXT";
	private ScrollRect _CategoryScroll;
	private ScrollRect _ColorScroll;

	private int _numberColorsActive = 5;
	private string _currentCategory;
	private int _indexItemCategoria = 0;
	private int _indexCategoriaAcessorio = 0;

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

		_CategoryScroll = Categories [0].transform.parent.transform.parent.transform.parent.GetComponent<ScrollRect>();
		_ColorScroll = Color [0].transform.parent.transform.parent.transform.parent.GetComponent<ScrollRect> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		// veficar se o retandogulo do botao inclui o centro da tela e executar a acao do botao


		
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
		}
	}

	// Muda o item da categoria
	void LeftButton() {

		if (_indexItemCategoria > 0) {
			_indexItemCategoria -= 1;

			switch (_currentCategory) {

			case "Camisas": 
				break;
			case "Nariz":
				break;
			case "Boca":
				break;
			case "Cabelo":
				break;
			case "Calca":
				break;
			case "Sapato":
				break;
			case "Acessorios":
				break;
			case "Olhos":
				break;
			default:
				break;

			}

		}
		
		if (_NumInSymbol > 0) {
			_NumInSymbol -= 1;
		}

		//TextoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];
		Debug.Log("Left");
	}

	// Muda o item da categoria
	void RightButton() {

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

		if (_NumInSymbol < 11) {
			_NumInSymbol += 1;
		}

		//TextoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];
		Debug.Log("Right");
	}

	void CategoryButton(int index) {

        // pega o nome da categoria e muda os itens relacionados a ele

        switch (Categories [index].name) {

		case "Camisas":
			
			_currentCategory = "Camisas";
				// mudar sempre pra camisa e cor que estava antes
			gameObject.GetComponent<Image> ().sprite = Camisetas [0];
			Estampa.GetComponent<Image> ().sprite = Estampas [0];
			ChangeAlphaPattern (1f);
                // dar um jeito de adicionar a estampa ao sprite da camiseta
			gameObject.GetComponent<Image> ().color = CorCamisetas [0];
			ChangePalette (CorCamisetas);
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

				// setar a paleta de cores com as cores disponíveis da camisa
				// ativar apenas a camisa no personagem
                break;

            case "Cor de pele":
			
			_currentCategory = "Cor de pele";
				gameObject.GetComponent<Image>().color = CorPele[0];
			ChangePalette (CorPele);
				ChangeAlphaPattern (0);
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

            case "Nariz":
			
			_currentCategory = "Nariz";
				gameObject.GetComponent<Image>().sprite = Narizes[0];
				gameObject.GetComponent<Image>().color = CorNariz[0];
			ChangePalette (CorNariz);
			ChangeAlphaPattern (0);
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

				break;

            case "Boca":
			
			_currentCategory = "Boca";
				gameObject.GetComponent<Image>().sprite = Bocas[0];
				gameObject.GetComponent<Image>().color = CorBoca[0];
			ChangePalette (CorBoca);
			ChangeAlphaPattern (0);
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

            case "Cabelo":
			
			_currentCategory = "Cabelo";
				gameObject.GetComponent<Image>().sprite = Cabelos[0];
				gameObject.GetComponent<Image>().color = CorCabelo[0];
			ChangePalette (CorCabelo);
			ChangeAlphaPattern (0);
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

            case "Calca":
			
			_currentCategory = "Calca";
				gameObject.GetComponent<Image>().sprite = Calcas[0];
				gameObject.GetComponent<Image>().color = CorCalca[0];
			ChangePalette (CorCalca);
			ChangeAlphaPattern (0);
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

            case "Sapato":
			
			_currentCategory = "Sapato";
				gameObject.GetComponent<Image>().sprite = Sapatos[0];
				gameObject.GetComponent<Image>().color = CorSapato[0];
			ChangePalette (CorSapato);
			ChangeAlphaPattern (0);
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

            case "Acessorios":
			
			_currentCategory = "Acessorios";
				gameObject.GetComponent<Image>().sprite = Acessorios[0];
				gameObject.GetComponent<Image>().color = CorAcessorio[0];
			ChangePalette (CorAcessorio);
			ChangeAlphaPattern (0);
			_indexItemCategoria = 0;
			_indexCategoriaAcessorio = 0;

                break;

            case "Olhos":
			
			_currentCategory = "Olhos";
				gameObject.GetComponent<Image>().sprite = Olhos[0];
				// dar um jeito de botar cílios aqui
				gameObject.GetComponent<Image>().color = CorOlhos[0];
				ChangePalette (CorOlhos);
			ChangeAlphaPattern (1f);
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
		// muda a cor do asset
	}
		
	// ajeita o numero de cores que aparecem no display de cores
	// + fazer com que o scroll diminua conforme o numero de cores
	void ChangePalette(Color[] colors){

		if (colors.Length < _numberColorsActive) {
			for (int i = colors.Length; i < _numberColorsActive; i++)
				Color [i].gameObject.SetActive (false);
			// se o numero de cores for menor que o numero de botoes, dar um jeito de diminuir o scroll
		} else if (colors.Length > _numberColorsActive) {
			for (int i = 0 ; i < colors.Length; i++)
				Color [i].gameObject.SetActive (true);
			// se o numero de cores for maior que o numero de botoes, dar um jeito de diminuir o scroll
		}

		_numberColorsActive = colors.Length;

		for (int i = 0; i < colors.Length; i++) {
			Color [i].GetComponent<Image> ().color = colors [i];
		}

	}

	// tira o alpha da estampa e ou cílio
	void ChangeAlphaPattern(float alpha){
		
			if (_currentCategory == "Camisas") {
			
			Color cor = Estampa.GetComponent<Image> ().color;
			cor.a = alpha;
			Estampa.GetComponent<Image> ().color = cor;

			} else if (_currentCategory == "Olhos") {
			
			Color cor = CiliosItem.GetComponent<Image> ().color;
			cor.a = alpha;
			CiliosItem.GetComponent<Image> ().color = cor;

		} else {
			
			Color corEstampa = Estampa.GetComponent<Image> ().color;
			corEstampa.a = alpha;
			CiliosItem.GetComponent<Image> ().color = corEstampa;

			Color corCilio = Estampa.GetComponent<Image> ().color;
			corCilio.a = alpha;
			Estampa.GetComponent<Image> ().color = corCilio;
		}

	}

}
