using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUi : MonoBehaviour
{
    [SerializeField] GameObject chargeObject;
    [SerializeField] GameObject chargeParent;
    [SerializeField] Color chargeColor;
    [SerializeField] float alpha;
    [SerializeField] Sprite chargeImage;
    private Color greyedChargeColor;
    private DimensionManager dimensionManager;
    List<GameObject> chargeList = new List<GameObject>();

    void Start()
    {
        
        greyedChargeColor = chargeColor;
        greyedChargeColor.a = alpha;
        dimensionManager = FindObjectOfType<DimensionManager>();
        CreateCharges();

    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateCharges();
    }

    public void UpdateCharges()
    {
        for (int i = 0; i < dimensionManager.GetMaxCharges(); i++)
        {
            Debug.Log(i.ToString());
            if (dimensionManager.GetCurrentCharges() > i)
            {
                chargeList[i].GetComponent<Image>().color = chargeColor;
            }
            else
                chargeList[i].GetComponent<Image>().color = greyedChargeColor;
        }
    }

    public void CreateCharges()
    {
        for (int i = 0; i < dimensionManager.GetMaxCharges(); i++)
        {
            chargeList.Add(Instantiate(chargeObject, chargeParent.transform));
        }
        for (int i = 0; i < dimensionManager.GetMaxCharges(); i++)
        {
            //chargeList[i].GetComponent<Image>().sprite = chargeImage;
            if (dimensionManager.GetCurrentCharges() > i)
            {
                chargeList[i].GetComponent<Image>().color = chargeColor;
            }
        }
    }



}
