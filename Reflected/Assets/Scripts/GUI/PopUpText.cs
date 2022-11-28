using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private InWorldUIElement inWorldUIElement;
    [SerializeField] private Animator animator;

    [Header("Values")]
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private bool swing;

    public TextMeshProUGUI Text => text;
    public InWorldUIElement InWorldUIElement => inWorldUIElement;

    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    public float   Speed    { get { return speed;    } set { speed = value;    } }
    public float   Duration { get { return duration; } set { duration = value; } }
    public bool    Swing    { get { return swing;    } set { swing = value;    } }

    private void Start()
    {
        StartCoroutine(Coroutine_DelayedDestory());

        if (swing)
            animator.Play("Pop-Up Text Swing", 1, Random.Range(0f, 1f));
    }

    private IEnumerator Coroutine_DelayedDestory()
    {
        yield return new WaitForSeconds(duration);
        animator.SetTrigger("Destroy");
        yield return 0;
    }

    private void Update()
    {
        inWorldUIElement.ObjectToFollow.position += velocity * speed * Time.deltaTime;
    }

    // Gets called by destory animation
    public void Destroy()
    {
        Destroy(inWorldUIElement.ObjectToFollow.gameObject);
        Destroy(gameObject);
    }

}
