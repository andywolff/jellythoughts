using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan_texoffset : MonoBehaviour {

    public Vector2 texspeed = new Vector2(0, 0.001f);
	
	// Update is called once per frame
	void LateUpdate () {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTextureOffset = texspeed * Time.time;
    }
}
