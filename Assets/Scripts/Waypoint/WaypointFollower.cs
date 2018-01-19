#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//object behavior that follow one specific path
public class WaypointFollower : MonoBehaviour {

    public string pathName;
    private float stickySpeed = 0;
    public float speed;
    public float touchDistance = 0.1f;
    public bool stopped = false;

    public RepetitionType repetitionType;
    public FollowDirection followdirection;

    private WaypointManager _wpm;
    private PathInfo _path;
	private Rigidbody2D rb;

    //the last visited and next to visit path points
    private PathPoint next;
    private PathPoint last;

    public static WaypointFollower CreateWaypointFollowerIn(GameObject where, string pathname)
    {
        WaypointFollower mywp = where.AddComponent<WaypointFollower>();
        mywp.pathName = pathname;
        return mywp;
    }

    private WaypointFollower()
    {

    }

    public WaypointFollower(string pathname)
    {
        this.pathName = pathname;

    }
    
    // Use this for initialization
	void Start ()
    {
        

        //inicialization expects that pathname is set
		rb = this.GetComponent<Rigidbody2D>();
        _wpm = FindObjectOfType<WaypointManager>();

        if(_wpm == null)
        {
            Debug.Log("Waypoint manager not found. Follower not created");
            Destroy(this);
            return;
        }

        _path = _wpm.GetPath(pathName);
        if(_path == null)
        {
            Debug.Log("No path exists with name " + pathName + ". Follower not created");
            Destroy(this);
            return;
        }

        //by default, find closest path point to start following it
        next = _path.GetClosestPathPoint(this.transform.position);
        last = _path.getPreviousPathPoint(next);
        //Debug.Log("next: " + next.ToString());
        //Debug.Log("last: " + last.ToString());

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        if (stopped) return;
        
        //será que funciona para 3 dimensões?

        //will be used several times
        Vector3 toNextPoint = next.position - this.transform.position;

        //if arrived at next point
        if (toNextPoint.magnitude < touchDistance)
        {
            last = next;
            next = _path.GetNextPathPoint(next, this);
        }

        //direção entre follower e proximo ponto
        Vector3 DirectionToNext = (next.position - this.transform.position).normalized;
        
        Vector3 proj = Vector3.Dot((this.transform.position - last.position), (next.position - last.position)) * (next.position - last.position).normalized;
        
        //algo está errado aqui...

        //direção entre follower e linha do path
        //Vector3 DirectionToLine = proj - this.transform.position;
        //não tá funcionando ainda

        transform.Translate(DirectionToNext.normalized * speed/* + DirectionToLine.normalized * stickySpeed*/);
		//rb.AddForce(DirectionToNext.normalized * speed);
	}

    public IEnumerator Wait(float seconds)
    {
        float temp = speed;
        speed = 0.0f;
        yield return new WaitForSeconds(seconds);
        speed = temp;
        yield return null;

    }
    
}
#endif