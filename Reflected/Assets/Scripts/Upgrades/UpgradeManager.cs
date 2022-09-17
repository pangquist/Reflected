using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Mirror lightMirror;
    [SerializeField] Mirror darkMirror;

    [SerializeField] List<MirrorPiece> allActivePieces = new List<MirrorPiece>();

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void GetActiveUpgrades()
    {
        foreach(MirrorPiece piece in lightMirror.GetActivePieces())
        {
            allActivePieces.Add(piece);
        }

        foreach (MirrorPiece piece in darkMirror.GetActivePieces())
        {
            allActivePieces.Add(piece);
        }
    }
}
