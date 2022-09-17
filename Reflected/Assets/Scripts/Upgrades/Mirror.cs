using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] List<MirrorPiece> activePieces = new List<MirrorPiece>();
    List<MirrorPiece> allPieces = new List<MirrorPiece>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlacePiece(MirrorPiece newPiece)
    {
        activePieces.Add(newPiece);
    }

    public List<MirrorPiece> GetActivePieces()
    {
        return activePieces;
    }
}
