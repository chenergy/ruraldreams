using UnityEngine;
using System.Collections;

public class playerRingScript : MonoBehaviour {
	
	// VARS
	public float RotationRate = 1.0f;
	public Color Color1 = new Color(1,0,1,1);
	public Color Color2 = new Color(0,0,1,1);
	
	// SIN TABLE
	private int num = 0;
	private float[] sinTable;
	private int sinTableSize = 120;
	
	// Use this for initialization
	void Start () {
		
		// CREATE SIN TABLE TO EASE OVERHEAD
		sinTable = new float[sinTableSize];
		for (int i =0; i < sinTableSize; i++)
		{
			sinTable[i] = Mathf.Sin(Mathf.PI*2 * (i * 1.0f/ sinTableSize))/2.0f + 0.5f;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// ROTATE
		transform.Rotate(0,RotationRate,0);
		
		// COLOR OSCILLATION
		num++;
		if (num >= sinTableSize) num -= sinTableSize;
		renderer.material.SetColor("_Tint", Color.Lerp(Color1, Color2, sinTable[num]));
	}
}
