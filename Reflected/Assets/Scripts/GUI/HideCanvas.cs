using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCanvas : MonoBehaviour
{
    private static bool hidden;

    private void OnEnable()
    {
        GetComponent<Canvas>().targetDisplay = hidden ? 7 : 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            bool wasHidden = GetComponent<Canvas>().targetDisplay == 7;
            hidden = !wasHidden;
            GetComponent<Canvas>().targetDisplay = hidden ? 7 : 0;
        }
    }

}
