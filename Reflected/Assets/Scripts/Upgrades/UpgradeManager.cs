using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Mirror lightMirror;
    [SerializeField] Mirror darkMirror;

    Dictionary<string, float> lightVariables;
    Dictionary<string, float> darkVariables;

    //[SerializeField] List<MirrorPiece> allActivePieces = new List<MirrorPiece>();

    [SerializeField] Player player;

    void Start()
    {
        lightVariables = new Dictionary<string, float>();
        darkVariables = new Dictionary<string, float>();
        DontDestroyOnLoad(this);
    }

    public void GetActiveUpgrades()
    {
        foreach (MirrorPiece piece in lightMirror.GetActivePieces())
        {
            if (!lightVariables.ContainsKey(piece.GetVariable()))
                lightVariables.Add(piece.GetVariable(), piece.GetValue());
            else
                lightVariables[piece.GetVariable()] += piece.GetValue();
        }

        foreach (MirrorPiece piece in darkMirror.GetActivePieces())
        {
            if (!darkVariables.ContainsKey(piece.GetVariable()))
                darkVariables.Add(piece.GetVariable(), piece.GetValue());
            else
                darkVariables[piece.GetVariable()] += piece.GetValue();
        }
    }

    public Dictionary<string, float> GetLightPieces()
    {


        return lightVariables;
    }

    public Dictionary<string, float> GetDarkPieces()
    {
        return darkVariables;
    }

    //public List<MirrorPiece> GetLightPieces()
    //{
    //    return lightMirror.GetActivePieces();
    //}

    //public List<MirrorPiece> GetDarkPieces()
    //{
    //    return darkMirror.GetActivePieces();
    //}

    public void AddPlayer(Player newPlayer)
    {
        player = newPlayer;
    }
}
