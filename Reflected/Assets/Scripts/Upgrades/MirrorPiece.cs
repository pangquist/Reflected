using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirrorPiece : MonoBehaviour
{
    [SerializeField] Sprite deactivatedSprite;
    [SerializeField] Sprite activatedSprite;
    Image currentImage;
    [SerializeField] Mirror mirror;
    [SerializeField] MirrorPiece neededPiece;

    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        currentImage = GetComponent<Image>();
        currentImage.sprite = deactivatedSprite;
    }

    public void PlaceInMirror()
    {
        if (neededPiece != null && !neededPiece.IsActive())
            return;

        mirror.PlacePiece(this);
        currentImage.sprite = activatedSprite;
        isActive = true;
    }

    public bool IsActive()
    {
        return isActive;
    }
}
