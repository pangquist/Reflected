using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AiDirector aiDirector;
    [SerializeField] private EnemySpawner enemySpawner;

    public AiDirector AiDirector => aiDirector;
    public EnemySpawner EnemySpawner => enemySpawner;
}
