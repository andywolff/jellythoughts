using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class verletter : MonoBehaviour {

    MeshFilter meshFilter;
    MeshCollider meshCollider;

// Use this for initialization
    void Start () {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        int i = 0;
        for (i = 0; i < rbs.Length; i++)
        {
            verletSpring spring = rbs[i].gameObject.AddComponent<verletSpring>();
            spring.springTargets = (from rb in rbs where rb != rbs[i] select rb.transform).ToArray<Transform>();
            spring.springForce = 2000f;
        }

        Mesh mesh = new Mesh();
        mesh.MarkDynamic();
        mesh.vertices = getMeshVertices();
        //mesh.uv = (from rb in rbs select Vector2.zero).ToArray<Vector2>();
        if (mesh.vertices.Length == 9)
        {
            mesh.triangles = new int[] {
                0,3,1,
                1,3,4,
                1,4,2,
                2,4,5,
                3,6,4,
                4,6,7,
                4,7,5,
                5,7,8,
            };
        } else if (mesh.vertices.Length == 15)
        {
            mesh.triangles = new int[] {
                0,5,1,
                1,5,6,
                1,6,2,
                2,6,7,
                2,7,3,
                3,7,8,
                4,3,8,
                4,8,9,
                5,10,6,
                6,10,11,
                7,6,11,
                7,11,12,
                8,7,12,
                8,12,13,
                9,8,13,
                9,13,14,
            };
        }
        meshFilter.mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null) meshCollider.sharedMesh = mesh;
	}

    private void Update()
    {
        meshFilter.mesh.vertices = getMeshVertices();
        if (meshCollider != null) meshCollider.sharedMesh = meshFilter.mesh;
    }

    private Vector3[] getMeshVertices()
    {
        return (from rb in GetComponentsInChildren<Rigidbody>() select rb.transform.position).ToArray<Vector3>();
    }
}
