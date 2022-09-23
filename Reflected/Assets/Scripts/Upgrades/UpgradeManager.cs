using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Mirror lightMirror;
    [SerializeField] Mirror darkMirror;

    [SerializeField] List<MirrorPiece> allActivePieces = new List<MirrorPiece>();

    [SerializeField] Player player;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void GetActiveUpgrades()
    {
        foreach (MirrorPiece piece in lightMirror.GetActivePieces())
        {
            allActivePieces.Add(piece);
        }

        foreach (MirrorPiece piece in darkMirror.GetActivePieces())
        {
            allActivePieces.Add(piece);
        }
    }

    public List<MirrorPiece> GetLightPieces()
    {
        return lightMirror.GetActivePieces();
    }

    public List<MirrorPiece> GetDarkPieces()
    {
        return darkMirror.GetActivePieces();
    }

    public void AddPlayer(Player newPlayer)
    {
        player = newPlayer;
    }
}
