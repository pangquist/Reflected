using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject closeButton;

    void Start()
    {
        
    }

    public void SetPanelActive()
    {
        tutorial.SetActive(true);
        closeButton.SetActive(true);
    }

    public void SetPanelInactive()
    {
        tutorial.SetActive(false);
    }
}
