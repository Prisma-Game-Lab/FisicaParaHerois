using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraController : MonoBehaviour {
    [HideInInspector] public Vector3 CurOffset = Vector3.zero;
    [HideInInspector] public float TimeLeft = 0;
    public float CameraSpeed = 5f;
    public PostProcessingBehaviour PhysicsVisionPostProcessing;

    void OnValidate()
    {
        if (PhysicsVisionPostProcessing == null)
        {
            PhysicsVisionPostProcessing = Camera.main.GetComponent<PostProcessingBehaviour>();
        }
    }

    [Header("Limits for camera movement")]
    public Vector2 Limit1;
    public Vector2 Limit2;

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

            //limits x axis
            if (Camera.main.transform.position.x + curMove.x > Mathf.Max(Limit1.x, Limit2.x))
            {
                curMove.x = Mathf.Max(Limit1.x, Limit2.x) - Camera.main.transform.position.x;
            }
            else if (Camera.main.transform.position.x + curMove.x < Mathf.Min(Limit1.x, Limit2.x))
            {
                curMove.x = Mathf.Min(Limit1.x, Limit2.x) - Camera.main.transform.position.x;
            }

            //limits y axis
            if (Camera.main.transform.position.y + curMove.y > Mathf.Max(Limit1.y, Limit2.y))
            {
                curMove.y = Mathf.Max(Limit1.y, Limit2.y) - Camera.main.transform.position.y;
            }
            else if (Camera.main.transform.position.y + curMove.y < Mathf.Min(Limit1.y, Limit2.y))
            {
                curMove.y = Mathf.Min(Limit1.y, Limit2.y) - Camera.main.transform.position.y;
            }

            
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

    public void OnPhysicsVisionActivated()
    {
        if (PhysicsVisionPostProcessing != null)
        {
            PhysicsVisionPostProcessing.enabled = true;
        }
    }

    public void OnPhysicsVisionDeactivated()
    {
        if (PhysicsVisionPostProcessing != null)
        {
            PhysicsVisionPostProcessing.enabled = false;
        }
    }
}
