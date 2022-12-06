using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    protected UiManager uiManager;
    protected Player player;
    // Start is called before the first frame update
    void Start()
    {       
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        Player.OnObjectInteraction += Interact;
    }

    private void OnDisable()
    {
        Player.OnObjectInteraction -= Interact;
    }

    // Update is called once per frame
    protected virtual void Interact()
    {
        if (isInRange)
        {
            interactAction.Invoke();
            uiManager.ShowInteractText(false);
            //Debug.Log("interact key down...");

        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiManager = GameObject.FindGameObjectWithTag("Ui").GetComponent<UiManager>();
            isInRange = true;
            uiManager.ShowInteractText(true);
            //Debug.Log("Player now in range");
        }

    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            uiManager.ShowInteractText(false);
            //Debug.Log("Player now out of range");
        }
    }
}
