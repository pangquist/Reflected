using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UiManager;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AiDirector aiDirector;
    [SerializeField] private EnemySpawner enemySpawner;
    private UiManager uiManager;
    private float runTimer;
    public AiDirector AiDirector => aiDirector;
    public EnemySpawner EnemySpawner => enemySpawner;
    
    private void Update()
    {
        if(uiManager.GetMenuState() != MenuState.Active)
            runTimer += Time.deltaTime;
    }
    private void Start()
    {
        GameObject.Find("Dimension Manager").GetComponent<DimensionManager>().FindSystems();
        runTimer = 0;
        uiManager = FindObjectOfType<UiManager>();
    }

    public void Save()
    {
        GameObject saveLoadSystem = GameObject.Find("SaveLoadSystem");

        if (saveLoadSystem)
            saveLoadSystem.GetComponent<SaveLoadSystem>().Save();
        else
            Debug.Log("No SaveLoadSystem found");
    }

    public float GetRunTimer()
    {
        return runTimer;
    }
}
