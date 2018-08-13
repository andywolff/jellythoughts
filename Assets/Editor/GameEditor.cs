using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Game))]
public class GameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Spawn gray thought"))
        {
            Game.instance.instantiateThought(Game.instance.data.thoughtDiamond, verletter.JellyColor.GRAY);
        }
        if (GUILayout.Button("Spawn red thought"))
        {
            Game.instance.instantiateThought(Game.instance.data.thoughtBigSquare, verletter.JellyColor.RED);
        }
        if (GUILayout.Button("Spawn blue thought"))
        {
            Game.instance.instantiateThought(Game.instance.data.thoughtWideRectangle, verletter.JellyColor.BLUE);
        }
        if (GUILayout.Button("Spawn green thought"))
        {
            Game.instance.instantiateThought(Game.instance.data.thoughtFish, verletter.JellyColor.GREEN);
        }

        DrawDefaultInspector();
    }
}