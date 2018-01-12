using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {

    public int LinkQuantity
    {
        get {
                return _links.Count;
            }
    }

    [SerializeField]
    [HideInInspector]
    private Vector3 _linkScale;
    [SerializeField]
    [HideInInspector]
    private float _linkMass;
    [SerializeField]
    [HideInInspector]
    private float _linkBreakForce;

    
    public Vector3 LinkScale
    {
        get { return _linkScale; }
        set
        {
            _links.ForEach((GameObject obj) => obj.transform.localScale = value);
            _linkScale = value;
        }
    }

    public float LinkMass
    {
        get { return _linkMass; }
        set
        {
            _links.ForEach((GameObject obj) => obj.GetComponent<Rigidbody2D>().mass = value);
            _linkMass = value;
        }
    }

    public float LinkBreakForce
    {
        get { return _linkBreakForce; }
        set
        {
            _links.ForEach(delegate (GameObject obj)
            {
                HingeJoint2D hj = obj.GetComponent<HingeJoint2D>();
                //check for null necessary because hinge may have been broken
                if (hj != null) hj.breakForce = value;
            });
            _linkBreakForce = value;
        }
    }

    public GameObject LinkPrefab;

    //public float AnchorOffset;
    //public float ConnectedAnchorOffset;

    [SerializeField]
    [HideInInspector]
    private List<GameObject> _links = new List<GameObject>();

    // Use this for initialization
    void Awake () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddLink()
    {
        //caso: adicionando primeiro link
        if (_links.Count == 0)
        {
            //adiciona na posição do pai
            GameObject link = CreateLink();
            HingeJoint2D hj =  link.GetComponent<HingeJoint2D>();

            //customizar primeira hingejoint
            //se o objeto pai tem um rigidbody, conecta a ele
            if (this.gameObject.GetComponent<Rigidbody2D>() != null) hj.connectedBody = this.gameObject.GetComponent<Rigidbody2D>();

            //ajusta posição das âncoras
            //(âncora do primeiro deve ficar na pontinha dele

            hj.autoConfigureConnectedAnchor = false;
            hj.anchor = Vector2.up;
            hj.connectedAnchor = link.transform.position + link.transform.up;

            _links.Add(link);
            return;
        }
        
        //adiciona um link
        GameObject newLink = CreateLink();
        HingeJoint2D joint = newLink.GetComponent<HingeJoint2D>();

        //customizar joint
        //conecta no rigidbody do link imediatamente anterior
        joint.connectedBody = _links[_links.Count - 1].GetComponent<Rigidbody2D>();

        //ajusta posição das âncoras
        //anchor deve ficar na pontinha do proprio link
        //connected anchor deve ficar na pontinha do link anterior
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = newLink.transform.up;
        joint.connectedAnchor = - newLink.transform.up;



        _links.Add(newLink);

        return;
    }

    public void RemoveLink()
    {

        if(_links.Count > 2)
        {
            DestroyImmediate(_links[_links.Count - 1]);
            _links.RemoveAt(_links.Count - 1);
        }
    }

    //creates a link and sets the desired individual attributes
    private GameObject CreateLink()
    {
        GameObject link = GameObject.Instantiate(LinkPrefab, this.gameObject.transform);
        link.transform.localScale = _linkScale;
        link.GetComponent<Rigidbody2D>().mass = _linkMass;
        HingeJoint2D hj = link.AddComponent<HingeJoint2D>();
        hj.breakForce = _linkBreakForce;

        return link;
    }

}
