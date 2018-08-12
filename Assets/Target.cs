using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public float texspeed = 0.001f;
    private Material mat;
	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
        mat.mainTextureOffset = mat.mainTextureOffset + Vector2.down * texspeed;
    }
}
