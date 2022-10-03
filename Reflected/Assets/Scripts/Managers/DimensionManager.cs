//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// DimensionManager description
/// </summary>
public class DimensionManager : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] VolumeProfile trueProfile;
    [SerializeField] VolumeProfile mirrorProfile;

    [SerializeField] List<ChangeableObject> changeableObjects;


    [Header("Charges")]
    [SerializeField] int maximumCharges;
    [SerializeField] int currentCharges;
    [SerializeField] int requiredChargesTrue;
    [SerializeField] int requiredChargesMirror;

    StatSystem statSystem;

    //public enum Dimension
    //{
    //    True, Mirror
    //};

    //public Dimension currentDimension;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetTrueDimension()
    {
        if (currentCharges < requiredChargesTrue)
            return;

        currentCharges -= requiredChargesTrue + statSystem.GetChargesToSwapTrue();

        volume.profile = trueProfile;

        foreach (ChangeableObject changeableObject in changeableObjects)
            changeableObject.ChangeToTrueMesh();
    }

    public void SetMirrorDimension()
    {
        if (currentCharges < requiredChargesMirror)
            return;

        currentCharges -= requiredChargesMirror + statSystem.GetChargesToSwapMirror();

        volume.profile = mirrorProfile;

        foreach (ChangeableObject changeableObject in changeableObjects)
            changeableObject.ChangeToMirrorMesh();
    }

    public void AddChangeableObject(ChangeableObject newObject)
    {
        changeableObjects.Add(newObject);
    }
    
    public bool CanSwapTrue()
    {
        return currentCharges >= requiredChargesTrue + statSystem.GetChargesToSwapTrue();
    }

    public bool CanSwapMirror()
    {
        return currentCharges >= requiredChargesMirror + statSystem.GetChargesToSwapMirror();
    }

    public void SetRequiredChargesTrue(int newCharges)
    {
        requiredChargesTrue = newCharges;
    }

    public void SetRequiredChargesMirror(int newCharges)
    {
        requiredChargesMirror = newCharges;
    }

    public void GainCharges(int addCharges)
    {
        currentCharges += Mathf.Clamp(addCharges, addCharges, maximumCharges-currentCharges);
    }

    public void SetStatSystem(StatSystem newStatSystem)
    {
        statSystem = newStatSystem;
    }

    public void UpdateChargeBar(GameObject go)
    {
        Slider chargeSlider = go.GetComponent<Slider>();
        TMP_Text sliderText = go.transform.Find("ChargeText").GetComponent<TMP_Text>();

        chargeSlider.value = (float)currentCharges / (float)maximumCharges;
        sliderText.text = currentCharges + " / " + maximumCharges;
    }
}