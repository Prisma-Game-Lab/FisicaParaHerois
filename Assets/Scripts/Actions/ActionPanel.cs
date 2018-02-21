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

    [Header("Components")]
    [Tooltip("Slider que define o valor da ação")] public Slider ChosenValueSlider;
    [Tooltip("Texto que mostra o valor da ação")] public Text ChosenValueText;
    [Tooltip("Texto que mostra o valor da gravidade")] public Text GravityValueText;
    [Tooltip("Texto que mostra o valor da massa")] public Text MassValueText;
    [Tooltip("Texto que contem o nome da ação")] public Text ActionNameText;
    [Tooltip("Menu onde a ação é escolhida")] public Transform ChooseActionMenu;
    [Tooltip("Menu onde a ação é configurada")] public Transform ConfirmActionMenu;

    [Tooltip("Numero que é a razão gravidade mostrada / gravidade real")] public float GravityTextMultiplier;
    [Tooltip("Numero que é a razão massa mostrada / massa real")] public float MassTextMultiplier;


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
        ConfirmActionMenu.gameObject.SetActive(true);
        //ChooseActionMenu.gameObject.SetActive(false);


        if (PlayerInfo.PlayerInstance.Actions.Count < action)
        {
            Debug.LogError("Ação inválida");
            return;
        }

        _chosenAction = PlayerInfo.PlayerInstance.Actions[action];
        ActionNameText.text = _chosenAction.GetActionName();
        
        //ChosenValueText.text = _chosenAction.GetCurrentValue().ToString();

        switch (action) {
		case 0:
            _actionAnim.SetBool("button1", false);
            _actionAnim.SetBool("button2", true);
			ChosenValueSlider.minValue = _physicsObject.AvailableActions.ChangeGravityActionMinValue / 2.0f; // /2.0 é gambiarra para o slider pular de dois em dois
			ChosenValueSlider.maxValue = _physicsObject.AvailableActions.ChangeGravityActionMaxValue / 2.0f;
            ChosenValueText.text = (_chosenAction.GetCurrentValue() * GravityTextMultiplier / 2.0f /* gambiarra*/).ToString();
            ChosenValueSlider.value = _chosenAction.GetCurrentValue() / 2.0f; // gambiarra
            break;
		case 1:
            _actionAnim.SetBool("button2", false);
            _actionAnim.SetBool("button1", true);
			ChosenValueSlider.minValue = _physicsObject.AvailableActions.ChangeMassActionMinValue;
			ChosenValueSlider.maxValue = _physicsObject.AvailableActions.ChangeMassActionMaxValue;
            ChosenValueText.text = (_chosenAction.GetCurrentValue() * MassTextMultiplier).ToString();
            ChosenValueSlider.value = _chosenAction.GetCurrentValue();
            break;
		}        

        StartCoroutine(ButtonAnimDelay(1.45f));

        ChooseActionMenu.gameObject.SetActive(false);
    }

    /// <summary>
    /// Define o que acontece quando o slider da ação escolhida é modificado
    /// </summary>
    /// <param name="value"></param>
    public void OnActionValueChanged()
    {
        
        
        switch (_chosenAction.GetActionName())
        {
            case "Change mass":
                _chosenValue = ChosenValueSlider.value;
                _actionAnim.SetFloat("mass", _chosenValue);
                ChosenValueText.text = (_chosenValue * MassTextMultiplier).ToString();
                break;
            case "Change gravity":
                _chosenValue = ChosenValueSlider.value;
                _actionAnim.SetFloat("grav", _chosenValue * 2.0f); // gambiarra
                ChosenValueText.text = (_chosenValue * GravityTextMultiplier).ToString();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Define o que acontece quando o botão de confirmar ação é clicado
    /// </summary>
    public void OnActionConfirm()
    {
        //_chosenAction.SetTarget(_physicsObject);
        switch (_chosenAction.GetActionName())
        {
            case "Change mass":
                _chosenAction.OnActionUse(_chosenValue);
                break;
            case "Change gravity":
                _chosenAction.OnActionUse(_chosenValue * 2.0f); //gambiarra
                break;
            default:
                break;
        }
    

        
        StartCoroutine(ButtonAnimDelay(1.45f));
        OnChooseActionPanelActivated();
    }

    /// <summary>
    /// Define o que acontece quando o botão de cancelar ação é clicado
    /// </summary>
    public void OnActionCanceled()
    {
        StartCoroutine(ButtonAnimDelay(1.45f));
        OnChooseActionPanelActivated();
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
        OnChooseActionPanelActivated();
    }

    /// <summary>
    /// Define o que ocorre quando o painel de escolher ação é ativado
    /// </summary>
    public void OnChooseActionPanelActivated()
    {
        //Ativa o painel de escolher ação
        ChooseActionMenu.gameObject.SetActive(true);
        ConfirmActionMenu.gameObject.SetActive(false);

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

        foreach (IAction<float> action in PlayerInfo.PlayerInstance.Actions)
        {
            action.SetTarget(_physicsObject);
            switch (action.GetActionName())
            {
                case "Change mass":
                    MassValueText.text = (action.GetCurrentValue()* MassTextMultiplier).ToString();
                    _actionAnim.SetFloat("mass", action.GetCurrentValue());
                    break;
                case "Change gravity":
                    GravityValueText.text = (action.GetCurrentValue() * GravityTextMultiplier / 2.0f /* gambiarra*/ ).ToString();
                    _actionAnim.SetFloat("grav", action.GetCurrentValue());
                    break;
                default:
                    break;
            }
        }
    }

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


