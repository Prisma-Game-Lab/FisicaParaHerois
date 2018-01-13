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


    public GameObject LinkPrefab;

    public float AnchorOffset;
    
    [SerializeField]
    [HideInInspector]
    private List<GameObject> _links = new List<GameObject>();

    // Use this for initialization
    void Awake () {

    }
	
	// Update is called once per frame
	void LateUpdate () {
		//check if bizarre bug
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

            _links.Add(link);
            return;
        }
        
        //adiciona um link
        GameObject newLink = CreateLink();
        HingeJoint2D joint = newLink.GetComponent<HingeJoint2D>();
        newLink.transform.localPosition = _links[_links.Count - 1].transform.localPosition + new Vector3(0, 2 * AnchorOffset);

        //customizar joint
        //conecta no rigidbody do link imediatamente anterior
        joint.connectedBody = _links[_links.Count - 1].GetComponent<Rigidbody2D>();

        //ajusta posição das âncoras
        //anchor deve ficar na pontinha do proprio link
        //connected anchor deve ficar na pontinha do link anterior
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = new Vector3(0, AnchorOffset);
        joint.connectedAnchor = - new Vector3(0, AnchorOffset);



        _links.Add(newLink);

        return;
    }

    public void RemoveLink()
    {

               
        DestroyImmediate(_links[_links.Count - 1]);
        _links.RemoveAt(_links.Count - 1);
        
    }

    //creates a link and sets the desired individual attributes
    private GameObject CreateLink()
    {
        GameObject link = GameObject.Instantiate(LinkPrefab, this.gameObject.transform);
        return link;
    }

}
