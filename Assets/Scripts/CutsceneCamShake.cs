using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamShake : MonoBehaviour 
{
	[Header("cam shake")]
	public float heightScale = 1F;
	public float xScale = 1F;
	void Update() 
	{
		float height = heightScale * Mathf.PerlinNoise(Time.time * xScale, 1F);
		Vector3 pos = transform.position;
		pos.y = height;
		transform.position = pos;
	}
}