using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public verletter.JellyColor color = verletter.JellyColor.RED;
    public static Material getMatFromJellyColor(verletter.JellyColor c, bool highlight)
    {
        switch (c)
        {
            case verletter.JellyColor.RED: return highlight ? Game.instance.data.brightRedTargetMat : Game.instance.data.redTargetMat;
            case verletter.JellyColor.GREEN: return highlight ? Game.instance.data.brightGreenTargetMat : Game.instance.data.greenTargetMat;
            case verletter.JellyColor.BLUE: return highlight ? Game.instance.data.brightBlueTargetMat : Game.instance.data.blueTargetMat;
            default: throw new System.Exception("expected valid target jellycolor, but had "+c.ToString());
        }
    }

    private Pan_texoffset panner;
    private Vector2 initialPan;
    private BoxCollider boxCollider;
    private ParticleSystem particleSystem;

    public int numOverlaps = 0;

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material = getMatFromJellyColor(color, false);
        panner = GetComponent<Pan_texoffset>();
        if (panner != null) initialPan = panner.texspeed;
        boxCollider = GetComponent<BoxCollider>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        
        numOverlaps = 0;

        List<verletter> myJellies = Game.instance.jelliesByColor[color];
        foreach (verletter jelly in myJellies)
        {
            Rigidbody[] rbs = jelly.GetComponentsInChildren<Rigidbody>();
            int numPointsOverlapping = 0;
            foreach (Rigidbody rb in rbs)
            {
                if (boxCollider.bounds.Intersects(rb.GetComponent<Collider>().bounds)) numPointsOverlapping++;
            }
            if (numPointsOverlapping == rbs.Length) numOverlaps++;
        }


		if (panner != null  && initialPan != null)
        {
            panner.texspeed = initialPan * (numOverlaps>0 ? numOverlaps*-5 : 1);
        }

        if (numOverlaps > 0) particleSystem.Play();
        else particleSystem.Stop();

        GetComponent<Renderer>().material = getMatFromJellyColor(color, numOverlaps > 0);
    }
}
