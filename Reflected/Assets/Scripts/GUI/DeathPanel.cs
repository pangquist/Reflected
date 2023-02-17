using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    [Header("Room Info")]
    [SerializeField] TextMeshProUGUI killCountText;
    [SerializeField] TextMeshProUGUI clearedRoomsText;
    [SerializeField] TextMeshProUGUI averageTimeText;
    [SerializeField] GameObject deathPanel;
    private GameManager gameManager;
    private AiDirector aiDirector;

    private float minute;
    private float second;
    private float boolTimer;
    bool doOnce;
    // Start is called before the first frame update
    private void Awake()
    {
        aiDirector = FindObjectOfType<AiDirector>();
        gameManager = FindObjectOfType<GameManager>();

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
        
    }

    public void BackToMainMenu()
    {
        deathPanel.SetActive(false);
        Time.timeScale = 1;
        GameObject.Find("GameManager").GetComponent<GameManager>().Save();
        SceneManager.LoadScene(0);
    }

}
