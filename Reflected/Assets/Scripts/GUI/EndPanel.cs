using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class EndPanel : MonoBehaviour
{
    [Header("Room Info")]
    [SerializeField] TextMeshProUGUI killCountText;
    [SerializeField] TextMeshProUGUI clearedRoomsText;
    [SerializeField] TextMeshProUGUI averageTimeText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject endScreenObject;

    private GameManager gameManager;
    private AiDirector aiDirector;
    private float minute;
    private float second;
    private float boolTimer;
    bool doOnce;
    
    // Start is called before the first frame update
    void Awake()
    {
        Diamond.OnDiamondCollected += (ItemData itemData) => UpdateEndScreen();
        
        aiDirector = FindObjectOfType<AiDirector>();
    }


    private void UpdateEndScreen()
    {
        gameManager = FindObjectOfType<GameManager>();

        endScreenObject.SetActive(true);
        killCountText.text = "Kill Count: " + aiDirector.GetKillCount().ToString();
        clearedRoomsText.text = "Cleared Rooms: " + aiDirector.GetClearedRooms().ToString();
        averageTimeText.text = "Average Room Clear Time: " + aiDirector.GetAverageTime().ToString("0.00") + " s";


        doOnce = true;
        if (!doOnce && Mathf.Round(gameManager.GetRunTimer()) % 60 == 0)
        {
            minute++;
            doOnce = true;
        }

        if (doOnce)
        {
            boolTimer += Time.deltaTime;

            if (boolTimer >= 10)
            {
                doOnce = false;
                boolTimer = 0;
            }
        }

        second = gameManager.GetRunTimer() % 60;
        timerText.text = "Run Timer: " + minute.ToString() + "m " + second.ToString("0.0") + "s";
    }

    public void BackToMainMenu()
    {
        endScreenObject.SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().Save();
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        endScreenObject.SetActive(false);
        gameManager.NextMap();
    }

}
