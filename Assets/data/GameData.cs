using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : ScriptableObject {
    public Material redMat;
    public Material greenMat;
    public Material blueMat;
    public Material grayMat;
    public Material redTargetMat;
    public Material greenTargetMat;
    public Material blueTargetMat;
    public Material brightRedTargetMat;
    public Material brightBlueTargetMat;
    public Material brightGreenTargetMat;

    public const string spacer = "-- prefabs --";
    // prefabs

    public GameObject thoughtDiamond;
    public GameObject thoughtBigSquare;
    public GameObject thoughtFish;
    public GameObject thoughtWeirdConcaveBottom;
    public GameObject thoughtWideRectangle;
}
