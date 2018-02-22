using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplePressurePlateEffects : AffectedByPressurePlate {
	public AffectedByPressurePlate[] ObjectsToTrigger;

	public override void OnPressed() {
		for(int i = 0; i < ObjectsToTrigger.Length; i++) {
			ObjectsToTrigger[i].OnPressed ();
		}
	}

	public override void OnUnpressed() {
		foreach (AffectedByPressurePlate obj in ObjectsToTrigger) {
			obj.OnUnpressed ();
		}
	}
}
