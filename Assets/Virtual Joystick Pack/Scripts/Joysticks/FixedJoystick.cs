using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    [Header("Fixed Joystick")]

    public PlayerInfo Player;
    public float MinDistanceToMoveCamera = 0.3f;
    Vector2 joystickPosition = Vector2.zero;

	private Camera cam = new Camera();
	private PlayerInfo _info;
	private PlayerInput _input;
	private bool _realJump;
	private bool _isJumping;

    void OnValidate()
    {
        if (Player == null)
        {
            Player = PlayerInfo.PlayerInstance;
        }
    }

    void Start()
    {
        OnValidate();
		_info = Player.GetComponent<PlayerInfo> ();
		_input = Player.GetComponent<PlayerInput> ();
		_realJump = _input.realJump;
		_isJumping = _input._isJumping;
        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);
    }

	void Update(){
		if (inputVector.x > 0) {

			if (_realJump) {
				if (!_isJumping) {
					Player.Move (false, MinDistanceToMoveCamera);
					_info.CheckInputFlip ("RightDir");
				}
			} else {
				Player.Move (false, MinDistanceToMoveCamera);
				_info.CheckInputFlip ("RightDir");
			}
		} else if (inputVector.x < 0) {
			if (_realJump) {
				if (!_isJumping) {
					Player.Move (true, MinDistanceToMoveCamera);
					_info.CheckInputFlip ("LeftDir");
				}
			} else {
				Player.Move (true, MinDistanceToMoveCamera);
				_info.CheckInputFlip ("LeftDir");
			}
		}
	}

    public override void OnDrag(PointerEventData eventData)
    {
		if (TutorialDialog.IsCanvasOn) {
			return;
		}
		
        Vector2 direction = eventData.position - joystickPosition;

        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}