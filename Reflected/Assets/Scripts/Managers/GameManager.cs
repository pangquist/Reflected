using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] mapGenerators;
    [SerializeField] private int currentMapGenerator;

    [SerializeField] private AiDirector aiDirector;
    [SerializeField] private EnemySpawner enemySpawner;
    private UiManager uiManager;
    private float runTimer;
    public AiDirector AiDirector => aiDirector;
    public EnemySpawner EnemySpawner => enemySpawner;

    public static UnityEvent DestroyingMap = new UnityEvent();

    private void Awake()
    {
        Diamond.OnDiamondCollected += (ItemData itemData) => NextMap(); 

        GameObject mapGenerator = Instantiate(mapGenerators[currentMapGenerator]);
        mapGenerator.name = "Map Generator";
        mapGenerator.transform.SetSiblingIndex(0);
    }

    private void Start()
    {
        GameObject.Find("Dimension Manager").GetComponent<DimensionManager>().FindSystems();
        uiManager = FindObjectOfType<UiManager>();
    }

    private void Update()
    {
        if (uiManager.GetMenuState() != UiManager.MenuState.Active)
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

    [ContextMenu("Next map")]
    private void NextMap()
    {
        StartCoroutine(Coroutine_NextMap());
    }

    private IEnumerator Coroutine_NextMap()
    {
        // Fade screen from transparent to black

        Color fromColor = new Color(0f, 0f, 0f, 0f);
        Color toColor = new Color(0f, 0f, 0f, 1f);

        float duration = 3f;
        float timer = 0f;

        while ((timer += Time.deltaTime) < duration)
        {
            uiManager.Tint.color = Color.Lerp(fromColor, toColor, Mathf.Pow(timer / duration, 4f));
            yield return null;
        }

        // Display text

        uiManager.Tint.color = toColor;
        uiManager.TintText.gameObject.SetActive(true);
        uiManager.TintText.text = "New map generating...";
        yield return null;

        // Destroy map

        DestroyingMap.Invoke();
        Destroy(GameObject.Find("Map"));
        yield return null;

        // Next map generator

        MapGenerator mapGenerator = GameObject.Find("Map Generator").GetComponent<MapGenerator>();
        int oldSeed = mapGenerator.seed;
        currentMapGenerator = Mathf.Min(currentMapGenerator + 1, mapGenerators.Length - 1);
        Destroy(mapGenerator.gameObject);

        mapGenerator = Instantiate(mapGenerators[currentMapGenerator]).GetComponent<MapGenerator>();
        mapGenerator.name = "Map Generator";
        mapGenerator.transform.SetSiblingIndex(0);
        mapGenerator.seed = oldSeed + 1;
        yield return null;

        // (The map generator will generate a new map using its Start method)

        uiManager.TintText.gameObject.SetActive(false);
        yield return 0;
    }
}
