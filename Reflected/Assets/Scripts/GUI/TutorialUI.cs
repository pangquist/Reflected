using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    

    void Start()
    {
        
    }

    public void SetPanelActive()
    {
        tutorial.SetActive(true);
    }

    public void SetPanelInactive()
    {
        tutorial.SetActive(false);
    }
}
