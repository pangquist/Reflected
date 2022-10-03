using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    TMP_Text uiText;
    void Start()
    {
        uiText = GetComponent<TMP_Text>();
    }

    public void ChangeText(string newText)
    {
        uiText.text = newText;
    }
}
