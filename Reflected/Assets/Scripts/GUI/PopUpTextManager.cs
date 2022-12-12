using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject popUpTextPrefab;

    private static PopUpTextManager popUpTextManager;

    private void Awake()
    {
        popUpTextManager = this;
    }

    public static PopUpText NewBasic(Vector3 position, string text)
    {
        GameObject objectToFollow = new GameObject("Pop-Up Text Position");
        objectToFollow.transform.position = position;
        objectToFollow.hideFlags = HideFlags.HideInHierarchy;

        PopUpText popUpText = Instantiate(popUpTextManager.popUpTextPrefab, popUpTextManager.transform).GetComponent<PopUpText>();
        popUpText.InWorldUIElement.ObjectToFollow = objectToFollow.transform;
        popUpText.Text.text = text;

        return popUpText;
    }

    public static PopUpText NewDamage(Vector3 position, float damage)
    {
        PopUpText popUpText = NewBasic(position, damage.ToString("0"));
        popUpText.Swing = true;
        popUpText.Text.color = new Color(1f, 0.3f, 0f);
        popUpText.Speed = 1.5f;
        return popUpText;
    }

    public static PopUpText NewHeal(Vector3 position, float heal)
    {
        PopUpText popUpText = NewBasic(position, heal.ToString("0"));
        popUpText.Swing = true;
        popUpText.Text.color = new Color(0f, 0.9f, 0f);
        return popUpText;
    }

}
