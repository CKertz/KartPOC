using System;
using UnityEngine;

public class Surface : ScriptableObject
{
    public bool isHarvestable;
    public float scoreModifier;
    public string surfaceName;
    public float totalScore = 0;
    enum surfaceType
    {
        Field,

    }
}