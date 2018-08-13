using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Game))]
public class GameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Spawn gray thought"))
        {
            instantiateThought(Game.instance.data.thoughtDiamond, verletter.JellyColor.GRAY, new Vector3(12, 11));
        }
        if (GUILayout.Button("Spawn red thought"))
        {
            instantiateThought(Game.instance.data.thoughtBigSquare, verletter.JellyColor.RED, new Vector3(12, 11));
        }
        if (GUILayout.Button("Spawn blue thought"))
        {
            instantiateThought(Game.instance.data.thoughtWideRectangle, verletter.JellyColor.BLUE, new Vector3(12, 11));
        }
        if (GUILayout.Button("Spawn green thought"))
        {
            instantiateThought(Game.instance.data.thoughtFish, verletter.JellyColor.GREEN, new Vector3(12, 11));
        }

        DrawDefaultInspector();
    }

    public void instantiateThought(GameObject thoughtPrefab, verletter.JellyColor color, Vector3 velocity)
    {
        GameObject thought = Instantiate(thoughtPrefab, Game.instance.thoughtsContainer);
        verletter jelly = thought.GetComponent<verletter>();
        if (jelly == null)
        {
            Destroy(thought);
            throw new System.Exception("failed to instanciate thought because prefab didn't have a verletter component, prefab: " + thoughtPrefab.ToString());
        }

        jelly.color = color;

        Vector3 offset = Game.instance.firemanBrain.position - jelly.centerPoint.position;
        offset.z = 0;
        foreach (Rigidbody rb in jelly.GetComponentsInChildren<Rigidbody>())
        {
            rb.position += offset;
            rb.velocity = velocity;
        }
    }
}