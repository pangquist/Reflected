using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Mirror lightMirror;
    [SerializeField] Mirror darkMirror;

    [SerializeField] List<MirrorPiece> allActivePieces = new List<MirrorPiece>();
    [SerializeField] List<MirrorPiece> lightActivePieces = new List<MirrorPiece>();
    [SerializeField] List<MirrorPiece> darkActivePieces = new List<MirrorPiece>();

    //Light side modifiers
    float lightHealthModifier = 1;
    float lightDamageModifier = 1;
    float lightMovementSpeedModifier = 1;
    float lightChargesToSwap;

    //dark side modifiers
    float darkHealthModifier = 1;
    float darkDamageModifier = 1;
    float darkMovementSpeedModifier = 1;
    float darkChargesToSwap;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void GetActiveUpgrades()
    {
        foreach (MirrorPiece piece in lightMirror.GetActivePieces())
        {
            lightActivePieces.Add(piece);
        }

        foreach (MirrorPiece piece in darkMirror.GetActivePieces())
        {
            darkActivePieces.Add(piece);
        }
    }

    public float ModifyLightStats(int index)
    {
        if (index < lightActivePieces.Count)
            return lightActivePieces[index].GetValue();
        else
            return 0;
    }

    public float ModifyDarkDamage()
    {
        return darkDamageModifier;
    }
}
