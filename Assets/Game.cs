using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public static Game instance = null;

    public GameData data;
    public Dictionary<verletter.JellyColor, List<verletter>> jelliesByColor = new Dictionary<verletter.JellyColor, List<verletter>>()
    {
        {verletter.JellyColor.RED, new List<verletter>() },
        {verletter.JellyColor.GREEN, new List<verletter>() },
        {verletter.JellyColor.BLUE, new List<verletter>() },
        {verletter.JellyColor.GRAY, new List<verletter>() },
    };
    public void addJelly(verletter jelly)
    {
        jelliesByColor[jelly.color].Add(jelly);
    }
    public void removeJelly(verletter jelly)
    {
        jelliesByColor[jelly.color].Remove(jelly);
    }

    public Transform thoughtsContainer;
    public Transform firemanBrain;

    public ParticleSystem thoughtExplodeRed;
    public ParticleSystem thoughtExplodeGreen;
    public ParticleSystem thoughtExplodeBlue;
    public ParticleSystem thoughtExplodeGray;
    public ParticleSystem getParticleSystemByColor(verletter.JellyColor color)
    {
        switch (color)
        {
            case verletter.JellyColor.RED: return thoughtExplodeRed;
            case verletter.JellyColor.GREEN: return thoughtExplodeGreen;
            case verletter.JellyColor.BLUE: return thoughtExplodeBlue;
            default: return thoughtExplodeGray;
        }
    }

    public void spawnParticles(verletter.JellyColor color, Rigidbody centerBody)
    {
        ParticleSystem ps = getParticleSystemByColor(color);
        ps.transform.position = centerBody.position;
        ps.GetComponent<Rigidbody>().velocity = centerBody.velocity;
        
        ps.Emit(100);

        ps.transform.position = Vector3.zero;
    }


    public void Awake()
    {
        // There can be only one.
        if (instance == null) { instance = this; initGame(); }
        else if (instance != this) { Destroy(gameObject); return; }
    }

    public void initGame()
    {

    }
}
