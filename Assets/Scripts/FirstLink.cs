using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]
public class FirstLink : MonoBehaviour {

    private Chain c;

    // Use this for initialization
	void Awake () {
        c = this.gameObject.GetComponentInParent<Chain>();
	}

    void OnJointBreak2D(Joint2D brokenJoint)
    {
        //Debug.Log("A joint has just been broken!");
        //Debug.Log("The broken joint exerted a reaction force of " + brokenJoint.reactionForce);
        //Debug.Log("The broken joint exerted a reaction torque of " + brokenJoint.reactionTorque);
        if (c != null) c.BreakRope();

    }
}
