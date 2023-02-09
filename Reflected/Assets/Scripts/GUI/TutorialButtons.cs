using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialButtons : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI movementText;
    [SerializeField] TextMeshProUGUI combatText;
    [SerializeField] TextMeshProUGUI lootText;
    [SerializeField] TextMeshProUGUI dimensionText;
    [SerializeField] TextMeshProUGUI goalText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMovementTextActive()
    {
        movementText.gameObject.SetActive(!movementText.IsActive());
    }
    public void ChangeCombatTextActive()
    {
        combatText.gameObject.SetActive(!combatText.IsActive());
    }
    public void ChangeLootTextActive()
    {
        lootText.gameObject.SetActive(!lootText.IsActive());
    }
    public void ChangeDimensionTextActive()
    {
        dimensionText.gameObject.SetActive(!dimensionText.IsActive());
    }
    public void ChangeGoalTextActive()
    {
        goalText.gameObject.SetActive(!goalText.IsActive());
    }
}
