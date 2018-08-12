using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class verletter : MonoBehaviour {

    MeshFilter meshFilter;
    PolygonCollider2D polygonCollider;
    public Transform centerPoint;
    public Transform[] edgePoints;
    private static Collider2D[] colliders = new Collider2D[10];
    private static ContactFilter2D contactFilter2D;

    private static GameObject tempCircleColliderGameObject;
    private static CircleCollider2D tempCircleCollider;
    private static float springForce = 2000f;

    // Use this for initialization
    void Start () {
        if (contactFilter2D.Equals(default(ContactFilter2D)))
        {
            contactFilter2D.SetLayerMask(1 << LayerMask.NameToLayer("jellymesh"));
        }

        if (tempCircleColliderGameObject == null)
        {
            tempCircleColliderGameObject = new GameObject("tempCircleCollider");
            tempCircleCollider = tempCircleColliderGameObject.AddComponent<CircleCollider2D>();
            tempCircleCollider.radius = 0.01f;
            tempCircleColliderGameObject.SetActive(false);
        }

        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        polygonCollider = GetComponentInChildren<PolygonCollider2D>();

        // Configure verletSpring component on children
        int i = 0;
        for (i = 0; i < rbs.Length; i++)
        {
            verletSpring spring = rbs[i].gameObject.GetComponent<verletSpring>();
            if (spring == null) spring = rbs[i].gameObject.AddComponent<verletSpring>();
            spring.springTargets = (from rb in rbs where rb != rbs[i] select rb.transform).ToArray<Transform>();
            spring.springForce = springForce;
            spring.init();
        }

        // Configure render mesh
        Mesh mesh = new Mesh();
        mesh.MarkDynamic();
        mesh.vertices = getMeshVertices();
        //mesh.uv = (from rb in rbs select Vector2.zero).ToArray<Vector2>();
        if (mesh.vertices.Length == 9)
        {
            // Stupid triangle ordering shit, TODO: automate this
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

        // configure collider polygon
        edgePoints = (from rb in GetComponentsInChildren<Rigidbody>()
                      where !rb.GetComponent<verletSpring>().isInnerVertex // select edge points
                      orderby -Mathf.Atan2(centerPoint.position.y - rb.position.y, centerPoint.position.x - rb.position.x) // order clockwise around center point
                      select rb.transform)
                     .ToArray<Transform>();
        updatePolygon();
	}

    private void Update()
    {
        meshFilter.mesh.vertices = getMeshVertices();
        updatePolygon();

        int numHits = polygonCollider.OverlapCollider(contactFilter2D, colliders);
        if (numHits > 0)
        {
            tempCircleColliderGameObject.SetActive(true);

            // map points to v2d
            List<Rigidbody> rigidBodies = GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();
            for (int i = 0; i < numHits; i++)
            {
                ColliderDistance2D overallDist = colliders[i].Distance(polygonCollider);
                Vector2 overallForce = -overallDist.normal * overallDist.distance * springForce;
                // Apply spring force to each overlapping point proportional to depth of overlap
                rigidBodies
                    .ForEach((Rigidbody rb) => {
                        Vector2 p = V2FromV3(rb.transform.position);
                        if (colliders[i].OverlapPoint(p))
                        {
                            tempCircleColliderGameObject.transform.position = rb.transform.position;
                            ColliderDistance2D dist = colliders[i].Distance(tempCircleCollider); // Find vector toward nearest non-overlapping position.
                            rb.AddForce(-dist.normal * dist.distance * springForce + overallForce); // Apply spring force toward freedom.
                            Debug.DrawLine(rb.position, rb.position - new Vector3(dist.normal.x, dist.normal.y, 0) * dist.distance);
                        }
                });
            }
            tempCircleColliderGameObject.SetActive(false);
        }
    }

    private Vector3[] getMeshVertices()
    {
        return (from rb in GetComponentsInChildren<Rigidbody>() select rb.transform.position).ToArray<Vector3>();
    }

    private void updatePolygon()
    {
        Vector2[] points = (from Transform t in edgePoints select new Vector2(t.position.x, t.position.y)).ToArray<Vector2>();
        polygonCollider.SetPath(0, points);
        polygonCollider.points = points;
    }

    private static Vector2 V2FromV3(Vector3 v) { return new Vector2(v.x, v.y); }
}
