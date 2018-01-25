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

    [Tooltip("Primeiro e único filho da imagem que é o texto genérico")] public Text TextoGenerico;
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
	}
	
	// Update is called once per frame
	void Update () {

		
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
		
		gameObject.GetComponent<Image> ().color = Categories [index].GetComponent<Image> ().color;
		Debug.Log("Categoria");
	}

	// Muda os cor do texto
	void ColorButton(int index) {
		
		TextoGenerico.color = Color [index].GetComponent<Image> ().color;
		Debug.Log("Cor");
	}

}
