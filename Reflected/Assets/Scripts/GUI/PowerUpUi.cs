using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUi : MonoBehaviour
{
    [SerializeField] GameObject UpgradeUiPanel;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        UpgradeUiPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if(player.) //
    }
}
