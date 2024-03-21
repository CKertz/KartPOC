using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Surface")]
public class Surface : ScriptableObject
{
    public bool isHarvestable;
    public float scoreModifier;
    public string surfaceName;
    private float totalScore = 0;
    enum surfaceType
    {
        EmptyField,
        LandingZone,
        CropA,
        CropB
    }
}