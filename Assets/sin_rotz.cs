using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sin_rotz : MonoBehaviour {

    public float range = 0.1f;
    public float speed = 0.1f;
    public float center = 0f;
    
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * speed) * range);
	}
}
