//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// DimensionManager description
/// </summary>
public class DimensionManager : MonoBehaviour
{
    [Header("Post processing")]
    [SerializeField] private Volume volume;
    [SerializeField] private VolumeProfile trueProfile;
    [SerializeField] private VolumeProfile mirrorProfile;

    [Header("Lighting")]
    [SerializeField] private GameObject trueLighting;
    [SerializeField] private GameObject mirrorLighting;

    [Header("Skybox")]
    [SerializeField] private Material trueSkybox;
    [SerializeField] private Material mirrorSkybox;

    [Header("Changeable")]
    [SerializeField] private List<ChangeableObject> changeableObjects;

    [Header("Ability")]
    //[SerializeField] private GameObject chargeBar;
    [SerializeField] private int maximumCharges;
    [SerializeField] private int currentCharges;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private StatSystem statSystem;

    private static Dimension currentDimension;

    // Properties

    public static Dimension CurrentDimension => currentDimension;

    /// <summary>
    /// Returns true if the current dimension is True
    /// </summary>
    public static bool True => currentDimension == Dimension.True;

    /// <summary>
    /// Returns true if the current dimension is Mirror
    /// </summary>
    public static bool Mirror => currentDimension == Dimension.Mirror;

    private void Awake()
    {
        SetDimension(Dimension.True);
        //UpdateChargeBar();
    }

    /// <summary>
    /// Swaps dimension if fully charged. Returns whether or not the swap was successful
    /// </summary>
    public bool TrySwap()
    {
        if (!CanSwap())
            return false;

        ForcedSwap();
        ResetCharges();
        return true;
    }

    /// <summary>
    /// Swaps dimension without affecting charges.
    /// </summary>
    public void ForcedSwap()
    {
        SetDimension(Mirror ? Dimension.True : Dimension.Mirror);
    }

    /// <summary>
    /// Sets the current dimension to Mirror regardless of current charges
    /// </summary>
    public void SetDimension(Dimension dimension)
    {
        currentDimension = dimension;

        volume.profile = True ? trueProfile : mirrorProfile;

        trueLighting.SetActive(True);
        mirrorLighting.SetActive(Mirror);

        RenderSettings.skybox = True ? trueSkybox : mirrorSkybox;
        RenderSettings.fog = Mirror;

        foreach (ChangeableObject changeableObject in changeableObjects)
            changeableObject.UpdateMesh();
    }

    public void AddChangeableObject(ChangeableObject newObject)
    {
        changeableObjects.Add(newObject);
    }
    
    public bool CanSwap()
    {
        return currentCharges >= maximumCharges;
    }

    public void SetMaxCharges(int newCharges)
    {
        maximumCharges = newCharges;
        //UpdateChargeBar();
    }

    public int GetCurrentCharges()
    {
        return currentCharges;
    }

    public int GetMaxCharges()
    {
        return maximumCharges;
    }
    public void GainCharges(int addCharges)
    {
        currentCharges = Mathf.Clamp(currentCharges + addCharges, 0, maximumCharges);
        //UpdateChargeBar();
    }

    public void ResetCharges()
    {
        currentCharges = 0;
        //UpdateChargeBar();
    }

    public void SetStatSystem(StatSystem newStatSystem)
    {
        statSystem = newStatSystem;
    }

    //private void UpdateChargeBar()
    //{
    //    if (SceneManager.GetActiveScene().name == "Start Scene")
    //        return;

    //    Slider chargeSlider = chargeBar.GetComponent<Slider>();
    //    TMP_Text sliderText = chargeBar.transform.Find("ChargeText").GetComponent<TMP_Text>();

    //    chargeSlider.value = maximumCharges == 0 ? 1f : (float)currentCharges / (float)maximumCharges;
    //    sliderText.text = currentCharges + " / " + maximumCharges;
    //}

}