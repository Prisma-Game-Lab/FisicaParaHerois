/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanelGangorra : MonoBehaviour {
	public static ActionPanelGangorra Instance;

	[Header("Colors")]
	public Color AvailableButtonColor = Color.white;
	public Color UnavailableButtonColor = Color.gray;

	[Header("Actions")]
	public List<Button> ActionButtons;
	[Tooltip("Nome da classe de cada action, na ordem dos botões da lista acima")]public List<string> ActionNames;

	[Header("Components")]
	[Tooltip("Slider que define o valor da ação")] public Slider ChosenValueSlider;
	[Tooltip("Texto que mostra o valor da ação")] public Text ChosenValueText;
	[Tooltip("Texto que contem o nome da ação")] public Text ActionNameText;

	private PhysicsObject _physicsObject; //guarda o PhysicsObject que chamou o menu de interação
	private Image _objectSpriteHolder; //guarda o campo de imagem que contém o sprite do objeto selecionado

	private IAction<float> _chosenAction; //ação selecionada no ActionPanel
	private float _chosenValue; //guarda o valor setado no slider
	private Animator _actionAnim; // Animator do ActionPanel

	// Use this for initialization
	void Start ()
	{
		// _objectSpriteHolder = transform.GetChild(2).GetChild(0).GetComponent<Image>();
		_actionAnim = gameObject.GetComponent<Animator>();

		if (isActiveAndEnabled)
		{
			gameObject.SetActive(false);
		}
	}

	void Awake()
	{
		Instance = this;
	}

	// Update is called once per frame
	void Update () {

	}

	/// <summary>
	/// Define o que acontece quando um botão é selecionado
	/// </summary>
	/// <param name="action"></param>


	/// <summary>
	/// Define o que acontece quando o slider da ação escolhida é modificado
	/// </summary>
	/// <param name="value"></param>
	public void OnActionValueChanged()
	{
		_chosenValue = ChosenValueSlider.value;
		ChosenValueText.text = _chosenValue.ToString();
//		
//		switch (_chosenAction.GetActionName())
//		{
//		case "Change mass":
//			_actionAnim.SetFloat("mass", _chosenValue);
//			break;
//		case "Change gravity":
//			_actionAnim.SetFloat("grav", _chosenValue);
//			break;
//		default:
//			break;
//		}
	}

	/// <summary>
	/// Define o que acontece quando o botão de confirmar ação é clicado
	/// </summary>
	public void OnActionConfirm()
	{
		//_chosenAction.SetTarget(_physicsObject);
		_chosenAction.OnActionUse(_chosenValue);
		StartCoroutine(ButtonAnimDelay(1.45f));

		//OnChooseActionPanelActivated();
		StartCoroutine(CancelButtonDelay(0.7f));
	}

	/// <summary>
	/// Define o que acontece quando o botão de cancelar é pressionado no menu de ações do objeto
	/// </summary>
	public void OnCancel()
	{
		StartCoroutine(CancelButtonDelay(0.7f));
		//gameObject.SetActive(false);
		//Time.timeScale = 1;

		//_physicsObject = null;
	}

	/// <summary>
	/// Define o que acontece quando o painel é ativado
	/// </summary>
	/// <param name="clicked">Physics Object que foi clicado</param>
	public void OnPanelActivated(PhysicsObject clicked)
	{
		_physicsObject = clicked;

		//Ativa o painel de ação
		//_objectSpriteHolder.sprite = _physicsObject.ObjectSprite;
		gameObject.SetActive(true);

		Time.timeScale = 0;
		//OnChooseActionPanelActivated();

		//escolhe a ação de alterar o eixo da gangorra
		_chosenAction = PlayerInfo.PlayerInstance.GetComponent<ChangeSeesawAnchor>();
		ActionNameText.text = _chosenAction.GetActionName();
		_chosenAction.SetTarget(_physicsObject);

		ChosenValueSlider.wholeNumbers = false;
		ChosenValueSlider.maxValue = 0.95f;
		ChosenValueSlider.minValue = -0.95f;
		ChosenValueSlider.value = _chosenAction.GetCurrentValue();

		OnActionValueChanged();
	}

	/// <summary>
	/// Define o que ocorre quando o painel de escolher ação é ativado
	/// </summary>

	public IEnumerator CancelButtonDelay (float tempo)
	{
		_actionAnim.SetTrigger("exit");

		yield return new WaitForSecondsRealtime (tempo);

		gameObject.SetActive(false);
		Time.timeScale = 1;

		_physicsObject = null;
	}

	public IEnumerator ButtonAnimDelay(float tempo)
	{
		_actionAnim.SetTrigger("go");

		yield return new WaitForSecondsRealtime(tempo);
	}
}


