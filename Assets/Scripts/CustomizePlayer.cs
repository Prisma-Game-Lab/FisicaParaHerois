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
	private int _NumInText = 0;
	private int _NumInSymbol = 0;
	private string[] _Symbols = {"", "!", "@", "#", "$", "%", "ˆ", "&", "*", "(", ")"};
	private string originalText = "CUSTOM TEXT";
	private ScrollRect _CategoryScroll;
	private ScrollRect _ColorScroll;

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

		_CategoryScroll = Categories [1].transform.parent.transform.parent.transform.parent.GetComponent<ScrollRect>();
		_ColorScroll = Color [1].transform.parent.transform.parent.transform.parent.GetComponent<ScrollRect> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		// veficar se o retandogulo do botao inclui o centro da tela e executar a acao do botao


		
	}

	// Muda os números
    void UpButton() {
		
		_NumInText += 1;

		TextoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];
		Debug.Log("Up");
	}

	// Muda os números
	void DownButton() {
		
		if (_NumInText > 0) {
			_NumInText -= 1;
		}

		TextoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];
		Debug.Log("Down");
	}

	// Muda os símbolos
	void LeftButton() {
		
		if (_NumInSymbol > 0) {
			_NumInSymbol -= 1;
		}

		TextoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];
		Debug.Log("Left");
	}

	// Muda os símbolos
	void RightButton() {

		if (_NumInSymbol < 11) {
			_NumInSymbol += 1;
		}

		TextoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];
		Debug.Log("Right");
	}

	void CategoryButton(int index) {

        // pega o nome da categoria e muda os itens relacionados a ele

        switch (Categories [index].name) {

            case "Camisas":
                gameObject.GetComponent<Image>().sprite = Camisetas[0];
                // dar um jeito de adicionar a estampa ao sprite da camiseta
                gameObject.GetComponent<Image>().color = CorCamisetas[0];
                break;
            case "Cor de pele":
				gameObject.GetComponent<Image>().color = CorPele[0];
				//vai ter algo mais?
                break;
            case "Nariz":
				gameObject.GetComponent<Image>().sprite = Narizes[0];
				gameObject.GetComponent<Image>().color = CorNariz[0];
				break;
            case "Boca":
				gameObject.GetComponent<Image>().sprite = Bocas[0];
				gameObject.GetComponent<Image>().color = CorBoca[0];
                break;
            case "Cabelo":
				gameObject.GetComponent<Image>().sprite = Cabelos[0];
				gameObject.GetComponent<Image>().color = CorCabelo[0];
                break;
            case "Calca":
				gameObject.GetComponent<Image>().sprite = Calcas[0];
				gameObject.GetComponent<Image>().color = CorCalca[0];
                break;
            case "Sapato":
				gameObject.GetComponent<Image>().sprite = Sapatos[0];
			gameObject.GetComponent<Image>().color = CorSapato[0];
                break;
            case "Acessorios":
				gameObject.GetComponent<Image>().sprite = Acessorios[0];
				gameObject.GetComponent<Image>().color = CorAcessorio[0];
                break;
            case "Olhos":
				gameObject.GetComponent<Image>().sprite = Olhos[0];
				// dar um jeito de botar cílios aqui
				gameObject.GetComponent<Image>().color = CorOlhos[0];
                break;
            default:
                break;
        }

		
		

		gameObject.GetComponent<Image> ().sprite = Categories [index].GetComponent<Image> ().sprite;		
		// Muda o asset
		Debug.Log("Categoria");
	}

	// Muda os cor do texto
	void ColorButton(int index) {
		
		gameObject.GetComponent<Image> ().color = Color [index].GetComponent<Image> ().color;
		// muda a cor do asset
		Debug.Log("Cor");
	}

	// cor da camisa muda mas a estampa da camisa nao

}
