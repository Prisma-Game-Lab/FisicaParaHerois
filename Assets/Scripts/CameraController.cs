using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraController : MonoBehaviour {
    [HideInInspector] public Vector3 CurOffset = Vector3.zero;
    [HideInInspector] public float TimeLeft = 0;
    public float CameraSpeed = 5f;
    public PostProcessingBehaviour PhysicsVisionPostProcessing;

    public bool disableScroll;
    public bool showBoundingBox;

    void OnValidate()
    {
        if (PhysicsVisionPostProcessing == null)
        {
            PhysicsVisionPostProcessing = Camera.main.GetComponent<PostProcessingBehaviour>();
        }
    }

    [HideInInspector]
    public Vector3 Limit1;
    [HideInInspector]
    public Vector3 Limit2;

    private float _maxX;
    private float _maxY;
    private float _minX;
    private float _minY;

    private Vector3 _cameraPosToCheck;

	// Use this for initialization
	void Start () {
        //calculate maximum camera positions, based on limits set and some camera calculations
        _maxY = Mathf.Max(Limit1.y, Limit2.y) - Camera.main.orthographicSize;
        _minY = Mathf.Min(Limit1.y, Limit2.y) + Camera.main.orthographicSize;
        _maxX = Mathf.Max(Limit1.x, Limit2.x) - Camera.main.orthographicSize * Screen.width / Screen.height;
        _minX = Mathf.Min(Limit1.x, Limit2.x) + Camera.main.orthographicSize * Screen.width / Screen.height;

    }
	
	// Update is called once per frame
	public void Update () {
        if (CurOffset != Vector3.zero && TimeLeft > 0)
        {
            Vector3 curMove = (Time.deltaTime / TimeLeft) * CurOffset;
            
			/*
            //checa se camera vai ultrapassar bounds
            if (!OutOfBoundsX(Camera.main.transform.position + curMove))
            {
                //Camera.main.transform.Translate(curMove);
				curMove = new Vector3(0, curMove.y, curMove.z);
                //CurOffset -= curMove;
                //TimeLeft -= Time.deltaTime;
            }

			if(!OutOfBoundsY(Camera.main.transform.position + curMove))
			{
				//Camera.main.transform.Translate(curMove);
				curMove = new Vector3(curMove.x, 0, curMove.z);
				//CurOffset -= curMove;
				//TimeLeft -= Time.deltaTime;
			}
			/*
            else
            {
                CurOffset = Vector3.zero;
                TimeLeft = 0;
            }       */   

			Camera.main.transform.Translate(curMove);
			CurOffset -= curMove;
			TimeLeft -= Time.deltaTime;

            _cameraPosToCheck = curMove + Camera.main.transform.position;
            
        }
        else
        {
            TimeLeft = 0;
        }
	}

    //recebe uma posição e retorna true se ela for out of bounds para a camera, conforme especificado pelos limits
    private bool OutOfBoundsX(Vector3 position)
    {
        return position.x > _maxX || position.x < _minX;
    }

	private bool OutOfBoundsY(Vector3 position)
	{
		return position.y > _maxY || position.y < _minY;
	}

    public void Move(Vector3 offset)
    {
        if (disableScroll) return;

		/*
		CurOffset = offset;
		if (!OutOfBoundsX (Camera.main.transform.position + offset)) {
			CurOffset = new Vector3 (0, CurOffset.y, CurOffset.z);
		}

		if (!OutOfBoundsY (Camera.main.transform.position + offset)) {
			CurOffset = new Vector3 (CurOffset.x, 0, CurOffset.z);
		}
		*/

		Vector3 posCam = Camera.main.transform.position;
		CurOffset = new Vector3 (Mathf.Clamp (offset.x, _minX - posCam.x, _maxX - posCam.x), 
					Mathf.Clamp (offset.y, _minY - posCam.y, _maxY - posCam.y));

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
