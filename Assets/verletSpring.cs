using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class verletSpring : MonoBehaviour {

    public Transform[] springTargets;
    public float[] targetDistance;
    public float springForce = 1f;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        targetDistance = new float[springTargets.Length];
        int i = 0;
        foreach (Transform t in springTargets)
        {
            targetDistance[i] = (t.position - transform.position).magnitude;
            i++;
        }
	}

    void FixedUpdate()
    {
        int i = 0;
        foreach (Transform t in springTargets)
        {
            Vector3 offset = (t.position - transform.position);
            Vector3 desiredOffset = offset.normalized * targetDistance[i];
            Vector3 delta = offset - desiredOffset;
            i++;
            if (Mathf.Abs(delta.magnitude) > 0.01f)
            {
                rb.AddForce(delta * springForce);
            }
        }
    }




    private void OnDrawGizmos()
    {
        foreach (Transform t in springTargets) Gizmos.DrawLine(transform.position, t.position);
    }
}
