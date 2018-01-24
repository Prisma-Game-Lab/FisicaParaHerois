/* 
* Copyright (c) Rio PUC Games
* RPG Programming Team 2017
*
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour {
    public static ActionPanel Instance;

    [Header("Colors")]
    public Color AvailableButtonColor = Color.white;
    public Color UnavailableButtonColor = Color.gray;

    [Header("Actions")]
    public List<Button> ActionButtons;
    [Tooltip("Nome da classe de cada action, na ordem dos botões da lista acima")]public List<string> ActionNames;

    private PhysicsObject _physicsObject; //guarda o PhysicsObject que chamou o menu de interação
    private Image _objectSpriteHolder; //guarda o campo de imagem que contém o sprite do objeto selecionado
    private Slider _chosenValueSlider; //guarda o slider que define o valor da ação
    private Text _chosenValueText; //guarda o texto que mostra o valor da ação
    private Text _actionNameText; //guarda o texto que contem o nome da ação

    private IAction<float> _chosenAction; //ação selecionada no ActionPanel
    private float _chosenValue; //guarda o valor setado no slider

    // Use this for initialization
    void Start ()
    {
        _actionNameText = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        _chosenValueSlider = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Slider>();
        _chosenValueText = transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<Text>();
        _objectSpriteHolder = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        gameObject.SetActive(false);
    }

    void Awake()
    {
        Instance = this;
        Debug.Log(Instance);
    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// Define o que acontece quando um botão é selecionado
    /// </summary>
    /// <param name="action"></param>
    public void OnActionChosen(int action)
    {
        //Ativa o painel de confirmar ação
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        if (PlayerInfo.PlayerInstance.Actions.Count < action)
        {
            Debug.LogError("Ação inválida");
            return;
        }

		switch (action) {
		case 0:
			_chosenValueSlider.minValue = _physicsObject.AvailableActions.ChangeGravityActionMinValue;
			_chosenValueSlider.maxValue = _physicsObject.AvailableActions.ChangeGravityActionMaxValue;
			break;
		case 1:
			_chosenValueSlider.minValue = _physicsObject.AvailableActions.ChangeMassActionMinValue;
			_chosenValueSlider.maxValue = _physicsObject.AvailableActions.ChangeMassActionMaxValue;
			break;
		}

        _chosenAction = PlayerInfo.PlayerInstance.Actions[action];
        _actionNameText.text = _chosenAction.GetActionName();
        _chosenAction.SetTarget(_physicsObject);
        _chosenValueSlider.value = _chosenAction.GetCurrentValue();
    }

    /// <summary>
    /// Define o que acontece quando o slider da ação escolhida é modificado
    /// </summary>
    /// <param name="value"></param>
    public void OnActionValueChanged()
    {
        _chosenValue = _chosenValueSlider.value;
        _chosenValueText.text = _chosenValue.ToString();
    }

    /// <summary>
    /// Define o que acontece quando o botão de confirmar ação é clicado
    /// </summary>
    public void OnActionConfirm()
    {
        //_chosenAction.SetTarget(_physicsObject);
        _chosenAction.OnActionUse(_chosenValue);
        OnChooseActionPanelActivated();
    }

    /// <summary>
    /// Define o que acontece quando o botão de cancelar ação é clicado
    /// </summary>
    public void OnActionCanceled()
    {
        OnChooseActionPanelActivated();
    }

    /// <summary>
    /// Define o que acontece quando o botão de cancelar é pressionado no menu de ações do objeto
    /// </summary>
    public void OnCancel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;

        _physicsObject = null;
    }

    /// <summary>
    /// Define o que acontece quando o painel é ativado
    /// </summary>
    /// <param name="clicked">Physics Object que foi clicado</param>
    public void OnPanelActivated(PhysicsObject clicked)
    {
        _physicsObject = clicked;

        //Ativa o painel de ação
        _objectSpriteHolder.sprite = _physicsObject.ObjectSprite;
        gameObject.SetActive(true);
        Time.timeScale = 0;

        OnChooseActionPanelActivated();
    }

    /// <summary>
    /// Define o que ocorre quando o painel de escolher ação é ativado
    /// </summary>
    public void OnChooseActionPanelActivated()
    {
        //Ativa o painel de escolher ação
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);

        //Checa ações disponíveis
        AvailableActionsData availableActions = _physicsObject.AvailableActions;

        if(ActionNames.Count != ActionButtons.Count)
        {
            Debug.LogError("Número de botões (excluindo o cancel) não corresponde ao número de ações nomeadas no Inspector.");
        }

        for(int i = 0; i < ActionNames.Count; i++)
        {
            //Checa se cada ação está disponível
            ActionButtons[i].interactable = availableActions.CheckIfActionIsAvailable(ActionNames[i]);

            //Muda a cor do botão para indicar se está disponível
            ActionButtons[i].gameObject.GetComponent<Image>().color = ActionButtons[i].interactable ? AvailableButtonColor : UnavailableButtonColor;
        }
    }
}
