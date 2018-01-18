using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {

    public int LinkQuantity
    {
        get
        {
            if (_links == null)
            {
                _links = new List<GameObject>();
            }
            return _links.Count;
        }
    }


    private HingeJoint2D firstlinkHJ;
    private DistanceJoint2D firstlinkDJ;

    public float ForceExerted
    {
        get
        {
            if (firstlinkHJ != null && firstlinkDJ != null)
            {
                Vector2 force = firstlinkHJ.GetReactionForce(Time.deltaTime) + firstlinkDJ.GetReactionForce(Time.deltaTime);
                return force.magnitude;
            }
            else return 0;
        }
    }


    public GameObject LinkPrefab;

    public float AnchorOffset;
    public float LinkInitialDistance;
    public float DistanceTolerance;

    [HideInInspector]
    [SerializeField]
    private float _breakForce;
    public float BreakForce
    {
        get { return _breakForce; }
        set { if (value >= 0)
            {
                _breakForce = value;
                //reset values
                ResetValues();
            }
        }
    }

    //usada pra informar se a corda já foi quebrada por código
    private bool _broken;
    
    [SerializeField]
    [HideInInspector]
    public List<GameObject> _links;

    // Use this for initialization
    void Awake () {
        _broken = false;
        if(_links != null && _links.Count > 0)
        {
            firstlinkHJ = _links[0].GetComponent<HingeJoint2D>();
            firstlinkDJ = _links[0].GetComponent<DistanceJoint2D>();
        }
        
    }
	
	// Update is called once per frame
	void LateUpdate () {
		//check if bizarre bug
        //checa se dois links estão distantes demais entre si: testa os primeiros links, os últimos e uns do meio

        if (_links.Count >= 2)
        {
            if (Vector3.Distance(_links[0].transform.position + _links[0].transform.up * AnchorOffset, _links[1].transform.position + _links[1].transform.up * AnchorOffset) > DistanceTolerance)
            {
                BreakRope();
            }

            int n = _links.Count;
            if (Vector3.Distance(_links[n - 1].transform.position + _links[n-1].transform.up * AnchorOffset, _links[n-2].transform.position + _links[n-2].transform.up * AnchorOffset) > DistanceTolerance)
            {
                BreakRope();
            }
            n = 1 + _links.Count / 2;
            if (Vector3.Distance(_links[n - 1].transform.position + _links[n - 1].transform.up * AnchorOffset, _links[n - 2].transform.position + _links[n - 2].transform.up * AnchorOffset) > DistanceTolerance)
            {
                BreakRope();
            }


        }

	}

    public void BreakRope()
    {
        if (_broken) return;
        else _broken = true;

        Debug.Log("break rope");

        //quebra o distance joint do primeiro link:
        FirstLink fl = _links[0].GetComponent<FirstLink>();
        if (fl != null) Destroy(fl);
        DistanceJoint2D dj = _links[0].GetComponent<DistanceJoint2D>();
        if (dj != null) Destroy(dj);
        
        //percorre todos os links e quebra seu hingejoint2d
        _links.ForEach(delegate(GameObject obj)
        {
            Destroy(obj.GetComponent<HingeJoint2D>());
            SpringJoint2D spr = obj.GetComponent<SpringJoint2D>();
            if (spr != null) Destroy(spr);
            obj.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        });
    }

    public void AddLink()
    {
        
        
        //caso: adicionando primeiro link
        if (_links.Count == 0)
        {
            //adiciona na posição do pai
            GameObject link = CreateLink();
            HingeJoint2D hj =  link.GetComponent<HingeJoint2D>();
            link.transform.localPosition = Vector3.zero;

            //customizar primeira hingejoint
            //se o objeto pai tem um rigidbody, conecta a ele
            if (this.gameObject.GetComponent<Rigidbody2D>() != null) hj.connectedBody = this.gameObject.GetComponent<Rigidbody2D>();

            //ajusta posição das âncoras
            //(âncora do primeiro deve ficar na pontinha dele

            hj.autoConfigureConnectedAnchor = true;
            hj.anchor = new Vector3(0, AnchorOffset);
            //hj.connectedAnchor = new Vector3(0, AnchorOffset);
            //hj.breakForce = Mathf.Infinity???;

            //adiciona um distance joint
            DistanceJoint2D dj2 = link.AddComponent<DistanceJoint2D>();
            dj2.maxDistanceOnly = true;
            dj2.breakForce = BreakForce;

            //adiciona o script especifico do primeiro link:
            link.AddComponent<FirstLink>();


            _links.Add(link);
            return;
        }
        
        //adiciona um link
        GameObject newLink = CreateLink();
        HingeJoint2D joint = newLink.GetComponent<HingeJoint2D>();
        
        newLink.transform.localPosition = _links[_links.Count - 1].transform.localPosition - _links[_links.Count - 1].transform.up * 2 * LinkInitialDistance;

        //customizar joint
        //conecta no rigidbody do link imediatamente anterior
        joint.connectedBody = _links[_links.Count - 1].GetComponent<Rigidbody2D>();
        

        //ajusta posição das âncoras
        //anchor deve ficar na pontinha do proprio link
        //connected anchor deve ficar na pontinha do link anterior
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = new Vector3(0, AnchorOffset);
        joint.connectedAnchor = - new Vector3(0, AnchorOffset);
        joint.breakForce = BreakForce;

        //conecta o distancejoint2d do primeiro link neste link
        DistanceJoint2D dj = _links[0].GetComponent<DistanceJoint2D>();
        dj.connectedBody = newLink.GetComponent<Rigidbody2D>();
        dj.anchor = new Vector3(0, AnchorOffset);
        dj.connectedAnchor = - new Vector3(0, AnchorOffset);
        dj.autoConfigureDistance = true;

        _links.Add(newLink);

        return;
    }

    public void RemoveLink()
    {

        if(_links != null && _links.Count > 0)
        {
            DestroyImmediate(_links[_links.Count - 1]);
            _links.RemoveAt(_links.Count - 1);

        }

    }

    //creates a link and sets the desired individual attributes
    private GameObject CreateLink()
    {
        GameObject link = GameObject.Instantiate(LinkPrefab, this.gameObject.transform);
        return link;
    }

    //resets values for:
    //break force
    private void ResetValues()
    {
        if(_links != null && _links.Count > 0)
        {
            //break force do distance joint
            DistanceJoint2D dj = _links[0].GetComponent<DistanceJoint2D>();
            if(dj != null) dj.breakForce = _breakForce;

            _links.ForEach(delegate (GameObject obj)
                {
                    HingeJoint2D hj = obj.GetComponent<HingeJoint2D>();
                    if(hj != null) hj.breakForce = _breakForce;
                });
        }


    }
}
