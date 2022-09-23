using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TabScript : MonoBehaviour
{
    [SerializeField] GameObject uiPanel;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            {
            uiPanel.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Tab))
            {
            uiPanel.SetActive(false);
        }
    }
}
