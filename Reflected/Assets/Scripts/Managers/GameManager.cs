using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AiDirector aiDirector;
    [SerializeField] private EnemySpawner enemySpawner;
    private UiManager uiManager;
    private float runTimer;
    public AiDirector AiDirector => aiDirector;
    public EnemySpawner EnemySpawner => enemySpawner;

    public static UnityEvent NewMap = new UnityEvent();

    private void Start()
    {
        GameObject.Find("Dimension Manager").GetComponent<DimensionManager>().FindSystems();
        uiManager = FindObjectOfType<UiManager>();
    }
    private void Update()
    {
        if(uiManager.GetMenuState() != UiManager.MenuState.Active)
        {
            runTimer += Time.deltaTime;
        }
    }

    public void Save()
    {
        GameObject saveLoadSystem = GameObject.Find("SaveLoadSystem");
        GameObject inventory = GameObject.Find("Inventory");
        if (inventory)
            inventory.GetComponent<Inventory>().ResetTemporaryCollectables();
            
        if (saveLoadSystem)
            saveLoadSystem.GetComponent<SaveLoadSystem>().Save();
        else
            Debug.Log("No SaveLoadSystem found");
    }

    public float GetRunTimer()
    {
        return runTimer;
    }

    [ContextMenu("New Map")]
    public void NextMap()
    {
        NewMap.Invoke();
        GameObject.Find("Map Generator").GetComponent<MapGenerator>().Generate();
    }
}
