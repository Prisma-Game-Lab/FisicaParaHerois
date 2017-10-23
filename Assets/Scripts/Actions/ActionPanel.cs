using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour {
    public static ActionPanel Instance;

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

        _chosenAction = PlayerInfo.PlayerInstance.Actions[action];
        _actionNameText.text = _chosenAction.GetActionName();

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
        _chosenAction.SetTarget(_physicsObject);
        OnCancel(); //despausa o jogo
        _chosenAction.Do(_chosenValue);
    }

    /// <summary>
    /// Define o que acontece quando o botão de cancelar ação é clicado
    /// </summary>
    public void OnActionCanceled()
    {
        //Ativa o painel de escolher ação
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
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

        //Ativa o painel de escolher ação
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
