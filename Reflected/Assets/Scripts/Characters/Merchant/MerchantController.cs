using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    [SerializeField] GameObject merchant;

    private void Awake()
    {
        DimensionManager.DimensionChanged.AddListener(ToggleMerchantActive);
    }
    // Start is called before the first frame update
    void Start()
    {
        ToggleMerchantActive();
    }

    private void ToggleMerchantActive()
    {
        if (DimensionManager.CurrentDimension == Dimension.True)
        {
            merchant.SetActive(false);
        }
        else
        {
            merchant.SetActive(true);
        }
    }
}
