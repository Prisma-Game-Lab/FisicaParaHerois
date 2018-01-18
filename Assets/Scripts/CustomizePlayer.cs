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

	private Text _textoGenerico;
	private int _NumInText = 0;
	private int _NumInSymbol = 0;
	private string[] _Symbols = {"", "!", "@", "#", "$", "%", "ˆ", "&", "*", "(", ")"};
	private string originalText = "CUSTOM TEXT";

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

		// Pega o primeiro e único filho da imagem que é o texto genérico 
		_textoGenerico = gameObject.transform.GetChild (0).GetComponent<Text>();


	}
	
	// Update is called once per frame
	void Update () {

		
	}

	// Muda os números
	void UpButton() {

		_NumInText += 1;

		_textoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];
		Debug.Log("Up");
	}

	// Muda os números
	void DownButton() {

		if (_NumInText > 0) {
			_NumInText -= 1;
		}

		_textoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];
		Debug.Log("Down");
	}

	// Muda os símbolos
	void LeftButton() {

		if (_NumInSymbol > 0) {
			_NumInSymbol -= 1;
		}

		_textoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];

		Debug.Log("Left");
	}

	// Muda os símbolos
	void RightButton() {

		if (_NumInSymbol < 11) {
			_NumInSymbol += 1;
		}

		_textoGenerico.text = originalText + " " + _NumInText + " " + _Symbols[_NumInSymbol];

		Debug.Log("Right");
	}

	// Muda os cor da imagem
	void CategoryButton(int index) {

		gameObject.GetComponent<Image> ().color = Categories [index].GetComponent<Image> ().color;
		Debug.Log("Categoria");
	}

	// Muda os cor do texto
	void ColorButton(int index) {

		_textoGenerico.color = Color [index].GetComponent<Image> ().color;
		Debug.Log("Cor");
	}

}
