using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjTrigger : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Box")
        {
            PhysicsObject obj = other.gameObject.GetComponent<PhysicsObject>();
            obj.ResetObj();
        }
    }
}
