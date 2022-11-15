using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using UnityEditor.Rendering.LookDev;

public class UpgradeButton : MonoBehaviour
{

    [SerializeField] GameObject button;
    [SerializeField] int index;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descText;
   

    public void SetButton(GameObject powerUp, int buttonIndex)
    {
        index = buttonIndex;
        titleText.text = powerUp.GetComponent<IBuyable>().GetValue().ToString();
        //titleText.text = powerUp.GetComponent<IBuyable>().
        descText.text = powerUp.GetComponent<IBuyable>().GetDescription();

    }
}
