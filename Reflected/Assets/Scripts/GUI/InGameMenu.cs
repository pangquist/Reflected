using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    

    [SerializeField] GameObject InGameMenuObj;

    
    public void Continue()
    {
        InGameMenuObj.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
