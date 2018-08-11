using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class cursor : MonoBehaviour {
    private GameObject outline;
    public float snapRadius = 2f;
    private Vector3 mousePos;
    public float dragForce = 200f;
    public float maxDragForce = 500f;

    private enum State
    {
        LOOKING_FOR_SNAP,
        DRAGGING,
    }
    private State state = State.LOOKING_FOR_SNAP;

    private Rigidbody dragTarget;

	// Use this for initialization
	void Start () {
        outline = transform.GetChild(0).gameObject;
        outline.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        mousePos.z = 1.8f;
        
        transform.position = mousePos;

        switch (state)
        {
            case State.LOOKING_FOR_SNAP: lookForSnap(); break;
            case State.DRAGGING: drag(); break;
        }
	}

    void lookForSnap()
    {
        Collider closest = Physics.OverlapSphere(
                mousePos,
                snapRadius,
                1 << LayerMask.NameToLayer("selectable"))
                .OrderBy((Collider c) => (c.transform.position - transform.position).sqrMagnitude)
                .FirstOrDefault();
        if (closest != null)
        {
            transform.position = closest.transform.position;
            outline.SetActive(true);

            if (Input.GetMouseButtonDown(0)) startDrag(closest);
        }
        else
        {
            outline.SetActive(false);
        }
    }

    void startDrag(Collider collider)
    {
        dragTarget = collider.gameObject.GetComponent<Rigidbody>();
        if (dragTarget == null)
        {
            return;
        }
        state = State.DRAGGING;
    }

    void drag()
    {
        if (!Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)
            || dragTarget == null || !dragTarget.gameObject.activeInHierarchy)
        {
            // exit dragging 
            outline.SetActive(false);
            state = State.LOOKING_FOR_SNAP;
        } else
        {
            transform.position = dragTarget.transform.position;
            outline.SetActive(true);

            // drag target
            Vector3 delta = mousePos - dragTarget.position;
            delta.z = 0;
            dragTarget.AddForce(Vector3.ClampMagnitude(delta * dragForce, maxDragForce));
        }
    }
}
