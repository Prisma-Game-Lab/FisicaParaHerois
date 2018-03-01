using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCustumizeInstance {

	//static public Customize ChangePlayer;

	public Color camisaCor;
	public Color peleCor;
	public Color cabeloCor;
	public Color calcaCor;
	public Color sapatoCor;

	public string camisa;
	public string cabelo;
	public string calca;
	public string sapato;


	static public playerCustumizeInstance instance = null;
		
	public static playerCustumizeInstance GetInstance()
	{
		if(instance == null)
		{
			instance = new playerCustumizeInstance();
		}

		return instance;
	}

	public void salvaCores(Color corCamisa,Color corPele,Color corCabelo,Color corCalca,Color corSapato){
		camisaCor = corCamisa;
		peleCor = corPele;
		cabeloCor = corCabelo;
		calcaCor = corCalca;
		sapatoCor = corSapato;
	}

	public void salvaRoupas(string camisa, string cabelo,string calca,string sapato){
		this.camisa = camisa;
		this.cabelo = cabelo;
		this.calca = calca;
		this.sapato = sapato;
	}
		
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
