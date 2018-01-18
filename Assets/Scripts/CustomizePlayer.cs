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

	// Use this for initialization
	void Start () {

		Up.onClick.AddListener (UpButton);
		Down.onClick.AddListener (DownButton);
		Left.onClick.AddListener (LeftButton);
		Right.onClick.AddListener (RightButton);


		
	}
	
	// Update is called once per frame
	void Update () {

		
	}

	// Muda os símbolos
    void UpButton() {
		Debug.Log("Up");
	}

	// Muda os símbolos
	void DownButton() {
		Debug.Log("Down");
	}

	// Muda os números
	void LeftButton() {
		Debug.Log("Left");
	}

	// Muda os números
	void RightButton() {
		Debug.Log("Right");
	}

}
