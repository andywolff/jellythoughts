using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Text text;
    public Transform targetRed;
    public Vector3 targetRed_desiredScale;
    public Transform targetGreen;
    public Vector3 targetGreen_desiredScale;
    public Transform targetBlue;
    public Vector3 targetBlue_desiredScale;

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
        StartCoroutine(level1Script());
    }

    public void Update()
    {
        targetRed.localScale = Vector3.Lerp(targetRed.localScale, targetRed_desiredScale, 0.1f);
        targetGreen.localScale = Vector3.Lerp(targetGreen.localScale, targetGreen_desiredScale, 0.1f);
        targetBlue.localScale = Vector3.Lerp(targetBlue.localScale, targetBlue_desiredScale, 0.1f);
    }


    const string redColorHex = "#F43434";
    const string redStart = "<b><color=" + redColorHex + ">";
    const string grayColorHex = "#363145";
    const string grayStart = "<b><color=" + grayColorHex + ">";
    const string cbEnd = "</color></b>";
    const string yellowColorHex = "#BD9512";
    const string yellowStart = "<b><color=" + yellowColorHex + ">";
    const string ctc = "<b>>></b>";

    private IEnumerator level1Script()
    {
        Vector3 initialTargetRedScale = targetRed.localScale;
        targetRed_desiredScale = targetRed.localScale = Vector3.zero;
        Vector3 initialTargetGreenScale = targetGreen.localScale;
        targetGreen_desiredScale = targetGreen.localScale = Vector3.zero;
        Vector3 initialTargetBlueScale = targetBlue.localScale;
        targetBlue_desiredScale = targetBlue.localScale = Vector3.zero;


        text.text = "Click anywhere to begin. \n\n\"" + ctc + "\" means the game will wait for another click to continue.";
        yield return waitForClick();

        text.text = "You are controlling the brain of a " + redStart + "fireman" + cbEnd + "."
            + "\nThere are <b><color=" + grayColorHex + ">many distractions</color></b> during his work.";
        yield return waitForClick();

        text.text = "Your job is to <i>focus</i> on " + redStart + "important thoughts" + cbEnd
            + " and <i>forget</i> " + grayStart + "distractions" + cbEnd + ".";
        yield return waitForClick();

        // try it now. *spawn thought*. *wait for task completion or failue. retry on failure*
        yield return tryItNowFocusRed(initialTargetRedScale);
        
        jelliesByColor[verletter.JellyColor.RED].ForEach((verletter v) => v.OnHitDestroyTrigger());

        text.text = "Very good. You succesfully focused on the thought "+redStart+"\"Gotta go to work\""+cbEnd+".";
        yield return new WaitForSeconds(0.5f);
        yield return waitForClick();

        text.text = "Now let's try dealing with some "+grayStart+"distractions"+cbEnd+".";
        yield return waitForClick();

        text.text = grayStart + "Distractions" + cbEnd + " come in many forms, and there are usually a lot of them at once.";
        yield return waitForClick();

        text.text = "To <i>forget</i> a thought, you can either throw it out the side of the brain, or use the " + yellowStart + "yellow ball" + cbEnd + " to suppress the thought."
            + "\n Try <i>forgetting</i> these "+grayStart+"distractions"+cbEnd+".";

        for (int i = 0; i<10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            instantiateThought(data.thoughtDiamond, verletter.JellyColor.GRAY);
        }
        
        yield return new WaitUntil(noGrayJellyExists);

        text.text = "Very good. You successfully forgot about various distractions.";
        yield return waitForClick();

        text.text = "That's as far as I got for for LD42. I'll spawn some more jelly thoughts for you.";

        targetRed_desiredScale = initialTargetRedScale;
        targetBlue_desiredScale = initialTargetBlueScale;
        targetGreen_desiredScale = initialTargetGreenScale;
        
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.2f);
            instantiateThought(data.thoughtBigSquare, verletter.JellyColor.RED);
        }

        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.2f);
            instantiateThought(data.thoughtFish, verletter.JellyColor.GREEN);
        }
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.2f);
            instantiateThought(data.thoughtWeirdConcaveBottom, verletter.JellyColor.GREEN);
        }
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.2f);
            instantiateThought(data.thoughtWeirdConcaveBottom, verletter.JellyColor.GREEN);
        }
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.2f);
            instantiateThought(data.thoughtWideRectangle, verletter.JellyColor.BLUE);
        }

        text.text += "Thanks for playing.";
    }

    IEnumerator tryItNowFocusRed(Vector3 initialTargetRedScale) {
        text.text = "Try it now. <i>Focus</i> on this " + redStart + "important thought" + cbEnd + " by dragging it to the highlighted area of the brain.";
        instantiateThought(data.thoughtBigSquare, verletter.JellyColor.RED);
        targetRed_desiredScale = initialTargetRedScale;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => redTargetHasOverlap() || noRedJellyExists());
        if (noRedJellyExists())
        {
            text.text = "Oh no! You <i>forgot</i> the important thought! That could get our fireman friend into lots of trouble. For now, let's try again.";
            yield return waitForClick();
            yield return tryItNowFocusRed(initialTargetRedScale);
        }
    }

    private bool redTargetHasOverlap() { return targetRed.GetComponent<Target>().numOverlaps > 0; }
    private bool noRedJellyExists() { return jelliesByColor[verletter.JellyColor.RED].Count == 0; }
    private bool noGrayJellyExists() { return jelliesByColor[verletter.JellyColor.GRAY].Count == 0; }

    private IEnumerator waitForClick() {
        text.text += "\n"+ctc;
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }
    
    public void instantiateThought(GameObject thoughtPrefab, verletter.JellyColor color)
    {
        Vector3 velocity = new Vector3(12, 11);
        GameObject thought = Instantiate(thoughtPrefab, thoughtsContainer);
        verletter jelly = thought.GetComponent<verletter>();
        if (jelly == null)
        {
            Destroy(thought);
            throw new System.Exception("failed to instanciate thought because prefab didn't have a verletter component, prefab: " + thoughtPrefab.ToString());
        }

        jelly.color = color;

        Vector3 offset = firemanBrain.position - jelly.centerPoint.position;
        offset.z = 0;
        foreach (Rigidbody rb in jelly.GetComponentsInChildren<Rigidbody>())
        {
            rb.position += offset;
            rb.velocity = velocity;
        }
    }
}
