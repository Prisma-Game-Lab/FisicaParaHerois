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
            

            ////limits x axis
            //if (Camera.main.transform.position.x + curMove.x > Mathf.Max(Limit1.x, Limit2.x))
            //{
            //    curMove.x = Mathf.Max(Limit1.x, Limit2.x) - Camera.main.transform.position.x;
            //}
            //else if (Camera.main.transform.position.x + curMove.x < Mathf.Min(Limit1.x, Limit2.x))
            //{
            //    curMove.x = Mathf.Min(Limit1.x, Limit2.x) - Camera.main.transform.position.x;
            //}

            ////limits y axis
            //if (Camera.main.transform.position.y + curMove.y > Mathf.Max(Limit1.y, Limit2.y))
            //{
            //    curMove.y = Mathf.Max(Limit1.y, Limit2.y) - Camera.main.transform.position.y;
            //}
            //else if (Camera.main.transform.position.y + curMove.y < Mathf.Min(Limit1.y, Limit2.y))
            //{
            //    curMove.y = Mathf.Min(Limit1.y, Limit2.y) - Camera.main.transform.position.y;
            //}

            CurOffset -= curMove;
            TimeLeft -= Time.deltaTime;
            Camera.main.transform.Translate(curMove);

            
        }

        else
        {
            TimeLeft = 0;
        }
	}

    public void LateUpdate()
    {
        //Vector3 newPos = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, _minX, _maxX), Mathf.Clamp(Camera.main.transform.position.y, _minY, _maxY), Camera.main.transform.position.z);
        //Camera.main.transform.SetPositionAndRotation(newPos, Camera.main.transform.rotation);
    }

    public void Move(Vector3 offset)
    {
        if (disableScroll) return;

        Debug.Log("Move camera. offset: " + offset.ToString());
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
