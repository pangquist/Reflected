using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AiDirector aiDirector;
    [SerializeField] private EnemySpawner enemySpawner;

    public AiDirector AiDirector => aiDirector;
    public EnemySpawner EnemySpawner => enemySpawner;

    private void Start()
    {
        GameObject.Find("Dimension Manager").GetComponent<DimensionManager>().FindSystems();
    }

    private void Update()
    {
    }
}
