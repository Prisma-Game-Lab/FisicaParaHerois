using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [HideInInspector] public Vector3 CurOffset = Vector3.zero;
    [HideInInspector] public float TimeLeft = 0;
    public float CameraSpeed = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (CurOffset != Vector3.zero && TimeLeft > 0)
        {
            Vector3 curMove = (Time.deltaTime / TimeLeft) * CurOffset;
            CurOffset -= curMove;
            TimeLeft -= Time.deltaTime;
            Camera.main.transform.Translate(curMove);
        }

        else
        {
            TimeLeft = 0;
        }
	}

    public void Move(Vector3 offset)
    {
        CurOffset = offset;
        TimeLeft = 1/CameraSpeed;
    }
}
