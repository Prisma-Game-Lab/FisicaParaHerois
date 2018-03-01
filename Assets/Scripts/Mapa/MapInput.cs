using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInput : MonoBehaviour {
	public CameraController MapCamera;
	public float CameraTouchSpeed = 0.01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int touches = Input.touchCount;

		if (touches > 0) {
			Touch _touch = Input.GetTouch (0);

			Debug.Log("Move Camera");
			CameraController.Instance.CameraScroll (_touch.deltaPosition * -CameraTouchSpeed);
		}
	}
}
