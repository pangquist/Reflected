using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirrorPiece : MonoBehaviour
{
    [SerializeField] Sprite deactivatedSprite;
    [SerializeField] Sprite canBeActivatedSprite;
    [SerializeField] Sprite activatedSprite;
    [SerializeField] Mirror mirror;
    [SerializeField] List<MirrorPiece> nextPieces = new List<MirrorPiece>();
    Image currentImage;

    [SerializeField] float resourceCost;
    [SerializeField] bool isPlaceable;
    bool isActive;
    
    [Header("Stat Changes")]
    [SerializeField] string modifiedValue;
    [SerializeField] float value;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        currentImage = GetComponent<Image>();
        currentImage.sprite = deactivatedSprite;
    }

    public void PlaceInMirror()
    {
        if (!isPlaceable) // || resourceCost > resourceAmount
            return;

        mirror.PlacePiece(this);
        currentImage.sprite = activatedSprite;

        foreach(MirrorPiece piece in nextPieces)
        {
            piece.SetIsPlaceable(true);
        }

        // resourceAmount -= resourceCost;
    }

    public void SetIsActive(bool state)
    {
        isActive = state;
    }

    public void SetIsPlaceable(bool state)
    {
        isPlaceable = state;
        currentImage.sprite = canBeActivatedSprite;
    }

    public bool IsActive()
    {
        return isActive;
    }
}
