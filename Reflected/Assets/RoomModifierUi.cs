using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomModifierUi : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI trueText;
    [SerializeField] TextMeshProUGUI mirrorText;
    [SerializeField] Image trueImage;
    [SerializeField] Image mirrorImage;
    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    // Start is called before the first frame update
    void Start()
    {
        trueText.gameObject.SetActive(false);
        mirrorText.gameObject.SetActive(false);
        trueImage.gameObject.SetActive(false);
        mirrorImage.gameObject.SetActive(false);
        Map.RoomEntered.AddListener(UpdateModifierUi);
        DimensionManager.DimensionChanged.AddListener(UpdateModifierUi);
    }

    // Update is called once per frame
    void UpdateModifierUi()
    {
        trueText.gameObject.SetActive(true);
        mirrorText.gameObject.SetActive(true);
        if (DimensionManager.CurrentDimension == Dimension.True)
        {
            trueText.color = activeColor;
            mirrorText.color = inactiveColor;
            trueImage.gameObject.SetActive(true);
            mirrorImage.gameObject.SetActive(false);
        }
        else if(DimensionManager.CurrentDimension == Dimension.Mirror)
        {
            trueText.color = inactiveColor;
            mirrorText.color = activeColor;
            trueImage.gameObject.SetActive(false);
            mirrorImage.gameObject.SetActive(true);
        }
        trueText.text = "True Stat: " + Map.ActiveRoom.TrueStat.ToString();
        mirrorText.text = "Mirror Stat: " + Map.ActiveRoom.MirrorStat.ToString();
    }
}
