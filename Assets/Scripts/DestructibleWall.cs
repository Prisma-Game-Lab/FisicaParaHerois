using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : AffectedByPressurePlate {
    public List<GameObject> CollidersToDisable; 

	public override void OnPressed () 
    {
        SetActive(false);
	}
	
    public override void OnUnpressed()
    {
        SetActive(true);	
	}

    void SetActive(bool status)
    {
        foreach (GameObject collider in CollidersToDisable)
        {
            collider.SetActive(status);
        }
    }
}
